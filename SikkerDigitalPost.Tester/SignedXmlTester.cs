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
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml;
using DigipostApiClientShared;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SikkerDigitalPost.Klient.Security;

namespace SikkerDigitalPost.Tester
{
    [TestClass]
    public class SignedXmlTester : TestBase
    {
        readonly ResourceUtility _resourceUtility = new ResourceUtility("SikkerDigitalPost.Tester.testdata");

        [ClassInitialize]
        public static void Initialize(TestContext context)
        {
            Initialiser();
        }

        [TestMethod]
        public void FindIdElement()
        {
            var tests = new[] {
                "<{0} {2}='{1}'></{0}>", 
                "<{0} {2}='{1}'></{0}>", 
                "<{0} {2}='{1}'></{0}>", 
                "<container><{0} {2}='{1}'></{0}></container>", 
                "<container><invalid Id='notThis' /><{0} {2}='{1}'></{0}></container>", 
                "<container><invalid ID='notThis'><{0} {2}='{1}'></{0}></invalid></container>",
                "<container xmlns='http://example.org'><{0} {2}='{1}'></{0}></container>", 
                "<a:container xmlns:a='http://example.org'><{0} {2}='{1}'></{0}></a:container>", 
                "<a:container xmlns:a='http://example.org'><a:{0} {2}='{1}'></a:{0}></a:container>", 
                "<a:container xmlns:a='http://example.org'><b:{0} xmlns:b='http://nowhere.com' {2}='{1}'></b:{0}></a:container>", 
                "<a:container xmlns:a='http://example.org'><{0} xmlns='' {2}='{1}'></{0}></a:container>", 
                "<a:container xmlns:a='http://example.org'><{0} xmlns='' a:{2}='{1}'></{0}></a:container>"};

            foreach (var item in tests)
            {
                foreach (var id in new string[] { "Id", "ID", "id" })
                {
                    var xml = new XmlDocument() { PreserveWhitespace = true };
                    xml.LoadXml(string.Format(item, "element", "value", id));

                    var signed = new SignedXmlWithAgnosticId(xml);
                    var response = signed.GetIdElement(xml, "value");

                    Assert.IsNotNull(response);
                    Assert.IsTrue(response.Attributes.OfType<XmlAttribute>().Any(a => a.LocalName == id && a.Value == "value"));
                }
            }
        }

        [TestMethod]
        public void GetPublicKey()
        {            
            XmlDocument doc = new XmlDocument { PreserveWhitespace = false };
            byte[] xmlBytes = _resourceUtility.ReadAllBytes(true, "1_response_kvittering_for_mottatt_forretningsmelding_fra_meldingsformidler_til_postavsender.xml");
            doc.LoadXml(Encoding.UTF8.GetString(xmlBytes));

            var mgr = new XmlNamespaceManager(doc.NameTable);
            mgr.AddNamespace("wsse", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd");
            mgr.AddNamespace("ds", "http://www.w3.org/2000/09/xmldsig#");

            // Find key
            var token = doc.SelectSingleNode("//wsse:BinarySecurityToken", mgr);
            var key = new X509Certificate2(Convert.FromBase64String(token.InnerText));

            var signed = new SignedXmlWithAgnosticId(doc);
            var signatureNode = (XmlElement)doc.SelectSingleNode("//ds:Signature", mgr);
            signed.LoadXml(signatureNode);            

            var result = typeof(SignedXmlWithAgnosticId).GetMethod("GetPublicKey", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(signed, null) as AsymmetricAlgorithm;

            Assert.AreEqual(result.ToXmlString(false), key.PublicKey.Key.ToXmlString(false));
        }
    }
}
