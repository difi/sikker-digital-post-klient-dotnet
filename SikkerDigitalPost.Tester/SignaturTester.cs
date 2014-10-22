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

        [TestMethod]
        public void HoveddokumentStarterMedEtTallXsdValidererIkke()
        {
            var signaturXml = Arkiv.Signatur.Xml();
            var signaturvalidator = new Signaturvalidator();
            var validerer = signaturvalidator.ValiderDokumentMotXsd(signaturXml.OuterXml);

            //Endre id på hoveddokument til å starte på et tall
            var namespaceManager = new XmlNamespaceManager(signaturXml.NameTable);
            namespaceManager.AddNamespace("ds", Navnerom.ds);
            namespaceManager.AddNamespace("ns10", Navnerom.Ns10);
            namespaceManager.AddNamespace("ns11", Navnerom.Ns11);

            var hoveddokumentReferanseNode = signaturXml.DocumentElement
                .SelectSingleNode("//ds:Reference[@Id = '" + Hoveddokument.Id + "']", namespaceManager);

            var gammelVerdi = hoveddokumentReferanseNode.Attributes["Id"].Value;
            hoveddokumentReferanseNode.Attributes["Id"].Value = "0_Id_Som_Skal_Feile";

            validerer = signaturvalidator.ValiderDokumentMotXsd(signaturXml.OuterXml);
            Assert.IsFalse(validerer, signaturvalidator.ValideringsVarsler);

            hoveddokumentReferanseNode.Attributes["Id"].Value = gammelVerdi;
        }

        [TestMethod]
        public void ValidereSignaturMotXsdValiderer()
        {
            var signaturXml = Arkiv.Signatur.Xml();
            var signaturValidering = new Signaturvalidator();
            var validerer = signaturValidering.ValiderDokumentMotXsd(signaturXml.OuterXml);

            Assert.IsTrue(validerer, signaturValidering.ValideringsVarsler);
        }
    }
}
