using System.Threading.Tasks;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Aktører;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer.Forretning;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer.Transport;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Post;
using Difi.SikkerDigitalPost.Klient.Internal;
using Difi.SikkerDigitalPost.Klient.Utilities;
using Difi.SikkerDigitalPost.Klient.XmlValidering;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using StandardBusinessDocument = Difi.SikkerDigitalPost.Klient.SBDH.StandardBusinessDocument;

namespace Difi.SikkerDigitalPost.Klient.Api
{
    public class SikkerDigitalPostKlient : ISikkerDigitalPostKlient
    {
        private readonly ILogger<SikkerDigitalPostKlient> _logger;
        private readonly ILoggerFactory _loggerFactory;

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
        public SikkerDigitalPostKlient(Databehandler databehandler, Klientkonfigurasjon klientkonfigurasjon,
            ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<SikkerDigitalPostKlient>();
            _loggerFactory = loggerFactory;

            //ValidateDatabehandlerCertificateAndThrowIfInvalid(databehandler, klientkonfigurasjon.Miljø);

            Databehandler = databehandler;
            Klientkonfigurasjon = klientkonfigurasjon;
            RequestHelper = new RequestHelper(klientkonfigurasjon, _loggerFactory);
            //CertificateValidationProperties = new CertificateValidationProperties(klientkonfigurasjon.Miljø.GodkjenteKjedeSertifikater, Klientkonfigurasjon.MeldingsformidlerOrganisasjon);
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
            StandardBusinessDocument standardBusinessDocument = SBDForsendelseBuilder.BuildSBD(Databehandler, forsendelse);

            return await RequestHelper.SendMessage(standardBusinessDocument, forsendelse.Dokumentpakke,
                forsendelse.MetadataDocument);
        }

        /// <summary>
        ///     Forespør <see cref="Kvittering" /> for <see cref="Forsendelse">Forsendelser</see>.
        ///     <see cref="Kvittering">Kvitteringer</see> blir tilgjengeliggjort etterhvert som de er klare i
        ///     Meldingsformidler. Det er ikke mulig å etterspørre <see cref="Kvittering" /> for en spesifikk
        ///     <see cref="Forsendelse" />.
        /// </summary>
        /// <param name="kvitteringsforespørsel"></param>
        /// <returns></returns>
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
        public async Task<Kvittering> HentKvitteringOgBekreftForrigeAsync(Kvitteringsforespørsel kvitteringsforespørsel,
            Forretningskvittering forrigeKvittering)
        {
            if (forrigeKvittering != null)
            {
                await BekreftAsync(forrigeKvittering).ConfigureAwait(false);
            }

            var guidUtility = new GuidUtility();

            _logger.LogDebug($"Utgående kvitteringsforespørsel, messageId '{guidUtility.MessageId}'.");


            int GUARD = 100;
            for (int i = 0; i < GUARD; i++)
            {
                IntegrasjonspunktKvittering ipKvittering = await RequestHelper.GetReceipt();

                if (ipKvittering == null)
                {
                    return new TomKøKvittering();
                }

                var shouldFetchKvitteringAgain =
                    ipKvittering.status == IntegrasjonspunktKvitteringType.SENDT ||
                    ipKvittering.status == IntegrasjonspunktKvitteringType.OPPRETTET;

                if (shouldFetchKvitteringAgain)
                {
                    await RequestHelper.ConfirmReceipt(ipKvittering.id);
                }
                else
                {
                    return KvitteringFactory.GetKvittering(ipKvittering);
                }
            }

            _logger.LogWarning(
                $"Antall forsøk på å hente kvittering overskredet. Det kan komme av det er mange {IntegrasjonspunktKvitteringType.SENDT}- og {IntegrasjonspunktKvitteringType.OPPRETTET}-kvitteringer på integrasjonspunktkøen. Prøv igjen.");
            return null;
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
            if (kvittering == null || kvittering.IntegrasjonsPunktId == -1L)
            {
                IntegrasjonspunktKvittering nyKvittering = await RequestHelper.GetReceipt();

                await RequestHelper.ConfirmReceipt(nyKvittering.id);
                _logger.LogDebug($"Bekreftet kvittering, conversationId '{nyKvittering.conversationId}'");
            }
            else
            {
                await RequestHelper.ConfirmReceipt(kvittering.IntegrasjonsPunktId);
                _logger.LogDebug($"Bekreftet kvittering, conversationId '{kvittering.KonversasjonsId}'");
            }
        }
    }
}
