using System;
using System.IO;
using System.Net;
using System.Xml;
using System.Xml.Linq;
using SikkerDigitalPost.Domene;
using SikkerDigitalPost.Domene.Entiteter.Aktører;
using SikkerDigitalPost.Domene.Entiteter.Kvitteringer;
using SikkerDigitalPost.Domene.Entiteter.Post;
using SikkerDigitalPost.Klient.AsicE;
using SikkerDigitalPost.Klient.Envelope;
using SikkerDigitalPost.Klient.Utilities;
using SikkerDigitalPost.Klient.Xml;

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
        /// <param name="forsendelse">Et objekt som har all informasjon klar til å kunne sendes (mottakerinformasjon, sertifikater, vedlegg mm), enten digitalt eller fyisk.</param>
        public Transportkvittering Send(Forsendelse forsendelse)
        {
            var manifest = new Manifest(forsendelse);
            var signatur = new Signatur(forsendelse, manifest, _databehandler.Sertifikat);
            
            var guidHandler = new GuidHandler();
            var arkiv = new AsicEArkiv(forsendelse.Dokumentpakke, signatur, manifest, forsendelse.DigitalPost.Mottaker.Sertifikat, guidHandler);

            var forretingsmeldingEnvelope = new ForretningsmeldingEnvelope(new EnvelopeSettings(forsendelse, arkiv, _databehandler, guidHandler));

            var soapContainer = new SoapContainer {Envelope = forretingsmeldingEnvelope, Action = "\"\""};
            soapContainer.Vedlegg.Add(arkiv);

            var response = SendSoapContainer(soapContainer);

            FileUtility.WriteXmlToFileInBasePath(forretingsmeldingEnvelope.Xml().OuterXml, "Forretningsmelding.xml");
            FileUtility.WriteXmlToFileInBasePath(response, "ForrigeKvittering.xml");

            return KvitteringFactory.GetTransportkvittering(response);
            //if(!ValiderSignatur(response))
            //    throw new Exception("Signatur validerer ikke");

            //if(!ValiderDigests(response, envelope.Xml(), guidHandler))
            //    throw new Exception("Hash av body og/eller dokumentpakke er ikke lik for sendte og mottatte dokumenter.");
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

            var soapContainer = new SoapContainer { Envelope = kvitteringsenvelope, Action = "\"\"" };

            var kvittering = SendSoapContainer(soapContainer);

            FileUtility.WriteXmlToFileInBasePath(kvitteringsenvelope.Xml().InnerXml, "Kvitteringsforespørsel.xml");
            FileUtility.WriteXmlToFileInBasePath(kvittering, "Kvittering.xml");

            return KvitteringFactory.GetForretningskvittering(kvittering);
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
            FileUtility.WriteXmlToFileInBasePath(kvitteringMottattEnvelope.Xml().OuterXml, "kvitteringMottattEnvelope.xml");

            var soapContainer = new SoapContainer { Envelope = kvitteringMottattEnvelope, Action = "\"\"" };
            SendSoapContainer(soapContainer);
        }

        private string SendSoapContainer(SoapContainer soapContainer)
        {
            string data = String.Empty;

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
                        var errorFileName = String.Format("{0} - SendSoapContainerFeilet.xml", DateUtility.DateForFile());
                        FileUtility.WriteXmlToFileInBasePath(soap.ToString(), "FeilVedSending", errorFileName);
                        data = soap.ToString();
                    }

                }
            }
            return data;
        }

        private bool ValiderSignatur(XmlDocument response)
        {
            XmlNode responseRot = response.DocumentElement;
            var responseMgr = new XmlNamespaceManager(response.NameTable);
            responseMgr.AddNamespace("env", Navnerom.env);
            responseMgr.AddNamespace("ds", Navnerom.ds);

            try
            {
                var signatureNode = (XmlElement)responseRot.SelectSingleNode("//ds:Signature", responseMgr);
                var signed = new SignedXmlWithAgnosticId(response);
                signed.LoadXml(signatureNode);
                return signed.CheckSignature();
            }
            catch (Exception e)
            {
                throw new Exception("Feil under validering av signatur.", e);
            }
        }

        private bool ValiderDigests(XmlDocument response, XmlDocument envelope, GuidHandler guidHandler)
        {
            XmlNode responseRot = response.DocumentElement;
            XmlNamespaceManager responseMgr = new XmlNamespaceManager(response.NameTable);
            responseMgr.AddNamespace("env", Navnerom.env);
            responseMgr.AddNamespace("ns5", Navnerom.Ns5);

            try
            {
                var responseBodyDigest = responseRot.SelectSingleNode("//ns5:Reference[@URI = '#" + guidHandler.BodyId + "']", responseMgr).InnerText;
                var responseAsicDigest = responseRot.SelectSingleNode("//ns5:Reference[@URI = 'cid:" + guidHandler.DokumentpakkeId + "']", responseMgr).InnerText;

                var envelopeRot = envelope.DocumentElement;
                var envelopeMgr = new XmlNamespaceManager(envelope.NameTable);
                envelopeMgr.AddNamespace("env", Navnerom.env);
                envelopeMgr.AddNamespace("wsse", Navnerom.wsse);
                envelopeMgr.AddNamespace(String.Empty, Navnerom.Ns5);
                
                var envelopeBodyDigest = envelopeRot.SelectSingleNode("//*[namespace-uri()='" + Navnerom.ds + "' and local-name()='Reference'][@URI = '#" + guidHandler.BodyId + "']", envelopeMgr).InnerText;
                var envelopeAsicDigest = envelopeRot.SelectSingleNode("//*[namespace-uri()='" + Navnerom.ds + "' and local-name()='Reference'][@URI = 'cid:" + guidHandler.DokumentpakkeId + "']", envelopeMgr).InnerText;

                return responseBodyDigest.Equals(envelopeBodyDigest) && responseAsicDigest.Equals(envelopeAsicDigest);
            }
            catch (Exception e)
            {
                throw new Exception("En feil", e);
            }
        }
    }
}
