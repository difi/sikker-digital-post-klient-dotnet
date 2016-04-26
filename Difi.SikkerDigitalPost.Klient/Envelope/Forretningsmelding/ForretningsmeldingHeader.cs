using System.Security.Cryptography.Xml;
using System.Xml;
using Difi.Felles.Utility.Security;
using Difi.SikkerDigitalPost.Klient.Envelope.Abstract;
using Difi.SikkerDigitalPost.Klient.Security;

namespace Difi.SikkerDigitalPost.Klient.Envelope.Forretningsmelding
{
    internal class ForretningsmeldingHeader : AbstractHeader
    {
        public ForretningsmeldingHeader(EnvelopeSettings settings, XmlDocument context)
            : base(settings, context)
        {
        }

        protected override XmlNode SecurityElement()
        {
            return Security = new Security(Settings, Context).Xml();
        }

        protected override XmlNode MessagingElement()
        {
            var messaging = new ForretningsmeldingMessaging(Settings, Context);
            return messaging.Xml();
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

            //Partinfo/Dokumentpakke
            {
                var partInfoReference = new Sha256Reference(Settings.DocumentBundle.BundleBytes)
                {
                    Uri = $"cid:{Settings.GuidUtility.DokumentpakkeId}"
                };
                partInfoReference.AddTransform(new AttachmentContentSignatureTransform());
                signed.AddReference(partInfoReference);
            }

            signed.KeyInfo.AddClause(new SecurityTokenReferenceClause("#" + Settings.GuidUtility.BinarySecurityTokenId));
            signed.ComputeSignature();

            Security.AppendChild(Context.ImportNode(signed.GetXml(), true));
        }
    }
}