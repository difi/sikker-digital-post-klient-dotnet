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
using System.Xml.Schema;
using ApiClientShared;
using Difi.Felles.Utility;

namespace SikkerDigitalPost.Klient.XmlValidering
{
    internal class Signaturvalidator : XmlValidator
    {
        private static readonly ResourceUtility ResourceUtility = new ResourceUtility("SikkerDigitalPost.Klient.XmlValidering.xsd");

        public Signaturvalidator()
        {
            LeggTilXsdRessurs(Navnerom.Ns10, HentRessurs("w3.ts_102918v010201.xsd"));
            LeggTilXsdRessurs(Navnerom.Ns11, HentRessurs("w3.XAdES.xsd"));
            LeggTilXsdRessurs(Navnerom.Ns5, HentRessurs("w3.xmldsig-core-schema.xsd"));
        }

        private XmlReader HentRessurs(string path)
        {
            var bytes = ResourceUtility.ReadAllBytes(true, path);
            return XmlReader.Create(new MemoryStream(bytes));
        }
    }
}
