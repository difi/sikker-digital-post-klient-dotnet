using System;
using System.Security.Cryptography.Xml;
using System.Xml;
using Difi.Felles.Utility.Security;
using Difi.SikkerDigitalPost.Klient.Envelope.Abstract;
using Difi.SikkerDigitalPost.Klient.Security;
using Difi.SikkerDigitalPost.Klient.Utilities;

namespace Difi.SikkerDigitalPost.Klient.Envelope.Forretningsmelding
{
    internal class StandardBusinessDocument : EnvelopeXmlPart
    {
        private readonly DateTime _creationDateAndtime;

        public StandardBusinessDocument(EnvelopeSettings settings, XmlDocument context)
            : base(settings, context)
        {
            _creationDateAndtime = DateTime.UtcNow;
        }

        public override XmlNode Xml()
        {
            var standardBusinessDocumentElement = Context.CreateElement("ns3", "StandardBusinessDocument", NavneromUtility.StandardBusinessDocumentHeader);
            standardBusinessDocumentElement.SetAttribute("xmlns:ns3", NavneromUtility.StandardBusinessDocumentHeader);
            standardBusinessDocumentElement.SetAttribute("xmlns:ns5", NavneromUtility.XmlDsig);
            standardBusinessDocumentElement.SetAttribute("xmlns:ns9", NavneromUtility.DifiSdpSchema10);
            Context.AppendChild(standardBusinessDocumentElement);

            standardBusinessDocumentElement.AppendChild(StandardBusinessDocumentHeaderElement());
            var digitalPostElement = standardBusinessDocumentElement.AppendChild(DigitalPostElement());
            digitalPostElement.PrependChild(Context.ImportNode(SignatureElement().GetXml(), true));
            return standardBusinessDocumentElement;
        }

        private XmlNode StandardBusinessDocumentHeaderElement()
        {
            var standardBusinessDocumentHeader = new StandardBusinessDocumentHeader(Settings, Context, _creationDateAndtime);
            return standardBusinessDocumentHeader.Xml();
        }

        private XmlNode DigitalPostElement()
        {
            var digitalPost = new DigitalPostElement(Settings, Context);
            return digitalPost.Xml();
        }

        private SignedXml SignatureElement()
        {
            SignedXml signedXml = new SignedXmlWithAgnosticId(Context, Settings.Databehandler.Sertifikat);

            var reference = new Sha256Reference("");
            reference.AddTransform(new XmlDsigEnvelopedSignatureTransform());
            reference.AddTransform(new XmlDsigExcC14NTransform("ns9"));
            signedXml.AddReference(reference);

            var keyInfoX509Data = new KeyInfoX509Data(Settings.Databehandler.Sertifikat);
            signedXml.KeyInfo.AddClause(keyInfoX509Data);

            signedXml.ComputeSignature();

            return signedXml;
        }
    }
}