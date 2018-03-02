using System.Xml;
using Difi.SikkerDigitalPost.Klient.Domene.Exceptions;
using Difi.SikkerDigitalPost.Klient.Domene.XmlValidering;
using Difi.SikkerDigitalPost.Klient.Tester.Utilities;
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

                SdpXmlValidator.Validate(forretningsmeldingEnvelopeXml, "Forretningsmelding");
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

                Assert.Throws<XmlValidationException>(() => SdpXmlValidator.Validate(forretningsmeldingEnvelopeXml, "Forretningsmelding"));

                securityNode.Attributes["mustUnderstand"].Value = gammelVerdi;
            }
        }
    }
}