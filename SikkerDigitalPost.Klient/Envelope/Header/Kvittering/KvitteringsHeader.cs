using System.Security.Cryptography.Xml;
using System.Xml;
using SikkerDigitalPost.Klient.Envelope.Abstract;
using SikkerDigitalPost.Klient.Envelope.Header.Forretningsmelding;
using SikkerDigitalPost.Klient.Xml;

namespace SikkerDigitalPost.Klient.Envelope.Header.Kvittering
{
    internal class KvitteringsHeader : AbstractHeader
    {
        public KvitteringsHeader(EnvelopeSettings settings, XmlDocument context) : base(settings, context)
        {
        }

        public override XmlNode Xml()
        {
            throw new System.NotImplementedException();
        }

        protected override XmlNode SecurityElement()
        {

            var security = new Security(Settings, Context);
            return security.Xml();
        }

        protected override XmlNode MessagingElement()
        {
            throw new System.NotImplementedException();
        }

        public override void AddSignatureElement()
        {
            SignedXml signed = new SignedXmlWithAgnosticId(Context, Settings.Databehandler.Sertifikat, "env");

            //Body
            {
                var bodyReference = new Sha256Reference("#" + Settings.GuidHandler.BodyId);
                bodyReference.AddTransform(new XmlDsigExcC14NTransform());
                signed.AddReference(bodyReference);
            }

            //TimestampElement
            {
                var timestampReference = new Sha256Reference("#" + Settings.GuidHandler.TimestampId);
                timestampReference.AddTransform(new XmlDsigExcC14NTransform("wsse env"));
                signed.AddReference(timestampReference);
            }

            //EbMessaging
            {
                var ebMessagingReference = new Sha256Reference("#" + Settings.GuidHandler.EbMessagingId);
                ebMessagingReference.AddTransform(new XmlDsigExcC14NTransform());
                signed.AddReference(ebMessagingReference);
            }
            
            signed.KeyInfo.AddClause(new SecurityTokenReferenceClause("#" + Settings.GuidHandler.BinarySecurityTokenId));
            signed.ComputeSignature();

            Security.AppendChild(Context.ImportNode(signed.GetXml(), true));
        }
    }
}
