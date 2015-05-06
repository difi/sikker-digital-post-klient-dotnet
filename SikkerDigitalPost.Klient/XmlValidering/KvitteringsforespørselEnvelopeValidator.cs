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

using System.IO;
using System.Xml;
using ApiClientShared;
using Difi.Felles.Utility;

namespace SikkerDigitalPost.Klient.XmlValidering
{
    internal class KvitteringsforespørselEnvelopeValidator : XmlValidator
    {
        private static readonly ResourceUtility ResourceUtility = new ResourceUtility("Difi.SikkerDigitalPost.Klient.XmlValidering.xsd");

        public KvitteringsforespørselEnvelopeValidator()
        {
            LeggTilXsdRessurs(Navnerom.SoapEnvelopeEnv12, HentRessurs("w3.soap-envelope.xsd"));
            LeggTilXsdRessurs(Navnerom.SoapEnvelope, HentRessurs("xmlsoap.envelope.xsd"));
            LeggTilXsdRessurs(Navnerom.EbXmlCore, HentRessurs("ebxml.ebms-header-3_0-200704.xsd"));
            LeggTilXsdRessurs(Navnerom.EbppSignals, HentRessurs("ebxml.ebbp-signals-2.0.xsd"));
            LeggTilXsdRessurs(Navnerom.XmlDsig, HentRessurs("w3.xmldsig-core-schema.xsd"));
            LeggTilXsdRessurs(Navnerom.XmlEnc, HentRessurs("w3.xenc-schema.xsd"));
            LeggTilXsdRessurs(Navnerom.Xlink, HentRessurs("w3.xlink.xsd"));
            LeggTilXsdRessurs(Navnerom.Xml1998, HentRessurs("w3.xml.xsd"));
            LeggTilXsdRessurs(Navnerom.XmlExcC14n, HentRessurs("w3.exc-c14n.xsd"));
            LeggTilXsdRessurs(Navnerom.WssecurityUtility10, HentRessurs("wssecurity.oasis-200401-wss-wssecurity-utility-1.0.xsd"));
            LeggTilXsdRessurs(Navnerom.WssecuritySecext10, HentRessurs("wssecurity.oasis-200401-wss-wssecurity-secext-1.0.xsd"));
        }

        private XmlReader HentRessurs(string path)
        {
            var bytes = ResourceUtility.ReadAllBytes(true, path);
            return XmlReader.Create(new MemoryStream(bytes));
        }
    }
}
