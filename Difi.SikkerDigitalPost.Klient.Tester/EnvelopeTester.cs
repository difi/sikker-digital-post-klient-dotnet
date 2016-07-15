using System.Xml;
using Difi.SikkerDigitalPost.Klient.Tester.Utilities;
using Difi.SikkerDigitalPost.Klient.Utilities;
using Difi.SikkerDigitalPost.Klient.XmlValidering;
using Xunit;

namespace Difi.SikkerDigitalPost.Klient.Tester
{
    public class EnvelopeTester
    {
        public class XsdValidering : EnvelopeTester
        {
            [Fact]
            public void ValidereEnvelopeMotXsdValiderer()
            {
                var envelope = DomainUtility.GetForretningsmeldingEnvelopeWithTestTestCertificate();
                var forretningsmeldingEnvelopeXml = envelope.Xml();

                string validationMessages;
                var validert = SdpXmlValidator.Instance.Validate(forretningsmeldingEnvelopeXml.OuterXml, out validationMessages);

                Assert.True(validert, validationMessages);
            }

            [Fact]
            public void LagUgyldigSecurityNodeXsdValidererIkke()
            {
                var envelope = DomainUtility.GetForretningsmeldingEnvelopeWithTestTestCertificate();
                var forretningsmeldingEnvelopeXml = envelope.Xml();

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
                var validert = SdpXmlValidator.Instance.Validate(forretningsmeldingEnvelopeXml.OuterXml, out validationMessages);
                Assert.False(validert, validationMessages);

                securityNode.Attributes["mustUnderstand"].Value = gammelVerdi;
            }
        }
    }
}