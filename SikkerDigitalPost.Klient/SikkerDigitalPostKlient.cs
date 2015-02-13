/** 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *         http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

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
using SikkerDigitalPost.Klient.Envelope.Forretningsmelding;
using SikkerDigitalPost.Klient.Envelope.Kvitteringsbekreftelse;
using SikkerDigitalPost.Klient.Envelope.Kvitteringsforespørsel;
using SikkerDigitalPost.Klient.Utilities;
using SikkerDigitalPost.Klient.XmlValidering;
using System.Diagnostics;
using SikkerDigitalPost.Domene.Entiteter.Kvitteringer.Forretning;
using SikkerDigitalPost.Domene.Entiteter.Kvitteringer.Transport;

namespace SikkerDigitalPost.Klient
{
    public class SikkerDigitalPostKlient
    {
        private readonly Databehandler _databehandler;
        private readonly Klientkonfigurasjon _klientkonfigurasjon;

        /// <param name="databehandler">
        /// Teknisk avsender er den parten som har ansvarlig for den tekniske utførelsen av sendingen.
        /// Teknisk avsender er den aktøren som står for utførelsen av den tekniske sendingen. 
        /// Hvis sendingen utføres av en databehandler vil dette være databehandleren. 
        /// Hvis sendingen utføres av behandlingsansvarlige selv er dette den behandlingsansvarlige.
        /// </param>
        /// <remarks>
        /// Se <a href="http://begrep.difi.no/SikkerDigitalPost/forretningslag/Aktorer">oversikt over aktører</a>
        /// </remarks>
        public SikkerDigitalPostKlient(Databehandler databehandler)
            : this(databehandler, new Klientkonfigurasjon())
        {

        }

        /// <param name="databehandler">
        /// Teknisk avsender er den parten som har ansvarlig for den tekniske utførelsen av sendingen.
        /// Teknisk avsender er den aktøren som står for utførelsen av den tekniske sendingen. 
        /// Hvis sendingen utføres av en databehandler vil dette være databehandleren. 
        /// Hvis sendingen utføres av behandlingsansvarlige selv er dette den behandlingsansvarlige.
        /// </param>
        /// <param name="klientkonfigurasjon">Klientkonfigurasjon for klienten. Brukes for å sette parametere
        /// som proxy, timeout og URI til meldingsformidler. For å bruke standardkonfigurasjon, lag
        /// SikkerDigitalPostKlient uten Klientkonfigurasjon som parameter.</param>
        /// <remarks>
        /// Se <a href="http://begrep.difi.no/SikkerDigitalPost/forretningslag/Aktorer">oversikt over aktører</a>
        /// </remarks>
        public SikkerDigitalPostKlient(Databehandler databehandler, Klientkonfigurasjon klientkonfigurasjon)
        {
            _databehandler = databehandler;
            _klientkonfigurasjon = klientkonfigurasjon;
            Logging.Initialize(klientkonfigurasjon);
            FileUtility.BasePath = klientkonfigurasjon.StandardLoggSti;
        }
        
        /// <summary>
        /// Sender en forsendelse til meldingsformidler. Dersom noe feilet i sendingen til meldingsformidler, vil det kastes en exception.
        /// </summary>
        /// <param name="forsendelse">Et objekt som har all informasjon klar til å kunne sendes (mottakerinformasjon, sertifikater, vedlegg mm), enten digitalt eller fysisk.</param>
        public Transportkvittering Send(Forsendelse forsendelse)
        {
            Logging.Log(TraceEventType.Information, forsendelse.KonversasjonsId, "Sender ny forsendelse til meldingsformidler.");

            var guidHandler = new GuidHandler();
            var arkiv = new AsicEArkiv(forsendelse, guidHandler, _databehandler.Sertifikat);
            var forretningsmeldingEnvelope = new ForretningsmeldingEnvelope(new EnvelopeSettings(forsendelse, arkiv, _databehandler, guidHandler, _klientkonfigurasjon));

            try
            {
                ValiderForretningsmeldingEnvelope(forretningsmeldingEnvelope.Xml(), arkiv.Manifest.Xml(), arkiv.Signatur.Xml());
            }
            catch (Exception e)
            {
                throw new XmlValidationException("Envelope xml validerer ikke mot xsd:", e);
            }

            var soapContainer = new SoapContainer { Envelope = forretningsmeldingEnvelope, Action = "\"\"" };
            soapContainer.Vedlegg.Add(arkiv);
            var meldingsformidlerRespons = SendSoapContainer(soapContainer);

            Logg(TraceEventType.Verbose, forsendelse.KonversasjonsId, soapContainer.SisteBytesSendt, true,false, "Sendt - SOAPContainer.txt");
            Logg(TraceEventType.Verbose, forsendelse.KonversasjonsId, arkiv.Signatur.Xml().OuterXml, true, true, "Sendt - Signatur.xml");
            Logg(TraceEventType.Verbose, forsendelse.KonversasjonsId, arkiv.Manifest.Xml().OuterXml, true, true, "Sendt - Manifest.xml");
            Logg(TraceEventType.Verbose, forsendelse.KonversasjonsId, forretningsmeldingEnvelope.Xml().OuterXml, true, true, "Sendt - Envelope.xml");
            Logg(TraceEventType.Verbose, forsendelse.KonversasjonsId, meldingsformidlerRespons, true, true, "Mottatt - Meldingsformidlerespons.txt");

            Logging.Log(TraceEventType.Information, forsendelse.KonversasjonsId, "Kvittering for forsendelse" + Environment.NewLine + meldingsformidlerRespons);
            
            return ValiderTransportkvittering(meldingsformidlerRespons, forretningsmeldingEnvelope.Xml(), guidHandler);
        }

        private static Transportkvittering ValiderTransportkvittering(string meldingsformidlerRespons,
            XmlDocument forretningsmeldingEnvelope, GuidHandler guidHandler)
        {
            try
            {
                var valideringAvRespons = new Responsvalidator(meldingsformidlerRespons, forretningsmeldingEnvelope);
                valideringAvRespons.ValiderHeaderSignatur();
                valideringAvRespons.ValiderDigest(guidHandler);
            }
            catch (Exception e)
            {
                var transportFeiletKvittering = KvitteringFactory.GetTransportkvittering(meldingsformidlerRespons);
                if (transportFeiletKvittering is TransportOkKvittering)
                {
                    throw new SdpSecurityException("Validering av signatur og digest på respons feilet.", e);
                }
                return transportFeiletKvittering;
            }

            return KvitteringFactory.GetTransportkvittering(meldingsformidlerRespons);
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
        public Kvittering HentKvittering(Kvitteringsforespørsel kvitteringsforespørsel)
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
        public Kvittering HentKvitteringOgBekreftForrige(Kvitteringsforespørsel kvitteringsforespørsel, Forretningskvittering forrigeKvittering)
        {
            if (forrigeKvittering != null)
            {
                Bekreft(forrigeKvittering);
            }

            Logging.Log(TraceEventType.Information, "Henter kvittering for " + kvitteringsforespørsel.Mpc);

            var guidHandler = new GuidHandler();
            var envelopeSettings = new EnvelopeSettings(kvitteringsforespørsel, _databehandler, guidHandler);
            var kvitteringsenvelope = new KvitteringsforespørselEnvelope(envelopeSettings);

            Logging.Log(TraceEventType.Verbose, "Envelope for kvitteringsforespørsel" + Environment.NewLine + kvitteringsenvelope.Xml().OuterXml);

            ValiderKvitteringsEnvelope(kvitteringsenvelope);

            var soapContainer = new SoapContainer { Envelope = kvitteringsenvelope, Action = "\"\"" };
            var kvittering = SendSoapContainer(soapContainer);

            Logg(TraceEventType.Verbose, Guid.Empty , kvitteringsenvelope.Xml().OuterXml, true, true, "Sendt - Kvitteringsenvelope.xml");

            try
            {
                var valideringAvResponsSignatur = new Responsvalidator(kvittering, kvitteringsenvelope.Xml());
                valideringAvResponsSignatur.ValiderHeaderSignatur();
                valideringAvResponsSignatur.ValiderKvitteringSignatur();
            }
            catch (Exception e)
            {
                return ValiderTransportkvittering(kvittering,kvitteringsenvelope.Xml(),guidHandler);
            }

            return KvitteringFactory.GetForretningskvittering(kvittering);
        }

        private static void ValiderKvitteringsEnvelope(KvitteringsforespørselEnvelope kvitteringsenvelope)
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
                throw new XmlValidationException("Kvitteringsforespørsel validerer ikke mot xsd:" + e.Message);
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
            Logging.Log(TraceEventType.Information, forrigeKvittering.KonversasjonsId, "Bekrefter forrige kvittering.");

            var envelopeSettings = new EnvelopeSettings(forrigeKvittering, _databehandler, new GuidHandler());
            var kvitteringMottattEnvelope = new KvitteringsbekreftelseEnvelope(envelopeSettings);

            string filnavn = forrigeKvittering.GetType().Name + ".xml";
            Logg(TraceEventType.Verbose, forrigeKvittering.KonversasjonsId, forrigeKvittering, true, true, filnavn);
            
            try
            {
                var kvitteringMottattEnvelopeValidering = new KvitteringMottattEnvelopeValidator();
                var kvitteringMottattEnvelopeValidert = kvitteringMottattEnvelopeValidering.ValiderDokumentMotXsd(kvitteringMottattEnvelope.Xml().OuterXml);
                if (!kvitteringMottattEnvelopeValidert)
                    throw new Exception(kvitteringMottattEnvelopeValidering.ValideringsVarsler);
            }
            catch (Exception e)
            {
                throw new XmlValidationException("Kvitteringsbekreftelse validerer ikke:" + e.Message);
            }


            var soapContainer = new SoapContainer { Envelope = kvitteringMottattEnvelope, Action = "\"\"" };
            SendSoapContainer(soapContainer);
        }

        private string SendSoapContainer(SoapContainer soapContainer)
        {
            var data = String.Empty;
            var request = (HttpWebRequest)WebRequest.Create(_klientkonfigurasjon.MeldingsformidlerUrl);
            if (_klientkonfigurasjon.BrukProxy)
                request.Proxy = new WebProxy(new UriBuilder(_klientkonfigurasjon.ProxyScheme, _klientkonfigurasjon.ProxyHost, _klientkonfigurasjon.ProxyPort).Uri);

            request.Timeout = _klientkonfigurasjon.TimeoutIMillisekunder;

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
               
                        Logg(TraceEventType.Critical, Guid.Empty, data, true, true, "Sendt - SoapException.xml");
                    }
                }
            }
            return data;
        }

        private static void ValiderForretningsmeldingEnvelope(XmlDocument forretningsmeldingEnvelopeXml, XmlDocument manifestXml, XmlDocument signaturXml)
        {
            var envelopeValidering = new ForretningsmeldingEnvelopeValidator();
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

        private void Logg(TraceEventType viktighet, Guid konversasjonsId, string melding, bool datoPrefiks, bool isXml, string filnavn, params string[] filsti)
        {
            string[] fullFilsti = new string[filsti.Length + 1];
            for (int i = 0; i < filsti.Length; i ++ )
            {
                var sti = filsti[i];
                fullFilsti[i] = sti;
            }

            filnavn = datoPrefiks ? String.Format("{0} - {1}", DateUtility.DateForFile(), filnavn) : filnavn;
            fullFilsti[filsti.Length] = filnavn;

            if (_klientkonfigurasjon.DebugLoggTilFil && filnavn!= null)
            {
                if (isXml)
                    FileUtility.WriteXmlToBasePath(melding, filnavn);
                else
                    FileUtility.WriteToBasePath(melding, filnavn);
            }

            Logging.Log(viktighet, konversasjonsId, melding);
        }

        private void Logg(TraceEventType viktighet, Guid konversasjonsId, byte[] melding, bool datoPrefiks, bool isXml, string filnavn, params string[] filsti)
        {
            string data = System.Text.Encoding.UTF8.GetString(melding);
            Logg(viktighet, konversasjonsId, data,datoPrefiks,isXml,filnavn,filsti);
        }

        private void Logg(TraceEventType viktighet, Guid konversasjonsId, Forretningskvittering kvittering, bool datoPrefiks, bool isXml, params string[] filsti)
        {
            var fileSuffix = isXml ? ".xml" : ".txt";
            Logg(viktighet,konversasjonsId,kvittering.Rådata, datoPrefiks, isXml,"Mottatt - " + kvittering.GetType().Name + fileSuffix);
        }
    }
}
