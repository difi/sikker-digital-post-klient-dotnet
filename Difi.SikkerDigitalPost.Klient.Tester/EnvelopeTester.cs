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

using System.Xml;
using Difi.SikkerDigitalPost.Klient.Tester.Utilities;
using Difi.SikkerDigitalPost.Klient.Utilities;
using Difi.SikkerDigitalPost.Klient.XmlValidering;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Difi.SikkerDigitalPost.Klient.Tester
{
    [TestClass]
    public class EnvelopeTester
    {
        [TestMethod]
        public void ValidereEnvelopeMotXsdValiderer()
        {
            var envelope = DomeneUtility.GetForretningsmeldingEnvelope();
            var forretningsmeldingEnvelopeXml = envelope.Xml();
            var  envelopeValidator = new ForretningsmeldingEnvelopeValidator();
            var validert = envelopeValidator.ValiderDokumentMotXsd(forretningsmeldingEnvelopeXml.OuterXml);

            Assert.IsTrue(validert, envelopeValidator.ValideringsVarsler);
        }

        [TestMethod]
        public void LagUgyldigSecurityNodeXsdValidererIkke()
        {
            var envelope = DomeneUtility.GetForretningsmeldingEnvelope();
            var forretningsmeldingEnvelopeXml = envelope.Xml();
            var envelopeValidator = new ForretningsmeldingEnvelopeValidator();

            //Endre til ugyldig forretningsmeldingenvelope
            var namespaceManager = new XmlNamespaceManager(forretningsmeldingEnvelopeXml.NameTable);
            namespaceManager.AddNamespace("env", NavneromUtility.SoapEnvelopeEnv12);
            namespaceManager.AddNamespace("eb", NavneromUtility.EbXmlCore);
            namespaceManager.AddNamespace("ds", NavneromUtility.XmlDsig);
            namespaceManager.AddNamespace("wsse", NavneromUtility.WssecuritySecext10);
            namespaceManager.AddNamespace("wsu", NavneromUtility.WssecurityUtility10);
            namespaceManager.AddNamespace("ns3", NavneromUtility.StandardBusinessDocumentHeader);
            namespaceManager.AddNamespace("ns9", NavneromUtility.DifiSdpSchema10);
            namespaceManager.AddNamespace("ns5", NavneromUtility.XmlDsig);

            var securityNode = forretningsmeldingEnvelopeXml.DocumentElement.SelectSingleNode("//wsse:Security", namespaceManager);

            var gammelVerdi = securityNode.Attributes["mustUnderstand"].Value;
            securityNode.Attributes["mustUnderstand"].Value = "en_tekst_som_ikke_er_bool";

            var validert = envelopeValidator.ValiderDokumentMotXsd(forretningsmeldingEnvelopeXml.OuterXml);
            Assert.IsFalse(validert, envelopeValidator.ValideringsVarsler);

            securityNode.Attributes["mustUnderstand"].Value = gammelVerdi;
        }
    }
}
