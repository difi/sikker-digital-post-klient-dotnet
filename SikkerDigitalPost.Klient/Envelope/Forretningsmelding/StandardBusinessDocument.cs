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
    internal class StandardBusinessDocument : EnvelopeXmlPart
    {
        private readonly DateTime _creationDateAndtime;

        public StandardBusinessDocument(EnvelopeSettings settings, XmlDocument context) : base(settings, context)
        {
            _creationDateAndtime = DateTime.UtcNow;
        }

        public override XmlNode Xml()
        {
            var standardBusinessDocumentElement = Context.CreateElement("ns3", "StandardBusinessDocument", Navnerom.Ns3);
            standardBusinessDocumentElement.SetAttribute("xmlns:ns3", Navnerom.Ns3);
            standardBusinessDocumentElement.SetAttribute("xmlns:ns5", Navnerom.Ns5);
            standardBusinessDocumentElement.SetAttribute("xmlns:ns9", Navnerom.Ns9);
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
