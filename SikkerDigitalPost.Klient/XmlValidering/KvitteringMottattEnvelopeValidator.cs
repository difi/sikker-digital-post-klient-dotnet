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
    internal class KvitteringMottattEnvelopeValidator : XmlValidator
    {
        private static readonly ResourceUtility ResourceUtility = new ResourceUtility("SikkerDigitalPost.Klient.XmlValidering.xsd");

        public KvitteringMottattEnvelopeValidator()
        {
            LeggTilXsdRessurs(Navnerom.env, HentRessurs("w3.soap-envelope.xsd"));
            LeggTilXsdRessurs(Navnerom.Ns4, HentRessurs("xmlsoap.envelope.xsd"));
            LeggTilXsdRessurs(Navnerom.Ns6, HentRessurs("ebxml.ebms-header-3_0-200704.xsd"));
            LeggTilXsdRessurs(Navnerom.Ns7, HentRessurs("ebxml.ebbp-signals-2.0.xsd"));
            LeggTilXsdRessurs(Navnerom.Ns5, HentRessurs("w3.xmldsig-core-schema.xsd"));
            LeggTilXsdRessurs(Navnerom.enc, HentRessurs("w3.xenc-schema.xsd"));
            LeggTilXsdRessurs(Navnerom.Ns8, HentRessurs("w3.xlink.xsd"));
            LeggTilXsdRessurs(Navnerom.xml1998, HentRessurs("w3.xml.xsd"));
            LeggTilXsdRessurs(Navnerom.ec, HentRessurs("w3.exc-c14n.xsd"));
            LeggTilXsdRessurs(Navnerom.wsu, HentRessurs("wssecurity.oasis-200401-wss-wssecurity-utility-1.0.xsd"));
            LeggTilXsdRessurs(Navnerom.wsse, HentRessurs("wssecurity.oasis-200401-wss-wssecurity-secext-1.0.xsd"));
        }

        private XmlReader HentRessurs(string path)
        {
            var bytes = ResourceUtility.ReadAllBytes(true, path);
            return XmlReader.Create(new MemoryStream(bytes));
        }
    }
}
