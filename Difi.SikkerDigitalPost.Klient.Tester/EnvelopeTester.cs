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
        [TestClass]
        public class XsdValidering : EnvelopeTester
        {
            [TestMethod]
            public void ValidereEnvelopeMotXsdValiderer()
            {
                var envelope = DomainUtility.GetForretningsmeldingEnvelopeWithTestTestCertificate();
                var forretningsmeldingEnvelopeXml = envelope.Xml();
                var envelopeValidator = new SdpXmlValidator();
                string validationMessages;
                var validert = envelopeValidator.Validate(forretningsmeldingEnvelopeXml.OuterXml,out validationMessages);

                Assert.IsTrue(validert, validationMessages);
            }

            [TestMethod]
            public void LagUgyldigSecurityNodeXsdValidererIkke()
            {
                var envelope = DomainUtility.GetForretningsmeldingEnvelopeWithTestTestCertificate();
                var forretningsmeldingEnvelopeXml = envelope.Xml();
                var envelopeValidator = new SdpXmlValidator();

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

                var securityNode = forretningsmeldingEnvelopeXml.DocumentElement.SelectSingleNode("//wsse:Security",
                    namespaceManager);

                var gammelVerdi = securityNode.Attributes["mustUnderstand"].Value;
                securityNode.Attributes["mustUnderstand"].Value = "en_tekst_som_ikke_er_bool";

                string validationMessages;
                var validert = envelopeValidator.Validate(forretningsmeldingEnvelopeXml.OuterXml, out validationMessages);
                Assert.IsFalse(validert, validationMessages);

                securityNode.Attributes["mustUnderstand"].Value = gammelVerdi;
            }
        }
    }
}