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
            var envelopeValiderer = new ForretningsmeldingEnvelopeValidering();
            var validert = envelopeValiderer.ValiderDokumentMotXsd(forretningsmeldingEnvelopeXml.OuterXml);

            Assert.IsTrue(validert, envelopeValiderer.ValideringsVarsler);

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
            securityNode.Attributes["mustUnderstand"].Value = "en_tekst_som_ikke_er_bool";

            validert = envelopeValiderer.ValiderDokumentMotXsd(forretningsmeldingEnvelopeXml.OuterXml);

            Assert.IsFalse(validert, envelopeValiderer.ValideringsVarsler);
        }
    }
}
