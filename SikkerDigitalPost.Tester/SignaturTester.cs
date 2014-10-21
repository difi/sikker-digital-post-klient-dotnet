using System;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SikkerDigitalPost.Klient;
using SikkerDigitalPost.Klient.XmlValidering;

namespace SikkerDigitalPost.Tester
{
    [TestClass]
    public class SignaturTester : TestBase
    {
        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            Initialiser();
        }

        private static bool _harFeilet;

        [TestMethod]
        public void ValidereSignaturMotXsdValiderer()
        {
            var signaturXml = Signatur.Xml();
            var signaturValidering = new SignaturValidering();

            var validerer = signaturValidering.ValiderDokumentMotXsd(signaturXml.OuterXml);

            Assert.IsTrue(validerer);

            //Endre id på hoveddokument til å starte på et tall
            var namespaceManager = new XmlNamespaceManager(signaturXml.NameTable);
            namespaceManager.AddNamespace("ds", Navnerom.ds);
            namespaceManager.AddNamespace("ns10", Navnerom.Ns10);
            namespaceManager.AddNamespace("ns11", Navnerom.Ns11);

            var hoveddokumentReferanseNode = signaturXml.DocumentElement.SelectSingleNode("//ds:Reference[@Id = 'Id_0']",
                namespaceManager);
            hoveddokumentReferanseNode.Attributes["Id"].Value = "0_Id_Som_Skal_Feile";

            validerer = signaturValidering.ValiderDokumentMotXsd(signaturXml.OuterXml);

            Assert.IsFalse(validerer);
        }
    }
}
