using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Xml;
using Difi.SikkerDigitalPost.Klient.Domene.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Difi.SikkerDigitalPost.Klient.Tester.testdata.meldinger;
using Difi.SikkerDigitalPost.Klient.Tester.Utilities;
using Difi.SikkerDigitalPost.Klient.Utilities;

namespace Difi.SikkerDigitalPost.Klient.Security.Tests
{
    [TestClass()]
    public class SignedXmlWithAgnosticIdTester
    {
        [TestClass]
        public class KonstruktørMethod : SignedXmlWithAgnosticIdTester
        {
            Comparator _comparator = new Comparator();   

            [TestMethod]
            public void KonstruktørMedXmlDokumentOgSertifikat()
            {
                //Arrange
                var xmlDokument = XmlUtility.TilXmlDokument(TransportKvittering.TransportOkKvittertingFunksjoneltTestmiljø);
                var sertifikat = DomeneUtility.GetAvsenderSertifikat();

                //Act
                var signedXmlWithAgnosticId = new SignedXmlWithAgnosticId(xmlDokument, sertifikat);
                var signingKey = typeof (SignedXmlWithAgnosticId).GetProperty("SigningKey", BindingFlags.Instance | BindingFlags.NonPublic)
                    .GetValue(signedXmlWithAgnosticId, null);

                //Assert
                Assert.IsTrue(signingKey is RSACryptoServiceProvider);
            }

            [TestMethod]
            [ExpectedException(typeof(SdpSecurityException))]
            public void FeilerMedSertifikatUtenPrivatnøkkel()
            {
                //Arrange
                var xmlDokument = XmlUtility.TilXmlDokument(TransportKvittering.TransportOkKvittertingFunksjoneltTestmiljø);
                var sertifikat = DomeneUtility.GetMottakerSertifikat();

                //Act
                var signedXmlWithAgnosticId = new SignedXmlWithAgnosticId(xmlDokument, sertifikat);
            }

            [TestMethod] public void FeilerMedPrivatnøkkelSomIkkeErRsa()
            {
                //Arrange
                

                //Act

                //Assert
                Assert.Fail();
            }
        }

        [TestClass]
        public class SignedXmlWithAgnosticIdMethod : SignedXmlWithAgnosticIdTester
        {
         
        }

        [TestClass]
        public class FindIdElementMethod : SignedXmlWithAgnosticIdTester
        {
            [TestMethod]
            public void FinnerIdElementVAFFORNO1()
            {
                //Arrange
                

                //Act

                //Assert
            }

            [TestMethod]
            public void FinnerIdElementVAFFORNO2()
            {
                //Arrange


                //Act

                //Assert
            }

            [TestMethod]
            public void FinnerIdElementVAFFORNO3()
            {
                //Arrange


                //Act

                //Assert
            }

        }

        [TestClass]
        public class GetPublicKeyMethod : SignedXmlWithAgnosticIdTester
        {
            private XmlNamespaceManager GetNamespaceManager(XmlDocument forDocument)
            {
                var xmlNamespaceManager = new XmlNamespaceManager(forDocument.NameTable);
                xmlNamespaceManager.AddNamespace("env", NavneromUtility.SoapEnvelopeEnv12);
                xmlNamespaceManager.AddNamespace("wsse", NavneromUtility.WssecuritySecext10);
                xmlNamespaceManager.AddNamespace("ds", NavneromUtility.XmlDsig);
                xmlNamespaceManager.AddNamespace("eb", NavneromUtility.EbXmlCore);
                xmlNamespaceManager.AddNamespace("wsu", NavneromUtility.WssecurityUtility10);
                xmlNamespaceManager.AddNamespace("ebbp", NavneromUtility.EbppSignals);
                xmlNamespaceManager.AddNamespace("sbd", NavneromUtility.StandardBusinessDocumentHeader);
                xmlNamespaceManager.AddNamespace("difi", NavneromUtility.DifiSdpSchema10);

                return xmlNamespaceManager;
            }

            [TestMethod]
            public void ValidererTransportkvitteringKorrekt()
            {
                //Arrange
                var xmlDokument = XmlUtility.TilXmlDokument(TransportKvittering.TransportOkKvittertingFunksjoneltTestmiljø);
                SignedXmlWithAgnosticId signedXmlWithAgnosticId = new SignedXmlWithAgnosticId(xmlDokument);

                var headerSignaturNode = (XmlElement) xmlDokument.DocumentElement.SelectSingleNode("/env:Envelope/env:Header/wsse:Security/ds:Signature", GetNamespaceManager(xmlDokument));
                signedXmlWithAgnosticId.LoadXml(headerSignaturNode);

                //Act
                var signingKey = typeof(SignedXmlWithAgnosticId).GetMethod("GetPublicKey", BindingFlags.Instance | BindingFlags.NonPublic)
                   .Invoke(signedXmlWithAgnosticId, null);

                //Assert
                Assert.IsNotNull(signingKey);
            }

            [TestMethod]
            public void ValidererMeldingskvitteringKorrekt()
            {
                //Arrange


                //Act

                //Assert
            }

            [TestMethod]
            public void FeilerForUgyldigTransportkvittering()
            {
                //Arrange


                //Act

                //Assert
            }

            [TestMethod]
            public void FeilerForUgyldingMeldingskvittering()
            {
                //Arrange


                //Act

                //Assert
            }

            [TestMethod]
            public void ReturnererIngenSertifikatVedFeilXml()
            {
                //Arrange
                

                //Act

                //Assert
            }

            [TestMethod]
            public void ReturererSertifikatVedKorrektXml()
            {
                //Arrange
                

                //Act

                //Assert
            }
        }

    }
}