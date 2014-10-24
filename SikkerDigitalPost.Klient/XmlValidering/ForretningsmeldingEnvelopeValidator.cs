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

using System.Xml.Schema;
using SikkerDigitalPost.Klient.Envelope;
using SikkerDigitalPost.Klient.XmlValidering;

namespace SikkerDigitalPost.Klient.XmlValidering
{

    internal class ForretningsmeldingEnvelopeValidator : Xmlvalidator
    {
        protected override XmlSchemaSet GenererSchemaSet()
        {
            var schemaSet = new XmlSchemaSet();

            schemaSet.Add(Navnerom.env, SoapEnvelopeXsdPath());
            schemaSet.Add(Navnerom.Ns4, EnvelopeXsdPath());

            schemaSet.Add(Navnerom.Ns6, EbmsHeaderXsdPath());

            schemaSet.Add(Navnerom.Ns9, FellesXsdPath());
            schemaSet.Add(Navnerom.Ns9, MeldingXsdPath());

            schemaSet.Add(Navnerom.Ns5, XmlDsigCoreXsdPath());
            //schemaSet.Add(Navnerom.enc, XmlXencXsdPath());
            schemaSet.Add("http://www.w3.org/XML/1998/namespace", XmlXsdPath());
            schemaSet.Add("http://www.w3.org/2001/10/xml-exc-c14n#", ExecC14nXsdPath());

            schemaSet.Add(Navnerom.Ns3, StandardBusinessDocumentHeaderXsdPath());
            schemaSet.Add(Navnerom.Ns3, DocumentIdentificationXsdPath());
            schemaSet.Add(Navnerom.Ns3, SbdhManifestXsdPath());
            schemaSet.Add(Navnerom.Ns3, PartnerXsdPath());
            schemaSet.Add(Navnerom.Ns3, BusinessScopeXsdPath());
            schemaSet.Add(Navnerom.Ns3, BasicTypesXsdPath());

            schemaSet.Add(Navnerom.wsu, WsSecurityUtilityXsdPath());
            schemaSet.Add(Navnerom.wsse, WsSecuritySecExt1_0XsdPath());

            return schemaSet;
        }
    }
}
