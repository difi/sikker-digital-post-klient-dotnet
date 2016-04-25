using System;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Common.Logging;
using Difi.Felles.Utility;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Aktører;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer.Forretning;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer.Transport;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Post;
using Difi.SikkerDigitalPost.Klient.Domene.Exceptions;
using Difi.SikkerDigitalPost.Klient.Envelope;
using Difi.SikkerDigitalPost.Klient.Envelope.Abstract;
using Difi.SikkerDigitalPost.Klient.Envelope.Forretningsmelding;
using Difi.SikkerDigitalPost.Klient.Envelope.Kvitteringsbekreftelse;
using Difi.SikkerDigitalPost.Klient.Envelope.Kvitteringsforespørsel;
using Difi.SikkerDigitalPost.Klient.Internal;
using Difi.SikkerDigitalPost.Klient.Internal.AsicE;
using Difi.SikkerDigitalPost.Klient.Utilities;
using Difi.SikkerDigitalPost.Klient.XmlValidering;

namespace Difi.SikkerDigitalPost.Klient.Api
{

    public class SikkerDigitalPostKlient : ISikkerDigitalPostKlient
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        
        /// <param name="databehandler">
        ///     Virksomhet (offentlig eller privat) som har en kontraktfestet avtale med Avsender med
        ///     formål å dekke hele eller deler av prosessen med å formidle en digital postmelding fra
        ///     Behandlingsansvarlig til Meldingsformidler. Det kan være flere databehandlere som har
        ///     ansvar for forskjellige steg i prosessen med å formidle en digital postmelding.
        /// </param>
        /// <param name="klientkonfigurasjon">
        ///     Klientkonfigurasjon for klienten. Brukes for å sette parametere
        ///     som proxy, timeout og URI til meldingsformidler. For å bruke standardkonfigurasjon, lag
        ///     SikkerDigitalPostKlient uten Klientkonfigurasjon som parameter.
        /// </param>
        /// <remarks>
        ///     Se <a href="http://begrep.difi.no/SikkerDigitalPost/forretningslag/Aktorer">oversikt over aktører</a>
        /// </remarks>
        public SikkerDigitalPostKlient(Databehandler databehandler, Klientkonfigurasjon klientkonfigurasjon)
        {
            Databehandler = databehandler;
            Klientkonfigurasjon = klientkonfigurasjon;
            RequestHelper = new RequestHelper(klientkonfigurasjon);
            FileUtility.BasePath = klientkonfigurasjon.StandardLoggSti;
        }

        public Databehandler Databehandler { get; }

        public Klientkonfigurasjon Klientkonfigurasjon { get; }

        internal RequestHelper RequestHelper { get; set; }

        /// <summary>
        ///     Sender en forsendelse til meldingsformidler. Dersom noe feilet i sendingen til meldingsformidler, vil det kastes en
        ///     exception.
        /// </summary>
        /// <param name="forsendelse">
        ///     Et objekt som har all informasjon klar til å kunne sendes (mottakerinformasjon, sertifikater,
        ///     vedlegg mm), enten digitalt eller fysisk.
        /// </param>
        /// <param name="lagreDokumentpakke">Hvis satt til true, så lagres dokumentpakken på Klientkonfigurasjon.StandardLoggSti.</param>
        public Transportkvittering Send(Forsendelse forsendelse, bool lagreDokumentpakke = false)
        {
            return SendAsync(forsendelse, lagreDokumentpakke).Result;
        }

        /// <summary>
        ///     Sender en forsendelse til meldingsformidler. Dersom noe feilet i sendingen til meldingsformidler, vil det kastes en
        ///     exception.
        /// </summary>
        /// <param name="forsendelse">
        ///     Et objekt som har all informasjon klar til å kunne sendes (mottakerinformasjon, sertifikater,
        ///     vedlegg mm), enten digitalt eller fysisk.
        /// </param>
        /// <param name="lagreDokumentpakke">Hvis satt til true, så lagres dokumentpakken på Klientkonfigurasjon.StandardLoggSti.</param>
        public async Task<Transportkvittering> SendAsync(Forsendelse forsendelse, bool lagreDokumentpakke = false)
        {
            var guidUtility = new GuidUtility();
            var documentBundle = AsiceGenerator.Create(forsendelse, guidUtility, Databehandler.Sertifikat, Klientkonfigurasjon.StandardLoggSti);
            var forretningsmeldingEnvelope = new ForretningsmeldingEnvelope(new EnvelopeSettings(forsendelse, documentBundle, Databehandler, guidUtility, Klientkonfigurasjon));

            ValidateEnvelopeAndThrowIfInvalid(forretningsmeldingEnvelope, $"konversasjonsid {forsendelse.KonversasjonsId}", new ForretningsmeldingEnvelopeValidator());
            
            var transportReceipt = (Transportkvittering) await RequestHelper.SendMessage(forretningsmeldingEnvelope, documentBundle);
            transportReceipt.AntallBytesDokumentpakke = documentBundle.BillableBytes;
            var transportReceiptXml = XmlUtility.TilXmlDokument(transportReceipt.Rådata);
            
            if (transportReceipt is TransportOkKvittering)
            {
                Log.Debug($"{transportReceipt}");

                var responsvalidator = new ResponseValidator(forretningsmeldingEnvelope.Xml(), transportReceiptXml, Klientkonfigurasjon.Miljø.CertificateChainValidator);
                responsvalidator.ValidateTransportReceipt(guidUtility);
            }
            else
            {
                Log.Warn($"{transportReceipt}");
            }

            return transportReceipt;
        }

        /// <summary>
        ///     Forespør kvittering for forsendelser. Kvitteringer blir tilgjengeliggjort etterhvert som de er klare i
        ///     meldingsformidler.
        ///     Det er ikke mulig å etterspørre kvittering for en spesifikk forsendelse.
        /// </summary>
        /// <param name="kvitteringsforespørsel"></param>
        /// <returns></returns>
        /// <remarks>
        ///     <list type="table">
        ///         <listheader>
        ///             <description>
        ///                 Dersom det ikke er tilgjengelige kvitteringer skal det ventes følgende tidsintervaller før en
        ///                 ny forespørsel gjøres
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
        ///     Forespør kvittering for forsendelser. Kvitteringer blir tilgjengeliggjort etterhvert som de er klare i
        ///     meldingsformidler.
        ///     Det er ikke mulig å etterspørre kvittering for en spesifikk forsendelse.
        /// </summary>
        /// <param name="kvitteringsforespørsel"></param>
        /// <returns></returns>
        /// <remarks>
        ///     <list type="table">
        ///         <listheader>
        ///             <description>
        ///                 Dersom det ikke er tilgjengelige kvitteringer skal det ventes følgende tidsintervaller før en
        ///                 ny forespørsel gjøres
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
            return await HentKvitteringOgBekreftForrigeAsync(kvitteringsforespørsel, null);
        }

        /// <summary>
        ///     Forespør kvittering for forsendelser med mulighet til å samtidig bekrefte på forrige kvittering for å slippe å
        ///     kjøre eget kall for bekreft.
        ///     Kvitteringer blir tilgjengeliggjort etterhvert som de er klare i meldingsformidler. Det er ikke mulig å etterspørre
        ///     kvittering for en
        ///     spesifikk forsendelse.
        /// </summary>
        /// <param name="kvitteringsforespørsel"></param>
        /// <param name="forrigeKvittering"></param>
        /// <returns></returns>
        /// <remarks>
        ///     <list type="table">
        ///         <listheader>
        ///             <description>
        ///                 Dersom det ikke er tilgjengelige kvitteringer skal det ventes følgende tidsintervaller før en
        ///                 ny forespørsel gjøres
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
        ///     Forespør kvittering for forsendelser med mulighet til å samtidig bekrefte på forrige kvittering for å slippe å
        ///     kjøre eget kall for bekreft.
        ///     Kvitteringer blir tilgjengeliggjort etterhvert som de er klare i meldingsformidler. Det er ikke mulig å etterspørre
        ///     kvittering for en
        ///     spesifikk forsendelse.
        /// </summary>
        /// <param name="kvitteringsforespørsel"></param>
        /// <param name="forrigeKvittering"></param>
        /// <returns></returns>
        /// <remarks>
        ///     <list type="table">
        ///         <listheader>
        ///             <description>
        ///                 Dersom det ikke er tilgjengelige kvitteringer skal det ventes følgende tidsintervaller før en
        ///                 ny forespørsel gjøres
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
                await BekreftAsync(forrigeKvittering);
            }

            var guidUtility = new GuidUtility();
            var envelopeSettings = new EnvelopeSettings(kvitteringsforespørsel, Databehandler, guidUtility);
            var kvitteringsforespørselEnvelope = new KvitteringsforespørselEnvelope(envelopeSettings);

            ValidateEnvelopeAndThrowIfInvalid(kvitteringsforespørselEnvelope, "", new KvitteringsforespørselEnvelopeValidator());

            var transportReceipt = await RequestHelper.GetReceipt(kvitteringsforespørselEnvelope);
            var transportReceiptXml = XmlUtility.TilXmlDokument(transportReceipt.Rådata);

            if (transportReceipt is TomKøKvittering)
            {
                Log.Debug($"{transportReceipt}");
                SecurityValidationOfEmptyQueueReceipt(transportReceiptXml, kvitteringsforespørselEnvelope.Xml());
            }
            else if (transportReceipt is Forretningskvittering)
            {
                Log.Debug($"{transportReceipt}");
                SecurityValidationOfMessageReceipt(transportReceiptXml, kvitteringsforespørselEnvelope);
            }

            return transportReceipt;
        }

        private void SecurityValidationOfEmptyQueueReceipt(XmlDocument kvittering, XmlDocument forretningsmelding)
        {
            var responseValidator = new ResponseValidator(forretningsmelding, kvittering, Klientkonfigurasjon.Miljø.CertificateChainValidator);
            responseValidator.ValidateEmptyQueueReceipt();
        }

        private void SecurityValidationOfMessageReceipt(XmlDocument kvittering, KvitteringsforespørselEnvelope kvitteringsforespørselEnvelope)
        {
            var valideringAvResponsSignatur = new ResponseValidator(kvitteringsforespørselEnvelope.Xml(), kvittering, Klientkonfigurasjon.Miljø.CertificateChainValidator);
            valideringAvResponsSignatur.ValidateMessageReceipt();
        }

        /// <summary>
        ///     Bekreft mottak av forretningskvittering gjennom <see cref="HentKvittering(Kvitteringsforespørsel)" />.
        ///     <list type="bullet">
        ///         <listheader>
        ///             <description>
        ///                 <para>Dette legger opp til følgende arbeidsflyt</para>
        ///             </description>
        ///         </listheader>
        ///         <item>
        ///             <description>
        ///                 <para>
        ///                     <see cref="HentKvittering(Kvitteringsforespørsel)" />
        ///                 </para>
        ///             </description>
        ///         </item>
        ///         <item>
        ///             <description>
        ///                 <para>Gjør intern prosessering av kvitteringen (lagre til database, og så videre)</para>
        ///             </description>
        ///         </item>
        ///         <item>
        ///             <description>
        ///                 <para>Bekreft mottak av kvittering</para>
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
        ///     Bekreft mottak av forretningskvittering gjennom <see cref="HentKvittering(Kvitteringsforespørsel)" />.
        ///     <list type="bullet">
        ///         <listheader>
        ///             <description>
        ///                 <para>Dette legger opp til følgende arbeidsflyt</para>
        ///             </description>
        ///         </listheader>
        ///         <item>
        ///             <description>
        ///                 <para>
        ///                     <see cref="HentKvittering(Kvitteringsforespørsel)" />
        ///                 </para>
        ///             </description>
        ///         </item>
        ///         <item>
        ///             <description>
        ///                 <para>Gjør intern prosessering av kvitteringen (lagre til database, og så videre)</para>
        ///             </description>
        ///         </item>
        ///         <item>
        ///             <description>
        ///                 <para>Bekreft mottak av kvittering</para>
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

            var receivedReceiptValidator = new KvitteringMottattEnvelopeValidator();
            ValidateEnvelopeAndThrowIfInvalid(bekreftKvitteringEnvelope, $"konversasjonsid {kvittering.KonversasjonsId}", receivedReceiptValidator);

            await RequestHelper.ConfirmReceipt(bekreftKvitteringEnvelope);
        }

        private static void ValidateEnvelopeAndThrowIfInvalid(AbstractEnvelope envelope, string description, XmlValidator envelopeValidator)
        {
            var isValid = envelopeValidator.Validate(envelope.Xml().OuterXml);
            if (!isValid)
            {
                var validatorName = envelopeValidator.GetType().Name;
                var errorDescription = $"{validatorName}: Ikke gyldig respons for {description}. {envelopeValidator.ValidationWarnings}";

                Log.Warn(errorDescription);
                throw new XmlValidationException(errorDescription);
            }
        }
    }
}