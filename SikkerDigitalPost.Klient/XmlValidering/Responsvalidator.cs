using System;
using System.Xml;
using SikkerDigitalPost.Domene.Exceptions;
using SikkerDigitalPost.Klient.Security;

namespace SikkerDigitalPost.Klient.XmlValidering
{
    internal class Responsvalidator
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
            responseMgr.AddNamespace("wsse", Navnerom.wsse);
            responseMgr.AddNamespace("ds", Navnerom.ds);
            responseMgr.AddNamespace("eb", Navnerom.eb);

            var signatureNode = (XmlElement)responseRot.SelectSingleNode("/env:Envelope/env:Header/wsse:Security/ds:Signature", responseMgr);
            var signed = new SignedXmlWithAgnosticId(document);

            ValiderInnhold(document, signatureNode, signed, responseMgr);

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

        private void ValiderInnhold(XmlDocument response, XmlElement signature, SignedXmlWithAgnosticId signedXml, XmlNamespaceManager nsmgr)
        {
            string[] requiredSignatureElements = { "/env:Envelope/env:Header/wsse:Security/wsu:Timestamp", "/env:Envelope/env:Body", "/env:Envelope/env:Header/eb:Messaging" };

            foreach (var elementXPath in requiredSignatureElements)
            {
                // Sørg for at svar inneholde påkrevede noder.
                var nodes = response.SelectNodes(elementXPath, nsmgr);
                if (nodes == null || nodes.Count == 0)
                    throw new Exception(string.Format("Kan ikke finne påkrevet element '{0}' i svar fra meldingsformidler.", elementXPath));
                if (nodes.Count > 1)
                    throw new Exception(string.Format("Påkrevet element '{0}' kan kun forekomme én gang i svar fra meldingsformidler. Ble funnet {1} ganger.", elementXPath, nodes.Count));

                // Sørg for at det finnes en refereanse til node i signatur element
                var elementId = nodes[0].Attributes["Id"].Value;

                var references = signature.SelectNodes(string.Format("./ds:SignedInfo/ds:Reference[URI='@{0}']", elementId), nsmgr);
                if (references == null || references.Count == 0)
                    throw new Exception(string.Format("Kan ikke finne påkrevet refereanse til element '{0}' i signatur fra meldingsformidler.", elementXPath));
                if (references.Count > 1)
                    throw new Exception(string.Format("Påkrevet refereanse til element '{0}' kan kun forekomme én gang i signatur fra meldingsformidler. Ble funnet {1} ganger.", elementXPath, references.Count));

                // Sørg for at Id node matcher
                var targetNode = signedXml.GetIdElement(response, elementId);
                if (targetNode != nodes[0])
                    throw new Exception(string.Format("Signaturreferansen med id '{0}' må refererer til node med sti '{1}'", elementId, elementXPath));
            }
        }
    }
}
