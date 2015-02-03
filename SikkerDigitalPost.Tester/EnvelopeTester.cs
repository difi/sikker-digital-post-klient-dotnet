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

using System.Collections.Generic;
using System.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SikkerDigitalPost.Klient;
using SikkerDigitalPost.Klient.XmlValidering;

namespace SikkerDigitalPost.Tester
{
    [TestClass]
    public class EnvelopeTester : TestBase
    {
        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            Initialiser();
        }

        [TestMethod]
        public void ValidereEnvelopeMotXsdValiderer()
        {
            var forretningsmeldingEnvelopeXml = Envelope.Xml();
            var  envelopeValidator = new ForretningsmeldingEnvelopeValidator();
            var validert = envelopeValidator.ValiderDokumentMotXsd(forretningsmeldingEnvelopeXml.OuterXml);

            Assert.IsTrue(validert, envelopeValidator.ValideringsVarsler);
        }

        [TestMethod]
        public void LagUgyldigSecurityNodeXsdValidererIkke()
        {
           var forretningsmeldingEnvelopeXml = Envelope.Xml();
            var envelopeValidator = new ForretningsmeldingEnvelopeValidator();

            //Endre til ugyldig forretningsmeldingenvelope
            var namespaceManager = new XmlNamespaceManager(forretningsmeldingEnvelopeXml.NameTable);
            namespaceManager.AddNamespace("env", Navnerom.env);
            namespaceManager.AddNamespace("eb", Navnerom.eb);
            namespaceManager.AddNamespace("ds", Navnerom.ds);
            namespaceManager.AddNamespace("wsse", Navnerom.wsse);
            namespaceManager.AddNamespace("wsu", Navnerom.wsu);
            namespaceManager.AddNamespace("ns3", Navnerom.Ns3);
            namespaceManager.AddNamespace("ns9", Navnerom.Ns9);
            namespaceManager.AddNamespace("ns5", Navnerom.Ns5);

            var securityNode = forretningsmeldingEnvelopeXml.DocumentElement.SelectSingleNode("//wsse:Security", namespaceManager);

            var gammelVerdi = securityNode.Attributes["mustUnderstand"].Value;
            securityNode.Attributes["mustUnderstand"].Value = "en_tekst_som_ikke_er_bool";

            var validert = envelopeValidator.ValiderDokumentMotXsd(forretningsmeldingEnvelopeXml.OuterXml);
            Assert.IsFalse(validert, envelopeValidator.ValideringsVarsler);

            securityNode.Attributes["mustUnderstand"].Value = gammelVerdi;
        }
    }
}
