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
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Xml;
using Difi.SikkerDigitalPost.Klient.Domene.Exceptions;
using Difi.SikkerDigitalPost.Klient.Security;
using Difi.SikkerDigitalPost.Klient.Utilities;

namespace Difi.SikkerDigitalPost.Klient.XmlValidering
{
    /// <summary>
    /// Inneholder funksjonalitet for å validere motatte svar fra meldingsformidleren.
    /// </summary>
    internal class Responsvalidator
    {
        public XmlDocument Respons { get; internal set; }

        public XmlDocument SendtMelding { get; internal set; }

        public Sertifikatvalidator Sertifikatvalidator { get; internal set; }
        
        private readonly XmlNamespaceManager nsMgr;
        private SignedXmlWithAgnosticId _signedXmlWithAgnosticId;
        private XmlElement _signaturnode;
        private X509Certificate2 _sertifikat;

        /// <summary>
        /// Oppretter en ny instanse av responsvalidatoren.
        /// </summary>
        /// <param name="respons">Et soap dokument i tekstform. Dette er svaret som har blitt motatt fra meldingsformidleren ved en forsendelse av brev eller kvittering.</param>
        /// <param name="sendtMelding">Soap meldingen som har blitt sendt til meldingsformidleren.</param>
        /// <param name="kjørendeMiljø"></param>
        public Responsvalidator(XmlDocument respons, XmlDocument sendtMelding, Sertifikatvalidator sertifikatvalidator)
        {
            Respons = respons;
            SendtMelding = sendtMelding;
            Sertifikatvalidator = sertifikatvalidator;

            
            nsMgr = new XmlNamespaceManager(Respons.NameTable);
            nsMgr.AddNamespace("env", NavneromUtility.SoapEnvelopeEnv12);
            nsMgr.AddNamespace("wsse", NavneromUtility.WssecuritySecext10);
            nsMgr.AddNamespace("ds", NavneromUtility.XmlDsig);
            nsMgr.AddNamespace("eb", NavneromUtility.EbXmlCore);
            nsMgr.AddNamespace("wsu", NavneromUtility.WssecurityUtility10);
            nsMgr.AddNamespace("ebbp", NavneromUtility.EbppSignals);
            nsMgr.AddNamespace("sbd", NavneromUtility.StandardBusinessDocumentHeader);
            nsMgr.AddNamespace("difi", NavneromUtility.DifiSdpSchema10);
        }

        public void ValiderMeldingskvittering()
        {
            ValiderHeaderSignatur();
            ValiderKvitteringSignatur();
        }

        public void ValiderTransportkvittering(GuidUtility guidUtility)
        {
            ValiderHeaderSignatur();
            ValiderDigest(guidUtility);
        }

        public void ValiderTomkøkvittering()
        {
            ValiderHeaderSignatur();
        }

        private void ValiderHeaderSignatur()
        {
            XmlNode responsRot = Respons.DocumentElement;
            _signaturnode = (XmlElement)responsRot.SelectSingleNode("/env:Envelope/env:Header/wsse:Security/ds:Signature", nsMgr);
            _signedXmlWithAgnosticId = new SignedXmlWithAgnosticId(Respons);

            ValiderSignaturelementer();
            ValiderSignaturOgSertifikat("/env:Envelope/env:Header/wsse:Security/wsse:BinarySecurityToken");
        }

        private void ValiderKvitteringSignatur()
        {
            var standardBusinessDocumentNode =
                Respons.SelectSingleNode("/env:Envelope/env:Body/sbd:StandardBusinessDocument", nsMgr);

            if (standardBusinessDocumentNode != null)
            {
                var standardBusinessDocument = XmlNodeToXmlDocument(standardBusinessDocumentNode);

                _signedXmlWithAgnosticId = new SignedXmlWithAgnosticId(standardBusinessDocument);
                _signaturnode = (XmlElement) standardBusinessDocument.SelectSingleNode("//ds:Signature", nsMgr);

                ValiderSignaturOgSertifikat("./ds:KeyInfo/ds:X509Data/ds:X509Certificate");
            }
            else
            {
                throw new SdpSecurityException("Fant ikke StandardBusinessDocument-node. Prøvde du å validere en transportkvittering?");
            }
        }

        private static XmlDocument XmlNodeToXmlDocument(XmlNode standardBusinessDocument)
        {
            XmlDocument sbd = new XmlDocument();
            sbd.LoadXml(standardBusinessDocument.OuterXml);
            return sbd;
        }

        private void ValiderSignaturOgSertifikat(string path)
        {
            _sertifikat = new X509Certificate2(Convert.FromBase64String(_signaturnode.SelectSingleNode(path, nsMgr).InnerText));
            ValiderResponssertifikat();

            _signedXmlWithAgnosticId.LoadXml(_signaturnode);

            AsymmetricAlgorithm asymmetricAlgorithm;
            if (!_signedXmlWithAgnosticId.CheckSignatureReturningKey(out asymmetricAlgorithm))
                throw new SdpSecurityException("Signaturen i motatt svar er ikke gyldig.");

            if (asymmetricAlgorithm.ToXmlString(false) != _sertifikat.PublicKey.Key.ToXmlString(false))
                throw new SdpSecurityException(string.Format("Sertifikatet som er benyttet for å validere signaturen er ikke det samme som er spesifisert i {0} elementet.", path));
        }

        private void ValiderResponssertifikat()
        {
            var erGyldigSertifikat = Sertifikatvalidator.ErGyldigResponssertifikat(_sertifikat);

            if (!erGyldigSertifikat)
            {
                throw new SdpSecurityException("Sertifikatet som er angitt i signaturen er ikke en del av en gyldig sertifikatkjede.");
            }
        }

        /// <summary>
        /// Sjekker at motatt soap dokument har samme digest verdier for body og dokumentpakke i avsendt brev vha motatt NonRepudiationInformation element. 
        /// </summary>
        /// <param name="guidHandler">Samme guid handler som ble benyttet for å generere det avsendte brevet.</param>
        private void ValiderDigest(GuidUtility guidHandler)
       {
            var sendtMeldingDigestSti = "/env:Envelope/env:Header/wsse:Security/ds:Signature/ds:SignedInfo/ds:Reference[@URI='{0}']/ds:DigestValue";
            var mottattSvarDigestSti = "/env:Envelope/env:Header/eb:Messaging/eb:SignalMessage/eb:Receipt/ebbp:NonRepudiationInformation/ebbp:MessagePartNRInformation/ds:Reference[@URI='{0}']/ds:DigestValue";

            var ider = new List<string>
            {
                string.Format("#{0}", guidHandler.BodyId),
                string.Format("cid:{0}", guidHandler.DokumentpakkeId)
            };

           foreach (var id in ider)
           {
                string sendtMeldingDigest;
                string mottattSvarDigest;

                var erGyldigDigest = ValiderDigestElement(sendtMeldingDigestSti, mottattSvarDigestSti, id, out sendtMeldingDigest, out mottattSvarDigest);
                if (!erGyldigDigest)
                {
                    throw new SdpSecurityException(string.Format("Digest verdien av uri {0} for sendt melding ({1}) matcher ikke motatt digest ({2}).", id,sendtMeldingDigest, mottattSvarDigest));
                }
            }
        }

        private bool ValiderDigestElement(string sendtMeldingDigestSti, string mottattSvarDigestSti, string id, out string sendtMeldingDigest, out string mottattSvarDigest)
        {
            sendtMeldingDigest = SendtMelding.SelectSingleNode(string.Format(sendtMeldingDigestSti, id), nsMgr).InnerText;
            mottattSvarDigest = Respons.SelectSingleNode(string.Format(mottattSvarDigestSti, id), nsMgr).InnerText;

            return sendtMeldingDigest == mottattSvarDigest;
        }

        /// <summary>
        /// Sjekker at soap envelopen inneholder timestamp, body og messaging element med korrekt id og referanser i security signaturen.
        /// </summary>
        private void ValiderSignaturelementer()
        {
            string[] requiredSignatureElements = { "/env:Envelope/env:Header/wsse:Security/wsu:Timestamp", "/env:Envelope/env:Body", "/env:Envelope/env:Header/eb:Messaging" };

            foreach (var elementXPath in requiredSignatureElements)
            {
                XmlNodeList nodes;
                ResponsInneholderPåkrevdeNoder(elementXPath, out nodes);

                var elementId = ElementId(nodes);
                FinnReferanseTilNodeISignaturElement(elementId, elementXPath);

                var targetNode = HentMålnode(elementId);
                if (targetNode != nodes[0])
                    throw new SdpSecurityException(string.Format("Signaturreferansen med id '{0}' må refererer til node med sti '{1}'", elementId, elementXPath));
            }
        }

        private XmlElement HentMålnode(string elementId)
        {
            var targetNode = _signedXmlWithAgnosticId.GetIdElement(Respons, elementId);
            return targetNode;
        }

        private void FinnReferanseTilNodeISignaturElement(string elementId, string elementXPath)
        {
            var references = _signaturnode.SelectNodes(string.Format("./ds:SignedInfo/ds:Reference[@URI='#{0}']", elementId),
                nsMgr);
            if (references == null || references.Count == 0)
                throw new SdpSecurityException(string.Format("Kan ikke finne påkrevet refereanse til element '{0}' i signatur fra meldingsformidler.",elementXPath));
            if (references.Count > 1)
                throw new SdpSecurityException(string.Format("Påkrevet refereanse til element '{0}' kan kun forekomme én gang i signatur fra meldingsformidler. Ble funnet {1} ganger.",elementXPath, references.Count));
        }

        private static string ElementId(XmlNodeList nodes)
        {
            var elementId = nodes[0].Attributes["wsu:Id"].Value;
            return elementId;
        }

        private void ResponsInneholderPåkrevdeNoder(string elementXPath, out XmlNodeList nodes)
        {
            nodes = Respons.SelectNodes(elementXPath, nsMgr);
            if (nodes == null || nodes.Count == 0)
                throw new SdpSecurityException(string.Format("Kan ikke finne påkrevet element '{0}' i svar fra meldingsformidler.",elementXPath));
            if (nodes.Count > 1)
                throw new SdpSecurityException(string.Format("Påkrevet element '{0}' kan kun forekomme én gang i svar fra meldingsformidler. Ble funnet {1} ganger.",elementXPath, nodes.Count));
        }
    }
}
