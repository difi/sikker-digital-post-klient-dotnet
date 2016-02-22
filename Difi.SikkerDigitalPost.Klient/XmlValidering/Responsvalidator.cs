﻿using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Xml;
using Difi.Felles.Utility;
using Difi.Felles.Utility.Security;
using Difi.SikkerDigitalPost.Klient.Domene.Exceptions;
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

        public Sertifikatkjedevalidator Sertifikatkjedevalidator { get; internal set; }
        
        private readonly XmlNamespaceManager _nsMgr;
        private SignedXmlWithAgnosticId _signedXmlWithAgnosticId;
        private XmlElement _signaturnode;
        private X509Certificate2 _sertifikat;

        /// <summary>
        /// Oppretter en ny instanse av responsvalidatoren.
        /// </summary>
        /// <param name="sendtMelding">Soap meldingen som har blitt sendt til meldingsformidleren.</param>
        /// <param name="respons">Et soap dokument i tekstform. Dette er svaret som har blitt motatt fra meldingsformidleren ved en forsendelse av brev eller kvittering.</param>
        /// <param name="sertifikatkjedevalidator"></param>
        public Responsvalidator(XmlDocument sendtMelding, XmlDocument respons, Sertifikatkjedevalidator sertifikatkjedevalidator)
        {
            Respons = respons;
            SendtMelding = sendtMelding;
            Sertifikatkjedevalidator = sertifikatkjedevalidator;

            
            _nsMgr = new XmlNamespaceManager(Respons.NameTable);
            _nsMgr.AddNamespace("env", NavneromUtility.SoapEnvelopeEnv12);
            _nsMgr.AddNamespace("wsse", NavneromUtility.WssecuritySecext10);
            _nsMgr.AddNamespace("ds", NavneromUtility.XmlDsig);
            _nsMgr.AddNamespace("eb", NavneromUtility.EbXmlCore);
            _nsMgr.AddNamespace("wsu", NavneromUtility.WssecurityUtility10);
            _nsMgr.AddNamespace("ebbp", NavneromUtility.EbppSignals);
            _nsMgr.AddNamespace("sbd", NavneromUtility.StandardBusinessDocumentHeader);
            _nsMgr.AddNamespace("difi", NavneromUtility.DifiSdpSchema10);
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

        public void ValiderTomKøKvittering()
        {
            ValiderHeaderSignatur();
        }

        private void ValiderHeaderSignatur()
        {
            XmlNode responsRot = Respons.DocumentElement;
            _signaturnode = (XmlElement)responsRot.SelectSingleNode("/env:Envelope/env:Header/wsse:Security/ds:Signature", _nsMgr);
            _signedXmlWithAgnosticId = new SignedXmlWithAgnosticId(Respons);

            ValiderSignaturelementer();
            ValiderSignaturOgSertifikat("/env:Envelope/env:Header/wsse:Security/wsse:BinarySecurityToken");
        }

        private void ValiderKvitteringSignatur()
        {
            var standardBusinessDocumentNode =
                Respons.SelectSingleNode("/env:Envelope/env:Body/sbd:StandardBusinessDocument", _nsMgr);

            if (standardBusinessDocumentNode != null)
            {
                var standardBusinessDocument = XmlNodeToXmlDocument(standardBusinessDocumentNode);

                _signedXmlWithAgnosticId = new SignedXmlWithAgnosticId(standardBusinessDocument);
                _signaturnode = (XmlElement) standardBusinessDocument.SelectSingleNode("//ds:Signature", _nsMgr);

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
            _sertifikat = new X509Certificate2(Convert.FromBase64String(_signaturnode.SelectSingleNode(path, _nsMgr).InnerText));
            ValiderResponssertifikat();

            _signedXmlWithAgnosticId.LoadXml(_signaturnode);

            AsymmetricAlgorithm asymmetricAlgorithm;
            if (!_signedXmlWithAgnosticId.CheckSignatureReturningKey(out asymmetricAlgorithm))
                throw new SdpSecurityException("Signaturen i motatt svar er ikke gyldig.");

            if (asymmetricAlgorithm.ToXmlString(false) != _sertifikat.PublicKey.Key.ToXmlString(false))
                throw new SdpSecurityException(
                    $"Sertifikatet som er benyttet for å validere signaturen er ikke det samme som er spesifisert i {path} elementet.");
        }

        private void ValiderResponssertifikat()
        {
            var erGyldigSertifikat = Sertifikatkjedevalidator.ErGyldigResponssertifikat(_sertifikat);

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
                $"#{guidHandler.BodyId}",
                $"cid:{guidHandler.DokumentpakkeId}"
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
            sendtMeldingDigest = null;
            mottattSvarDigest = null;

            var sendtMeldingSelectedNode = SendtMelding.SelectSingleNode(string.Format(sendtMeldingDigestSti, id), _nsMgr);
            if (sendtMeldingSelectedNode != null)
                sendtMeldingDigest = sendtMeldingSelectedNode.InnerText;

            var responsSelectedNode = Respons.SelectSingleNode(string.Format(mottattSvarDigestSti, id), _nsMgr);
            if (responsSelectedNode != null)
                mottattSvarDigest = responsSelectedNode.InnerText;

            return sendtMeldingDigest != null && responsSelectedNode != null && sendtMeldingDigest == mottattSvarDigest;
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
                    throw new SdpSecurityException(
                        $"Signaturreferansen med id '{elementId}' må refererer til node med sti '{elementXPath}'");
            }
        }

        private XmlElement HentMålnode(string elementId)
        {
            var targetNode = _signedXmlWithAgnosticId.GetIdElement(Respons, elementId);
            return targetNode;
        }

        private void FinnReferanseTilNodeISignaturElement(string elementId, string elementXPath)
        {
            var references = _signaturnode.SelectNodes($"./ds:SignedInfo/ds:Reference[@URI='#{elementId}']",
                _nsMgr);
            if (references == null || references.Count == 0)
                throw new SdpSecurityException(
                    $"Kan ikke finne påkrevet refereanse til element '{elementXPath}' i signatur fra meldingsformidler.");
            if (references.Count > 1)
                throw new SdpSecurityException(
                    $"Påkrevet refereanse til element '{elementXPath}' kan kun forekomme én gang i signatur fra meldingsformidler. Ble funnet {references.Count} ganger.");
        }

        private static string ElementId(XmlNodeList nodes)
        {
            var elementId = nodes[0].Attributes["wsu:Id"].Value;
            return elementId;
        }

        private void ResponsInneholderPåkrevdeNoder(string elementXPath, out XmlNodeList nodes)
        {
            nodes = Respons.SelectNodes(elementXPath, _nsMgr);
            if (nodes == null || nodes.Count == 0)
                throw new SdpSecurityException(
                    $"Kan ikke finne påkrevet element '{elementXPath}' i svar fra meldingsformidler.");
            if (nodes.Count > 1)
                throw new SdpSecurityException(
                    $"Påkrevet element '{elementXPath}' kan kun forekomme én gang i svar fra meldingsformidler. Ble funnet {nodes.Count} ganger.");
        }
    }
}
