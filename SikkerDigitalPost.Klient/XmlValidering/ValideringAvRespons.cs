using System;
using System.Xml;
using SikkerDigitalPost.Domene.Exceptions;
using SikkerDigitalPost.Klient.Security;

namespace SikkerDigitalPost.Klient.XmlValidering
{
    internal class ValideringAvRespons
    {
        public void ValiderRespons(string response, XmlDocument sendtMelding, GuidHandler guidHandler)
        {
            if (!ValiderSignatur(response))
                throw new SendException("Signatur av respons fra Meldingsformidler var ikke gyldig.");

            if (!ValiderDigests(response, sendtMelding, guidHandler))
                throw new SendException("Hash av body og/eller dokumentpakke er ikke lik for sendte og mottatte dokumenter.");
        }

        public bool ValiderSignatur(string response)
        {
            XmlDocument document = new XmlDocument();
            document.LoadXml(response);
            XmlNode responseRot = document.DocumentElement;
            var responseMgr = new XmlNamespaceManager(document.NameTable);
            responseMgr.AddNamespace("env", Navnerom.env);
            responseMgr.AddNamespace("ds", Navnerom.ds);

            var signatureNode = (XmlElement)responseRot.SelectSingleNode("//ds:Signature", responseMgr);
            var signed = new SignedXmlWithAgnosticId(document);
            signed.LoadXml(signatureNode);
            return signed.CheckSignature();
        }

        private bool ValiderDigests(string response, XmlDocument envelope, GuidHandler guidHandler)
        {
            XmlDocument document = new XmlDocument();
            document.LoadXml(response);

            XmlNode responseRot = document.DocumentElement;
            XmlNamespaceManager responseMgr = new XmlNamespaceManager(document.NameTable);
            responseMgr.AddNamespace("env", Navnerom.env);
            responseMgr.AddNamespace("ns5", Navnerom.Ns5);

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
    }
}
