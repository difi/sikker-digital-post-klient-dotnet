using System;
using System.Security.Cryptography.Xml;
using System.Xml;
using SikkerDigitalPost.Domene.Entiteter.Aktører;
using SikkerDigitalPost.Domene.Entiteter.Post;
using SikkerDigitalPost.Klient.Xml;

namespace SikkerDigitalPost.Klient.Envelope.EnvelopeBody
{
    internal class StandardBusinessDocument : XmlPart
    {
        private readonly DateTime _creationDateAndtime;

        public StandardBusinessDocument(Envelope rot)
            : base(rot)
        {
            _creationDateAndtime = DateTime.UtcNow;
        }

        public override XmlElement Xml()
        {
            XmlDocument standardBusinessDocument = new XmlDocument();
            standardBusinessDocument.PreserveWhitespace = true;
            var standardBusinessDocumentElement = standardBusinessDocument.CreateElement("ns3", "StandardBusinessDocument", Navnerom.Ns3);
            standardBusinessDocumentElement.SetAttribute("xmlns:ns3", Navnerom.Ns3);
            standardBusinessDocumentElement.SetAttribute("xmlns:ns5", Navnerom.Ns5);
            standardBusinessDocumentElement.SetAttribute("xmlns:ns9", Navnerom.Ns9);

            standardBusinessDocumentElement.AppendChild(StandardBusinessDocumentHeaderElement());
            var digitalPostElement = standardBusinessDocumentElement.AppendChild(DigitalPostElement());

            XmlElement signatur = SignatureElement(standardBusinessDocument).GetXml();
            digitalPostElement.PrependChild(standardBusinessDocument.ImportNode(signatur, true));

            return standardBusinessDocument.DocumentElement;
        }

        private XmlElement StandardBusinessDocumentHeaderElement()
        {
            var standardBusinessDocumentHeader = new StandardBusinessDocumentHeader(Rot, _creationDateAndtime);
            return standardBusinessDocumentHeader.Xml();
        }

        private XmlElement DigitalPostElement()
        {
            var digitalPost = new DigitalPostElement(Rot);
            return digitalPost.Xml();
        }

        private SignedXml SignatureElement(XmlDocument standardBusinessDocument)
        {
            SignedXml signedXml = new SignedXmlWithAgnosticId(standardBusinessDocument, Rot.Databehandler.Sertifikat);

            var reference = new Sha256Reference("");
            reference.AddTransform(new XmlDsigEnvelopedSignatureTransform());
            reference.AddTransform(new XmlDsigExcC14NTransform("ns9"));
            signedXml.AddReference(reference);

            var keyInfoX509Data = new KeyInfoX509Data(Rot.Databehandler.Sertifikat);
            signedXml.KeyInfo.AddClause(keyInfoX509Data);

            signedXml.ComputeSignature();

            return signedXml;
        }
    }
}
