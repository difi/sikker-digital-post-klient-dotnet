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

using System.Security.Cryptography.Xml;
using System.Xml;
using Difi.SikkerDigitalPost.Klient.Envelope.Abstract;
using Difi.SikkerDigitalPost.Klient.Security;

namespace Difi.SikkerDigitalPost.Klient.Envelope.Kvitteringsbekreftelse
{
    internal class KvitteringsbekreftelseHeader : AbstractHeader
    {
        public KvitteringsbekreftelseHeader(EnvelopeSettings settings, XmlDocument context) : base(settings, context)
        {
        }

        protected override XmlNode SecurityElement()
        {
            return Security = new Security(Settings, Context).Xml();
        }

        protected override XmlNode MessagingElement()
        {
            var messaging = new KvitteringsbekreftelseMessaging(Settings, Context).Xml();
            return messaging;
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
