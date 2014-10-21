using System;
using System.IO;
using System.Net;
using System.Xml;
using System.Xml.Linq;
using SikkerDigitalPost.Domene.Entiteter.Aktører;
using SikkerDigitalPost.Domene.Entiteter.Kvitteringer;
using SikkerDigitalPost.Domene.Entiteter.Post;
using SikkerDigitalPost.Domene.Exceptions;
using SikkerDigitalPost.Klient.AsicE;
using SikkerDigitalPost.Klient.Envelope;
using SikkerDigitalPost.Klient.Utilities;
using SikkerDigitalPost.Klient.XmlValidering;

namespace SikkerDigitalPost.Klient
{
    public class SikkerDigitalPostKlient
    {
        private readonly Databehandler _databehandler;
        private readonly Klientkonfigurasjon _konfigurasjon;


        /// <param name="databehandler">
        /// Teknisk avsender er den parten som har ansvarlig for den tekniske utførelsen av sendingen.
        /// Teknisk avsender er den aktøren som står for utførelsen av den tekniske sendingen. 
        /// Hvis sendingen utføres av en databehandler vil dette være databehandleren. 
        /// Hvis sendingen utføres av behandlingsansvarlige selv er dette den behandlingsansvarlige.
        /// </param>
        /// <remarks>
        /// Se <a href="http://begrep.difi.no/SikkerDigitalPost/forretningslag/Aktorer">oversikt over aktører</a>
        /// </remarks>
        public SikkerDigitalPostKlient(Databehandler databehandler) : this (databehandler,new Klientkonfigurasjon())
        {
            
        }


        /// <param name="databehandler">
        /// Teknisk avsender er den parten som har ansvarlig for den tekniske utførelsen av sendingen.
        /// Teknisk avsender er den aktøren som står for utførelsen av den tekniske sendingen. 
        /// Hvis sendingen utføres av en databehandler vil dette være databehandleren. 
        /// Hvis sendingen utføres av behandlingsansvarlige selv er dette den behandlingsansvarlige.
        /// </param>
        /// <param name="konfigurasjon">Klientkonfigurasjon for klienten. Brukes for å sette parametere
        /// som proxy, timeout og URI til meldingsformidler. For å bruke standardkonfigurasjon, lag
        /// SikkerDigitalPostKlient uten Klientkonfigurasjon som parameter.</param>
        /// <remarks>
        /// Se <a href="http://begrep.difi.no/SikkerDigitalPost/forretningslag/Aktorer">oversikt over aktører</a>
        /// </remarks>
        public SikkerDigitalPostKlient(Databehandler databehandler, Klientkonfigurasjon konfigurasjon)
        {
            _databehandler = databehandler;
            _konfigurasjon = konfigurasjon;
        }


        /// <summary>
        /// Sender en forsendelse til meldingsformidler. Dersom noe feilet i sendingen til meldingsformidler, vil det kastes en exception.
        /// </summary>
        /// <param name="forsendelse">Et objekt som har all informasjon klar til å kunne sendes (mottakerinformasjon, sertifikater, vedlegg mm), enten digitalt eller fysisk.</param>
        public Transportkvittering Send(Forsendelse forsendelse)
        {
            var guidHandler = new GuidHandler();
            var arkiv = new AsicEArkiv(forsendelse, guidHandler, _databehandler.Sertifikat);
            var forretningsmeldingEnvelope = new ForretningsmeldingEnvelope(new EnvelopeSettings(forsendelse, arkiv, _databehandler, guidHandler));

            try
            {
                ValiderForretningsmeldingEnvelope(forretningsmeldingEnvelope.Xml(), arkiv.Manifest.Xml(), arkiv.Signatur.Xml());
            }
            catch (Exception e)
            {
                throw new XmlValidationException("Envelope xml validerer ikke mot xsd:", e);
            }

            var soapContainer = new SoapContainer {Envelope = forretningsmeldingEnvelope, Action = "\"\""};
            soapContainer.Vedlegg.Add(arkiv);
            var response = SendSoapContainer(soapContainer);
#if DEBUG
            FileUtility.WriteXmlToFileInBasePath(forretningsmeldingEnvelope.Xml().OuterXml, "Forretningsmelding.xml");
            FileUtility.WriteXmlToFileInBasePath(response, "ForrigeKvittering.xml");
            FileUtility.WriteXmlToFileInBasePath(arkiv.Signatur.Xml().OuterXml, "Signatur.xml");
            FileUtility.WriteXmlToFileInBasePath(arkiv.Manifest.Xml().OuterXml, "Manifest.xml");
#endif
            
            try
            {
                var valideringAvRespons = new Responsvalidator();
                valideringAvRespons.ValiderRespons(response, forretningsmeldingEnvelope.Xml(), guidHandler);
            }
            catch (Exception e)
            {
                throw new SendException("Validering av respons fra meldingsformidler feilet. Se inner exception for detaljer.\n", e);
            }

            return KvitteringFactory.GetTransportkvittering(response);
        }


        /// <summary>
        /// Forespør kvittering for forsendelser. Kvitteringer blir tilgjengeliggjort etterhvert som de er klare i meldingsformidler.
        /// Det er ikke mulig å etterspørre kvittering for en spesifikk forsendelse.
        /// </summary>
        /// <param name="kvitteringsforespørsel"></param>
        /// <returns></returns>
        /// <remarks>
        /// <list type="table">
        /// <listheader><description>Dersom det ikke er tilgjengelige kvitteringer skal det ventes følgende tidsintervaller før en ny forespørsel gjøres</description></listheader>
        /// <item><term>normal</term><description>Minimum 10 minutter</description></item>
        /// <item><term>prioritert</term><description>Minimum 1 minutt</description></item>
        /// </list>
        /// </remarks>
        public Forretningskvittering HentKvittering(Kvitteringsforespørsel kvitteringsforespørsel)
        {
            return HentKvitteringOgBekreftForrige(kvitteringsforespørsel, null);
        }


        /// <summary>
        /// Forespør kvittering for forsendelser med mulighet til å samtidig bekrefte på forrige kvittering for å slippe å kjøre eget kall for bekreft. 
        /// Kvitteringer blir tilgjengeliggjort etterhvert som de er klare i meldingsformidler. Det er ikke mulig å etterspørre kvittering for en 
        /// spesifikk forsendelse. 
        /// </summary>
        /// <param name="kvitteringsforespørsel"></param>
        /// <param name="forrigeKvittering"></param>
        /// <returns></returns>
        /// <remarks>
        /// <list type="table">
        /// <listheader><description>Dersom det ikke er tilgjengelige kvitteringer skal det ventes følgende tidsintervaller før en ny forespørsel gjøres</description></listheader>
        /// <item><term>normal</term><description>Minimum 10 minutter</description></item>
        /// <item><term>prioritert</term><description>Minimum 1 minutt</description></item>
        /// </list>
        /// </remarks>
        public Forretningskvittering HentKvitteringOgBekreftForrige(Kvitteringsforespørsel kvitteringsforespørsel, Forretningskvittering forrigeKvittering)
        {
            if (forrigeKvittering != null)
            {
                Bekreft(forrigeKvittering);
            }
            
            var envelopeSettings = new EnvelopeSettings(kvitteringsforespørsel, _databehandler, new GuidHandler());
            var kvitteringsenvelope = new KvitteringsEnvelope(envelopeSettings);

            ValiderKvitteringsEnvelope(kvitteringsenvelope);

            var soapContainer = new SoapContainer { Envelope = kvitteringsenvelope, Action = "\"\"" };
            var kvittering = SendSoapContainer(soapContainer);
#if DEBUG
            FileUtility.WriteXmlToFileInBasePath(kvitteringsenvelope.Xml().InnerXml, "Kvitteringsforespørsel.xml");
            FileUtility.WriteXmlToFileInBasePath(kvittering, "Kvittering.xml");
#endif

            try
            {
                var valideringAvResponsSignatur = new Responsvalidator();
                if (!valideringAvResponsSignatur.ValiderSignatur(kvittering))
                    throw new Exception("Signaturen på kvitteringen du har mottatt validerer ikke.\n");
            }
            catch (Exception e)
            {
                throw new SendException(e.Message, e);
            }
            
            return KvitteringFactory.GetForretningskvittering(kvittering);
        }

        private static void ValiderKvitteringsEnvelope(KvitteringsEnvelope kvitteringsenvelope)
        {
            try
            {
                var kvitteringForespørselEnvelopeValidering = new KvitteringForespørselEnvelopeValidering();
                var kvitteringForespørselEnvelopeValidert =
                    kvitteringForespørselEnvelopeValidering.ValiderDokumentMotXsd(kvitteringsenvelope.Xml().OuterXml);
                if (!kvitteringForespørselEnvelopeValidert)
                    throw new Exception(kvitteringForespørselEnvelopeValidering.ValideringsVarsler);
            }
            catch (Exception e)
            {
                throw new XmlValidationException("Kvitteringsforespørsel  validerer ikke mot xsd:" + e.Message);
            }
            
        }


        /// <summary>
        /// Bekreft mottak av forretningskvittering gjennom <see cref="HentKvittering(Kvitteringsforespørsel)"/>.
        /// <list type="bullet">
        /// <listheader><description><para>Dette legger opp til følgende arbeidsflyt</para></description></listheader>
        /// <item><description><para><see cref="HentKvittering(Kvitteringsforespørsel)"/></para></description></item>
        /// <item><description><para>Gjør intern prosessering av kvitteringen (lagre til database, og så videre)</para></description></item>
        /// <item><description><para>Bekreft mottak av kvittering</para></description></item>
        /// </list>
        /// </summary>
        /// <param name="forrigeKvittering"></param>
        /// <remarks>
        /// <see cref="HentKvittering(Kvitteringsforespørsel)"/> kommer ikke til å returnere en ny kvittering før mottak av den forrige er bekreftet.
        /// </remarks>
        public void Bekreft(Forretningskvittering forrigeKvittering)
        {
            var envelopeSettings = new EnvelopeSettings(forrigeKvittering, _databehandler, new GuidHandler());
            var kvitteringMottattEnvelope = new KvitteringMottattEnvelope(envelopeSettings);

            try
            {
                var kvitteringMottattEnvelopeValidering = new KvitteringMottattEnvelopeValidering();
                var kvitteringMottattEnvelopeValidert = kvitteringMottattEnvelopeValidering.ValiderDokumentMotXsd(kvitteringMottattEnvelope.Xml().OuterXml);
                if (!kvitteringMottattEnvelopeValidert)
                    throw new Exception(kvitteringMottattEnvelopeValidering.ValideringsVarsler);
            }
            catch (Exception e)
            {
                throw new XmlValidationException("Kvitteringsbekreftelse validerer ikke:" + e.Message);
            }
#if DEBUG
            FileUtility.WriteXmlToFileInBasePath(kvitteringMottattEnvelope.Xml().OuterXml, "kvitteringMottattEnvelope.xml");
#endif

            var soapContainer = new SoapContainer { Envelope = kvitteringMottattEnvelope, Action = "\"\"" };
            var response = SendSoapContainer(soapContainer);
        }


        private string SendSoapContainer(SoapContainer soapContainer)
        {
            var data = String.Empty;
            var request = (HttpWebRequest) WebRequest.Create("https://qaoffentlig.meldingsformidler.digipost.no/api/ebms");

            soapContainer.Send(request);
            try
            {
                var response = request.GetResponse();
                data = new StreamReader(response.GetResponseStream()).ReadToEnd();
            }
            catch (WebException we)
            {
                using (var response = we.Response as HttpWebResponse)
                {
                    using (Stream errorStream = response.GetResponseStream())
                    {
                        XDocument soap = XDocument.Load(errorStream);
                        data = soap.ToString();

#if DEBUG               
                        var errorFileName = String.Format("{0} - SendSoapContainerFeilet.xml", DateUtility.DateForFile());
                        FileUtility.WriteXmlToFileInBasePath(data, "FeilVedSending", errorFileName);
#endif


                    }
                }
            }
            return data;
        }


        private static void ValiderForretningsmeldingEnvelope(XmlDocument forretningsmeldingEnvelopeXml, XmlDocument manifestXml, XmlDocument signaturXml)
        {
            var envelopeValidering = new ForretningsmeldingEnvelopeValidering();
            var envelopeValidert = envelopeValidering.ValiderDokumentMotXsd(forretningsmeldingEnvelopeXml.OuterXml);
            if (!envelopeValidert)
                throw new Exception(envelopeValidering.ValideringsVarsler);

            var manifestValidering = new ManifestValidering();
            var manifestValidert = manifestValidering.ValiderDokumentMotXsd(manifestXml.OuterXml);
            if (!manifestValidert)
                throw new Exception(manifestValidering.ValideringsVarsler);

            var signaturValidering = new Signaturvalidator();
            var signaturValidert = signaturValidering.ValiderDokumentMotXsd(signaturXml.OuterXml);
            if (!signaturValidert)
                throw new Exception(signaturValidering.ValideringsVarsler);
        }
    }
}
