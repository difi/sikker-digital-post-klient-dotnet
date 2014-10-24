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
using System.Security.Cryptography.Xml;
using System.Xml;
using SikkerDigitalPost.Klient.Envelope.Abstract;
using SikkerDigitalPost.Klient.Security;

namespace SikkerDigitalPost.Klient.Envelope.Forretningsmelding
{
    internal class ForretningsmeldingHeader : AbstractHeader
    {
        
        public ForretningsmeldingHeader(EnvelopeSettings settings, XmlDocument context) : base(settings, context)
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

            //Partinfo/Dokumentpakke
            {
                var partInfoReference = new Sha256Reference(Settings.AsicEArkiv.Bytes);
                partInfoReference.Uri = String.Format("cid:{0}", Settings.GuidHandler.DokumentpakkeId);
                partInfoReference.AddTransform(new AttachmentContentSignatureTransform());
                signed.AddReference(partInfoReference);
            }

            signed.KeyInfo.AddClause(new SecurityTokenReferenceClause("#" + Settings.GuidHandler.BinarySecurityTokenId));
            signed.ComputeSignature();
            
            Security.AppendChild(Context.ImportNode(signed.GetXml(), true));
        }
    }
}
