using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Aktører;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer.Forretning;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer.Transport;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Post;
using Difi.SikkerDigitalPost.Klient.Domene.Exceptions;
using Difi.SikkerDigitalPost.Klient.Domene.Extensions;
using Difi.SikkerDigitalPost.Klient.Envelope;
using Difi.SikkerDigitalPost.Klient.Envelope.Abstract;
using Difi.SikkerDigitalPost.Klient.Envelope.Forretningsmelding;
using Difi.SikkerDigitalPost.Klient.Envelope.Kvitteringsbekreftelse;
using Difi.SikkerDigitalPost.Klient.Envelope.Kvitteringsforespørsel;
using Difi.SikkerDigitalPost.Klient.Internal;
using Difi.SikkerDigitalPost.Klient.Internal.AsicE;
using Difi.SikkerDigitalPost.Klient.Utilities;
using Difi.SikkerDigitalPost.Klient.XmlValidering;
using Digipost.Api.Client.Shared.Certificate;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Difi.SikkerDigitalPost.Klient.Api
{
    public class SikkerDigitalPostKlient : ISikkerDigitalPostKlient
    {
        private readonly ILogger<SikkerDigitalPostKlient> _logger;
        private readonly ILoggerFactory _loggerFactory;
        
        internal CertificateValidationProperties CertificateValidationProperties { get; set; }

        /// <param name="databehandler">
        ///     Virksomhet (offentlig eller privat) som har en kontraktfestet avtale med Avsender med
        ///     formål å dekke hele eller deler av prosessen med å formidle en digital postmelding fra
        ///     <see cref="Avsender" /> til Meldingsformidler.
        /// </param>
        /// <param name="klientkonfigurasjon">
        ///     Brukes for å sette parametere som proxy, timeout, logging av forespørsel/respons og selve dokumentpakken.
        /// </param>
        /// <remarks>
        ///     Se <a href="http://begrep.difi.no/SikkerDigitalPost/forretningslag/Aktorer">oversikt over aktører</a>
        /// </remarks>
        public SikkerDigitalPostKlient(Databehandler databehandler, Klientkonfigurasjon klientkonfigurasjon) 
            : this(databehandler, klientkonfigurasjon, new NullLoggerFactory())
        {
        }
        
        /// <param name="databehandler">
        ///     Virksomhet (offentlig eller privat) som har en kontraktfestet avtale med Avsender med
        ///     formål å dekke hele eller deler av prosessen med å formidle en digital postmelding fra
        ///     <see cref="Avsender" /> til Meldingsformidler.
        /// </param>
        /// <param name="klientkonfigurasjon">
        ///     Brukes for å sette parametere som proxy, timeout, logging av forespørsel/respons og selve dokumentpakken.
        /// </param>
        /// <param name="loggerFactory">
        ///     Brukes for å sette loggeren.
        /// </param>
        /// <remarks>
        ///     Se <a href="http://begrep.difi.no/SikkerDigitalPost/forretningslag/Aktorer">oversikt over aktører</a>
        /// </remarks>
        public SikkerDigitalPostKlient(Databehandler databehandler, Klientkonfigurasjon klientkonfigurasjon, ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<SikkerDigitalPostKlient>();
            _loggerFactory = loggerFactory;
            
            ValidateDatabehandlerCertificateAndThrowIfInvalid(databehandler, klientkonfigurasjon.Miljø);

            Databehandler = databehandler;
            Klientkonfigurasjon = klientkonfigurasjon;
            RequestHelper = new RequestHelper(klientkonfigurasjon, _loggerFactory);
            CertificateValidationProperties = new CertificateValidationProperties(klientkonfigurasjon.Miljø.GodkjenteKjedeSertifikater, Klientkonfigurasjon.MeldingsformidlerOrganisasjon);
        }

        private void ValidateDatabehandlerCertificateAndThrowIfInvalid(Databehandler databehandler, Miljø miljø)
        {
            var valideringsResultat = CertificateValidator.ValidateCertificateAndChain(
                databehandler.Sertifikat, 
                databehandler.Organisasjonsnummer.Verdi, 
                miljø.GodkjenteKjedeSertifikater
            );

            if (valideringsResultat.Type != CertificateValidationType.Valid)
            {
                throw new SecurityException($"Sertifikatet som brukes for { nameof(Databehandler) } er ikke gyldig. Prøver du å sende med et testsertifikat i produksjonsmiljø eller omvendt? Grunnen er '{valideringsResultat.Type.ToNorwegianString()}', beskrivelse: '{valideringsResultat.Message}'");

            }
        }

        public Databehandler Databehandler { get; }

        public Klientkonfigurasjon Klientkonfigurasjon { get; }

        internal RequestHelper RequestHelper { get; set; }

        /// <summary>
        ///     Sender en <see cref="Forsendelse" /> til Meldingsformidler.
        /// </summary>
        /// <param name="forsendelse">
        ///     All informasjon, klar til å kunne sendes (mottakerinformasjon, sertifikater,
        ///     vedlegg mm), enten digitalt eller fysisk.
        /// </param>
        public Transportkvittering Send(Forsendelse forsendelse)
        {
            return SendAsync(forsendelse).Result;
        }

        /// <summary>
        ///     Sender en <see cref="Forsendelse" /> til Meldingsformidler.
        /// </summary>
        /// <param name="forsendelse">
        ///     All informasjon, klar til å kunne sendes (mottakerinformasjon, sertifikater,
        ///     vedlegg mm), enten digitalt eller fysisk.
        /// </param>
        public async Task<Transportkvittering> SendAsync(Forsendelse forsendelse)
        {
            var guidUtility = new GuidUtility();
            _logger.LogDebug($"Utgående forsendelse, conversationId '{forsendelse.KonversasjonsId}', messageId '{guidUtility.MessageId}'.");

            var documentBundle = AsiceGenerator.Create(forsendelse, guidUtility, Databehandler.Sertifikat, Klientkonfigurasjon);
            var forretningsmeldingEnvelope = new ForretningsmeldingEnvelope(new EnvelopeSettings(forsendelse, documentBundle, Databehandler, guidUtility, Klientkonfigurasjon));

            ValidateEnvelopeAndThrowIfInvalid(forretningsmeldingEnvelope, forretningsmeldingEnvelope.GetType().Name);

            var transportReceipt = (Transportkvittering) await RequestHelper.SendMessage(forretningsmeldingEnvelope, documentBundle).ConfigureAwait(false);
            transportReceipt.AntallBytesDokumentpakke = documentBundle.BillableBytes;
            var transportReceiptXml = transportReceipt.Xml;

            if (transportReceipt is TransportOkKvittering)
            {
                _logger.LogDebug($"{transportReceipt}");

                var responsvalidator = new ResponseValidator(forretningsmeldingEnvelope.Xml(), transportReceiptXml, CertificateValidationProperties);
                responsvalidator.ValidateTransportReceipt(guidUtility);
            }
            else
            {
                _logger.LogError(($"{transportReceipt}"));
            }

            return transportReceipt;
        }

        /// <summary>
        ///     Forespør <see cref="Kvittering" /> for <see cref="Forsendelse">Forsendelser</see>.
        ///     <see cref="Kvittering">Kvitteringer</see> blir tilgjengeliggjort etterhvert som de er klare i
        ///     Meldingsformidler. Det er ikke mulig å etterspørre <see cref="Kvittering" /> for en spesifikk
        ///     <see cref="Forsendelse" />.
        /// </summary>
        /// <param name="kvitteringsforespørsel"></param>
        /// <returns></returns>
        /// <remarks>
        ///     <list type="table">
        ///         <listheader>
        ///             <description>
        ///                 Dersom det ikke er tilgjengelige <see cref="Kvittering">Kvitteringer</see> skal det ventes følgende
        ///                 tidsintervaller før en ny forespørsel gjøres:
        ///             </description>
        ///         </listheader>
        ///         <item>
        ///             <term>normal</term><description>Minimum 10 minutter</description>
        ///         </item>
        ///         <item>
        ///             <term>prioritert</term><description>Minimum 1 minutt</description>
        ///         </item>
        ///     </list>
        /// </remarks>
        public Kvittering HentKvittering(Kvitteringsforespørsel kvitteringsforespørsel)
        {
            return HentKvitteringOgBekreftForrige(kvitteringsforespørsel, null);
        }

        /// <summary>
        ///     Forespør <see cref="Kvittering" /> for <see cref="Forsendelse">Forsendelser</see>.
        ///     <see cref="Kvittering">Kvitteringer</see> blir tilgjengeliggjort etterhvert som de er klare i
        ///     Meldingsformidler. Det er ikke mulig å etterspørre <see cref="Kvittering" /> for en spesifikk
        ///     <see cref="Forsendelse" />.
        /// </summary>
        /// <param name="kvitteringsforespørsel"></param>
        /// <returns></returns>
        /// <remarks>
        ///     <list type="table">
        ///         <listheader>
        ///             <description>
        ///                 Dersom det ikke er tilgjengelige <see cref="Kvittering">Kvitteringer</see> skal det ventes følgende
        ///                 tidsintervaller før en ny forespørsel gjøres:
        ///             </description>
        ///         </listheader>
        ///         <item>
        ///             <term>normal</term><description>Minimum 10 minutter</description>
        ///         </item>
        ///         <item>
        ///             <term>prioritert</term><description>Minimum 1 minutt</description>
        ///         </item>
        ///     </list>
        /// </remarks>
        public async Task<Kvittering> HentKvitteringAsync(Kvitteringsforespørsel kvitteringsforespørsel)
        {
            return await HentKvitteringOgBekreftForrigeAsync(kvitteringsforespørsel, null).ConfigureAwait(false);
        }

        /// <summary>
        ///     Forespør <see cref="Kvittering" /> for <see cref="Forsendelse">Forsendelser</see>, med mulighet til å samtidig
        ///     <see cref="Bekreft">bekrefte</see> på forrige <see cref="Kvittering" /> for å slippe å
        ///     kjøre eget kall for <see cref="Bekreft" />. <see cref="Kvittering">Kvitteringer</see> blir tilgjengeliggjort
        ///     etterhvert som de er klare i Meldingsformidler. Det er ikke mulig å etterspørre
        ///     <see cref="Kvittering" /> for en spesifikk forsendelse.
        /// </summary>
        /// <param name="kvitteringsforespørsel"></param>
        /// <param name="forrigeKvittering"></param>
        /// <returns></returns>
        /// <remarks>
        ///     <list type="table">
        ///         <listheader>
        ///             <description>
        ///                 Dersom det ikke er tilgjengelige <see cref="Kvittering">Kvitteringer</see> skal det ventes følgende
        ///                 tidsintervaller før en ny forespørsel gjøres:
        ///             </description>
        ///         </listheader>
        ///         <item>
        ///             <term>normal</term><description>Minimum 10 minutter</description>
        ///         </item>
        ///         <item>
        ///             <term>prioritert</term><description>Minimum 1 minutt</description>
        ///         </item>
        ///     </list>
        /// </remarks>
        public Kvittering HentKvitteringOgBekreftForrige(Kvitteringsforespørsel kvitteringsforespørsel,
            Forretningskvittering forrigeKvittering)
        {
            return HentKvitteringOgBekreftForrigeAsync(kvitteringsforespørsel, forrigeKvittering).Result;
        }

        /// <summary>
        ///     Forespør <see cref="Kvittering" /> for <see cref="Forsendelse">Forsendelser</see>, med mulighet til å samtidig
        ///     <see cref="BekreftAsync">bekrefte</see> på forrige <see cref="Kvittering" /> for å slippe å
        ///     kjøre eget kall for <see cref="BekreftAsync" />. <see cref="Kvittering">Kvitteringer</see> blir tilgjengeliggjort
        ///     etterhvert som de er klare i Meldingsformidler. Det er ikke mulig å etterspørre
        ///     <see cref="Kvittering" /> for en spesifikk forsendelse.
        /// </summary>
        /// <param name="kvitteringsforespørsel"></param>
        /// <param name="forrigeKvittering"></param>
        /// <returns></returns>
        /// <remarks>
        ///     <list type="table">
        ///         <listheader>
        ///             <description>
        ///                 Dersom det ikke er tilgjengelige <see cref="Kvittering">Kvitteringer</see> skal det ventes følgende
        ///                 tidsintervaller før en ny forespørsel gjøres:
        ///             </description>
        ///         </listheader>
        ///         <item>
        ///             <term>normal</term><description>Minimum 10 minutter</description>
        ///         </item>
        ///         <item>
        ///             <term>prioritert</term><description>Minimum 1 minutt</description>
        ///         </item>
        ///     </list>
        /// </remarks>
        public async Task<Kvittering> HentKvitteringOgBekreftForrigeAsync(Kvitteringsforespørsel kvitteringsforespørsel, Forretningskvittering forrigeKvittering)
        {
            if (forrigeKvittering != null)
            {
                await BekreftAsync(forrigeKvittering).ConfigureAwait(false);
            }

            var guidUtility = new GuidUtility();

            _logger.LogDebug($"Utgående kvitteringsforespørsel, messageId '{guidUtility.MessageId}'.");

            var envelopeSettings = new EnvelopeSettings(kvitteringsforespørsel, Databehandler, guidUtility);
            var kvitteringsforespørselEnvelope = new KvitteringsforespørselEnvelope(envelopeSettings);

            ValidateEnvelopeAndThrowIfInvalid(kvitteringsforespørselEnvelope, kvitteringsforespørselEnvelope.GetType().Name);

            var receipt = await RequestHelper.GetReceipt(kvitteringsforespørselEnvelope).ConfigureAwait(false);
            var transportReceiptXml = receipt.Xml;

            if (receipt is TomKøKvittering)
            {
                _logger.LogDebug($"{receipt}");
                SecurityValidationOfEmptyQueueReceipt(transportReceiptXml, kvitteringsforespørselEnvelope.Xml());
            }
            else if (receipt is Forretningskvittering)
            {
                _logger.LogDebug($"{receipt}");
                SecurityValidationOfMessageReceipt(transportReceiptXml, kvitteringsforespørselEnvelope);
            }

            else if (receipt is Transportkvittering)
            {
                _logger.LogDebug($"{receipt}");
            }

            return receipt;
        }

        /// <summary>
        ///     Bekreft mottak av <see cref="Forretningskvittering" /> mottatt gjennom
        ///     <see cref="HentKvittering(Kvitteringsforespørsel)" />.
        ///     <list type="bullet">
        ///         <listheader>
        ///             <description>
        ///                 <para>Dette legger opp til følgende arbeidsflyt:</para>
        ///             </description>
        ///         </listheader>
        ///         <item>
        ///             <description>
        ///                 <para>
        ///                     <see cref="HentKvittering(Kvitteringsforespørsel)" />.
        ///                 </para>
        ///             </description>
        ///         </item>
        ///         <item>
        ///             <description>
        ///                 <para>Gjør intern prosessering av <see cref="Kvittering">Kvitteringen</see>.</para>
        ///             </description>
        ///         </item>
        ///         <item>
        ///             <description>
        ///                 <para><see cref="Bekreft">Bekreft</see> mottak av <see cref="Forretningskvittering" />.</para>
        ///             </description>
        ///         </item>
        ///     </list>
        /// </summary>
        /// <param name="kvittering"></param>
        /// <remarks>
        ///     <see cref="HentKvittering(Kvitteringsforespørsel)" /> kommer ikke til å returnere en ny kvittering før mottak av
        ///     den forrige er bekreftet.
        /// </remarks>
        public void Bekreft(Forretningskvittering kvittering)
        {
            BekreftAsync(kvittering).Wait();
        }

        /// <summary>
        ///     Bekreft mottak av <see cref="Forretningskvittering" /> mottatt gjennom
        ///     <see cref="HentKvittering(Kvitteringsforespørsel)" />.
        ///     <list type="bullet">
        ///         <listheader>
        ///             <description>
        ///                 <para>Dette legger opp til følgende arbeidsflyt:</para>
        ///             </description>
        ///         </listheader>
        ///         <item>
        ///             <description>
        ///                 <para>
        ///                     <see cref="HentKvittering(Kvitteringsforespørsel)" />.
        ///                 </para>
        ///             </description>
        ///         </item>
        ///         <item>
        ///             <description>
        ///                 <para>Gjør intern prosessering av <see cref="Kvittering">Kvitteringen</see>.</para>
        ///             </description>
        ///         </item>
        ///         <item>
        ///             <description>
        ///                 <para><see cref="BekreftAsync">Bekreft</see> mottak av <see cref="Forretningskvittering" />.</para>
        ///             </description>
        ///         </item>
        ///     </list>
        /// </summary>
        /// <param name="kvittering"></param>
        /// <remarks>
        ///     <see cref="HentKvittering(Kvitteringsforespørsel)" /> kommer ikke til å returnere en ny kvittering før mottak av
        ///     den forrige er bekreftet.
        /// </remarks>
        public async Task BekreftAsync(Forretningskvittering kvittering)
        {
            var envelopeSettings = new EnvelopeSettings(kvittering, Databehandler, new GuidUtility());
            var bekreftKvitteringEnvelope = new KvitteringsbekreftelseEnvelope(envelopeSettings);

            ValidateEnvelopeAndThrowIfInvalid(bekreftKvitteringEnvelope, bekreftKvitteringEnvelope.GetType().Name);

            await RequestHelper.ConfirmReceipt(bekreftKvitteringEnvelope).ConfigureAwait(false);
            _logger.LogDebug($"Bekreftet kvittering, conversationId '{kvittering.KonversasjonsId}'");
        }

        private void SecurityValidationOfEmptyQueueReceipt(XmlDocument kvittering, XmlDocument forretningsmelding)
        {
            var responseValidator = new ResponseValidator(forretningsmelding, kvittering, CertificateValidationProperties);
            responseValidator.ValidateEmptyQueueReceipt();
        }

        private void SecurityValidationOfMessageReceipt(XmlDocument kvittering, KvitteringsforespørselEnvelope kvitteringsforespørselEnvelope)
        {
            var valideringAvResponsSignatur = new ResponseValidator(kvitteringsforespørselEnvelope.Xml(), kvittering, CertificateValidationProperties);
            valideringAvResponsSignatur.ValidateMessageReceipt();
        }

        private void ValidateEnvelopeAndThrowIfInvalid(AbstractEnvelope envelope, string prefix)
        {
            List<string> validationMessages;
            var isValid = SdpXmlValidator.Instance.Validate(envelope.Xml().OuterXml, out validationMessages);
            if (!isValid)
            {
                var errorDescription = $"Ikke gyldig innhold i {prefix}. {validationMessages.Aggregate((current, variable) => current + Environment.NewLine + variable)}";
                _logger.LogWarning(errorDescription);
                throw new XmlValidationException(errorDescription, validationMessages);
            }
        }
    }
}