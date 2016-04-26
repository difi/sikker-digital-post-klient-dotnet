using System.Security.Cryptography.Xml;
using System.Xml;
using Difi.Felles.Utility.Security;
using Difi.SikkerDigitalPost.Klient.Envelope.Abstract;
using Difi.SikkerDigitalPost.Klient.Security;

namespace Difi.SikkerDigitalPost.Klient.Envelope.Kvitteringsforespørsel
{
    internal class KvitteringsforespørselHeader : AbstractHeader
    {
        public KvitteringsforespørselHeader(EnvelopeSettings settings, XmlDocument context)
            : base(settings, context)
        {
        }

        protected override XmlNode SecurityElement()
        {
            return Security = new Security(Settings, Context).Xml();
        }

        protected override XmlNode MessagingElement()
        {
            var messaging = new KvitteringsforespørselMessaging(Settings, Context).Xml();
            return messaging;
        }

        public override void AddSignatureElement()
        {
            SignedXml signed = new SignedXmlWithAgnosticId(Context, Settings.Databehandler.Sertifikat, "env");

            //Body
            {
                var bodyReference = new Sha256Reference("#" + Settings.GuidUtility.BodyId);
                bodyReference.AddTransform(new XmlDsigExcC14NTransform());
                signed.AddReference(bodyReference);
            }

            //TimestampElement
            {
                var timestampReference = new Sha256Reference("#" + Settings.GuidUtility.TimestampId);
                timestampReference.AddTransform(new XmlDsigExcC14NTransform("wsse env"));
                signed.AddReference(timestampReference);
            }

            //EbMessaging
            {
                var ebMessagingReference = new Sha256Reference("#" + Settings.GuidUtility.EbMessagingId);
                ebMessagingReference.AddTransform(new XmlDsigExcC14NTransform());
                signed.AddReference(ebMessagingReference);
            }

            signed.KeyInfo.AddClause(new SecurityTokenReferenceClause("#" + Settings.GuidUtility.BinarySecurityTokenId));
            signed.ComputeSignature();

            Security.AppendChild(Context.ImportNode(signed.GetXml(), true));
        }
    }
}