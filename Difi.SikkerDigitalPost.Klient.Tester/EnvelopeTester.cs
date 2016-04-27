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
                var envelopeValidator = new ForretningsmeldingEnvelopeValidator();
                var validert = envelopeValidator.Validate(forretningsmeldingEnvelopeXml.OuterXml);

                Assert.IsTrue(validert, envelopeValidator.ValidationWarnings);
            }

            [TestMethod]
            public void LagUgyldigSecurityNodeXsdValidererIkke()
            {
                var envelope = DomainUtility.GetForretningsmeldingEnvelopeWithTestTestCertificate();
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

                var securityNode = forretningsmeldingEnvelopeXml.DocumentElement.SelectSingleNode("//wsse:Security",
                    namespaceManager);

                var gammelVerdi = securityNode.Attributes["mustUnderstand"].Value;
                securityNode.Attributes["mustUnderstand"].Value = "en_tekst_som_ikke_er_bool";

                var validert = envelopeValidator.Validate(forretningsmeldingEnvelopeXml.OuterXml);
                Assert.IsFalse(validert, envelopeValidator.ValidationWarnings);

                securityNode.Attributes["mustUnderstand"].Value = gammelVerdi;
            }
        }
    }
}