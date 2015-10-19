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
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Xml;
using Difi.SikkerDigitalPost.Klient.Security;
using Difi.SikkerDigitalPost.Klient.Utilities;

namespace Difi.SikkerDigitalPost.Klient.XmlValidering
{
    /// <summary>
    /// Inneholder funksjonalitet for å validere motatte svar fra meldingsformidleren.
    /// </summary>
    internal class Responsvalidator
    {
        internal Miljø Miljø { get; set; }
        private XmlDocument responseDocument;
        private XmlNamespaceManager nsMgr;
        private XmlDocument _sendtMelding;

        /// <summary>
        /// Oppretter en ny instanse av responsvalidatoren.
        /// </summary>
        /// <param name="respons">Et soap dokument i tekstform. Dette er svaret som har blitt motatt fra meldingsformidleren ved en forsendelse av brev eller kvittering.</param>
        /// <param name="sendtMelding">Soap meldingen som har blitt sendt til meldingsformidleren.</param>
        /// <param name="miljø"></param>
        public Responsvalidator(string respons, XmlDocument sendtMelding, Miljø miljø)
        {
            Miljø = miljø;
            responseDocument = new XmlDocument();
            responseDocument.LoadXml(respons);

            nsMgr = new XmlNamespaceManager(responseDocument.NameTable);
            nsMgr.AddNamespace("env", NavneromUtility.SoapEnvelopeEnv12);
            nsMgr.AddNamespace("wsse", NavneromUtility.WssecuritySecext10);
            nsMgr.AddNamespace("ds", NavneromUtility.XmlDsig);
            nsMgr.AddNamespace("eb", NavneromUtility.EbXmlCore);
            nsMgr.AddNamespace("wsu", NavneromUtility.WssecurityUtility10);
            nsMgr.AddNamespace("ebbp", NavneromUtility.EbppSignals);
            nsMgr.AddNamespace("sbd", NavneromUtility.StandardBusinessDocumentHeader);
            nsMgr.AddNamespace("difi", NavneromUtility.DifiSdpSchema10);

            _sendtMelding = sendtMelding;
        }


        /// <summary>
        /// Validerer signaturen i soap headeren for motatt dokument.
        /// </summary>
        public void ValiderHeaderSignatur()
        {
            XmlNode responseRot = responseDocument.DocumentElement;
            var signatureNode = (XmlElement)responseRot.SelectSingleNode("/env:Envelope/env:Header/wsse:Security/ds:Signature", nsMgr);
            var signed = new SignedXmlWithAgnosticId(responseDocument);

            ValiderInnhold(signatureNode, signed);

            ValiderSignaturOgSertifikat(signed, signatureNode, "/env:Envelope/env:Header/wsse:Security/wsse:BinarySecurityToken");
        }

        public void ValiderKvitteringSignatur()
        {
            // Signaturer i //difi elementer har kontekst av standard business document. Kjører derfor valideringen på et subset av originaldokumentet.

            var standardBusinessDocument =
                responseDocument.SelectSingleNode("/env:Envelope/env:Body/sbd:StandardBusinessDocument", nsMgr);

            if (standardBusinessDocument == null)
                return;

            XmlDocument sbd = new XmlDocument();
            sbd.LoadXml(standardBusinessDocument.OuterXml);

            var signed = new SignedXmlWithAgnosticId(sbd);
            var signatureNode = (XmlElement)sbd.SelectSingleNode("//ds:Signature", nsMgr);

            ValiderSignaturOgSertifikat(signed, signatureNode, "./ds:KeyInfo/ds:X509Data/ds:X509Certificate");
        }

        private void ValiderSignaturOgSertifikat(SignedXmlWithAgnosticId signed, XmlElement signatureNode, string path)
        {
            var certificate = new X509Certificate2(Convert.FromBase64String(signatureNode.SelectSingleNode(path, nsMgr).InnerText));
            ErKvalifisertMellomliggendeSertifikat(certificate);

            signed.LoadXml(signatureNode);

            AsymmetricAlgorithm key = null;
            if (!signed.CheckSignatureReturningKey(out key))
                throw new Exception("Signaturen i motatt svar er ikke gyldig.");

            if (key.ToXmlString(false) != certificate.PublicKey.Key.ToXmlString(false))
                throw new Exception(string.Format("Sertifikatet som er benyttet for å validere signaturen er ikke det samme som er spesifisert i {0} elementet.", path));
        }

        private void ErKvalifisertMellomliggendeSertifikat(X509Certificate2 certificate)
        {
            var chain = new X509Chain(false);
            chain.Build(certificate);

            foreach (var item in chain.ChainElements)
            {
                if (item.Certificate.RawData.SequenceEqual(Sertifikater.BPClass3CA3))
                    return;
                if (item.Certificate.RawData.SequenceEqual(Sertifikater.Buypass_Class_3_Test4_CA_3))
                    return;
                if (item.Certificate.RawData.SequenceEqual(Sertifikater.commfidesenterprise_sha256))
                    return;
                if (item.Certificate.RawData.SequenceEqual(Sertifikater.cpn_enterprise_sha256_class_3))
                    return;
            }

            throw new Exception("Sertifikatet som er angitt i signaturen er ikke signert av en gyldig mellomliggende utsteder.");
        }

        /// <summary>
        /// Sjekker at motatt soap dokument har samme digest verdier for body og dokumentpakke i avsendt brev vha motatt NonRepudiationInformation element. 
        /// </summary>
        /// <param name="guidHandler">Samme guid handler som ble benyttet for å generere det avsendte brevet.</param>
        public void ValiderDigest(GuidUtility guidHandler)
        {
            string sourceDigestPath = "/env:Envelope/env:Header/wsse:Security/ds:Signature/ds:SignedInfo/ds:Reference[@URI='{0}']/ds:DigestValue";
            string targetDigestPath = "/env:Envelope/env:Header/eb:Messaging/eb:SignalMessage/eb:Receipt/ebbp:NonRepudiationInformation/ebbp:MessagePartNRInformation/ds:Reference[@URI='{0}']/ds:DigestValue";

            foreach (var uri in new string[] { "#" + guidHandler.BodyId, "cid:" + guidHandler.DokumentpakkeId })
            {
                string sourceDigest = _sendtMelding.SelectSingleNode(string.Format(sourceDigestPath, uri), nsMgr).InnerText;
                string targetDigest = responseDocument.SelectSingleNode(string.Format(targetDigestPath, uri), nsMgr).InnerText;

                if (sourceDigest != targetDigest)
                    throw new Exception(string.Format("Digest verdien av uri {0} for sendt melding ({1}) matcher ikke motatt digest ({2}).", uri, sourceDigest, targetDigest));
            }
        }

        /// <summary>
        /// Sjekker at soap envelopen inneholder timestamp, body og messaging element med korrekt id og referanser i security signaturen.
        /// </summary>
        private void ValiderInnhold(XmlElement signature, SignedXmlWithAgnosticId signedXml)
        {
            string[] requiredSignatureElements = { "/env:Envelope/env:Header/wsse:Security/wsu:Timestamp", "/env:Envelope/env:Body", "/env:Envelope/env:Header/eb:Messaging" };

            foreach (var elementXPath in requiredSignatureElements)
            {
                // Sørg for at svar inneholde påkrevede noder.
                var nodes = responseDocument.SelectNodes(elementXPath, nsMgr);
                if (nodes == null || nodes.Count == 0)
                    throw new Exception(string.Format("Kan ikke finne påkrevet element '{0}' i svar fra meldingsformidler.", elementXPath));
                if (nodes.Count > 1)
                    throw new Exception(string.Format("Påkrevet element '{0}' kan kun forekomme én gang i svar fra meldingsformidler. Ble funnet {1} ganger.", elementXPath, nodes.Count));

                // Sørg for at det finnes en refereanse til node i signatur element
                var elementId = nodes[0].Attributes["wsu:Id"].Value;

                var references = signature.SelectNodes(string.Format("./ds:SignedInfo/ds:Reference[@URI='#{0}']", elementId), nsMgr);
                if (references == null || references.Count == 0)
                    throw new Exception(string.Format("Kan ikke finne påkrevet refereanse til element '{0}' i signatur fra meldingsformidler.", elementXPath));
                if (references.Count > 1)
                    throw new Exception(string.Format("Påkrevet refereanse til element '{0}' kan kun forekomme én gang i signatur fra meldingsformidler. Ble funnet {1} ganger.", elementXPath, references.Count));

                // Sørg for at Id node matcher
                var targetNode = signedXml.GetIdElement(responseDocument, elementId);
                if (targetNode != nodes[0])
                    throw new Exception(string.Format("Signaturreferansen med id '{0}' må refererer til node med sti '{1}'", elementId, elementXPath));
            }
        }
    }
}
