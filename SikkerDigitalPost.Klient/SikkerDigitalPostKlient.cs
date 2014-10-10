using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.Xml;
using System.Xml;
using System.Xml.Linq;
using SikkerDigitalPost.Domene.Entiteter.Aktører;
using SikkerDigitalPost.Domene.Entiteter.AsicE.Manifest;
using SikkerDigitalPost.Domene.Entiteter.AsicE.Signatur;
using SikkerDigitalPost.Domene.Entiteter.Kvitteringer;
using SikkerDigitalPost.Domene.Entiteter.Post;
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
        public void Send(Forsendelse forsendelse)
        {
            var mottaker = forsendelse.DigitalPost.Mottaker;
            var manifest = new Manifest(mottaker, forsendelse.Behandlingsansvarlig, forsendelse);
            var signatur = new Signatur(_databehandler.Sertifikat);

            var manifestbygger = new ManifestBygger(manifest);
            manifestbygger.Bygg();
            var signaturbygger = new SignaturBygger(signatur, forsendelse, manifest);
            signaturbygger.Bygg();

            var guidHandler = new GuidHandler();
            var arkiv = new AsicEArkiv(forsendelse.Dokumentpakke, signatur, manifest, forsendelse.DigitalPost.Mottaker.Sertifikat, guidHandler);

            var envelope = new ForretingsmeldingEnvelope(new EnvelopeSettings(forsendelse, arkiv, _databehandler, guidHandler));

            //envelope.SkrivTilFil(Environment.MachineName.Contains("LEK")
            //    ? @"Z:\Development\Digipost\Envelope.xml"
            //    : @"C:\Prosjekt\DigiPost\Temp\Envelope.xml");

            var soapContainer = new SoapContainer {Envelope = envelope, Action = "\"\""};
            soapContainer.Vedlegg.Add(arkiv);

            var responseXml = SendSoapContainer(soapContainer);

            //Valider signatur
            XmlNode rot = responseXml.DocumentElement;
            XmlNamespaceManager responseMgr = new XmlNamespaceManager(responseXml.NameTable);
            responseMgr.AddNamespace("ds", Navnerom.ds);
            responseMgr.AddNamespace("ns5", Navnerom.Ns5);
            responseMgr.AddNamespace("ns7", Navnerom.Ns7);
            
            try
            {
                XmlNode signatureNode = rot.SelectSingleNode("ds:Signature", responseMgr);
                //valider
            }
            catch (Exception e)
            {
                throw new Exception("", e);
            }

            //Valider body og attachment
            try
            {
                //Hent body og attachment digests
                var bodyReference = rot.SelectSingleNode("//ns5:Reference[@URI = #" + guidHandler.BodyId + "]", responseMgr);
                var asicReference = rot.SelectSingleNode("//ns5:Reference[@URI = cid:" + guidHandler.DokumentpakkeId + "]", responseMgr);
                var bodyDigest = bodyReference.SelectSingleNode("ns5:DigestValue", responseMgr).InnerText;
                var asicDigest = asicReference.SelectSingleNode("ns5:DigestValue", responseMgr).InnerText;

                //Valider mot envelope
                var envelopeXml = envelope.Xml();
                var envelopeRoot = envelopeXml.DocumentElement;
                XmlNamespaceManager envelopeMgr = new XmlNamespaceManager(envelopeXml.NameTable);
                envelopeMgr.AddNamespace("", "http://www.w3.org/2000/09/xmldsig#");
                var envBodyReference = rot.SelectSingleNode("Signature/SignedInfo/Reference[@URI = " + guidHandler.BodyId + "]", envelopeMgr);
                var envAsicReference = rot.SelectSingleNode("Signature/SignedInfo/Reference[@URI = " + guidHandler.DokumentpakkeId + "]", envelopeMgr);
                var envBodyDigest = envBodyReference.SelectSingleNode("DigestValue", envelopeMgr).InnerText;
                var envAsicDigest = envAsicReference.SelectSingleNode("DigestValue", envelopeMgr).InnerText;
                if (!bodyDigest.Equals(envBodyDigest) || !envAsicDigest.Equals(asicDigest))
                {
                    throw new Exception("Digests are not equal!");
                }
            }
            catch (Exception e)
            {
                throw new Exception("Fant ikke...", e);
            }
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
            var envelopeSettings = new EnvelopeSettings(kvitteringsforespørsel, _databehandler, new GuidHandler());
            var envelope = new KvitteringsEnvelope(envelopeSettings);
            var soapContainer = new SoapContainer {Envelope = envelope, Action = "\"\""};

            SendSoapContainer(soapContainer);
            return null;
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
            return null;
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
            
        }

        private XmlDocument SendSoapContainer(SoapContainer soapContainer)
        {
            var request = (HttpWebRequest)WebRequest.Create("https://qaoffentlig.meldingsformidler.digipost.no/api/ebms");
            soapContainer.Send(request);
            try
            {
                using (Stream responseStream = request.GetResponse().GetResponseStream())
                {
                    using (XmlReader xmlReader = XmlReader.Create(responseStream))
                    {
                        var xmlDocument = new XmlDocument();
                        xmlDocument.Load(xmlReader);
                        return xmlDocument;
                    }
                }
            }
            catch (WebException we)
            {
                using (var response = we.Response as HttpWebResponse)
                {
                    using (Stream errorStream = response.GetResponseStream())
                    {
                        var soap = XDocument.Load(errorStream);
                        throw new Exception("En feil");
                    }

                }
            }
        }
    }
}
