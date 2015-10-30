using System.Reflection;
using System.Security.Cryptography;
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
            [TestMethod]
            public void KonstruktørMedXmlDokumentOgSertifikat()
            {
                //Arrange
                var xmlDokument = XmlUtility.TilXmlDokument(TransportKvittering.TransportOkKvittertingFunksjoneltTestmiljø);
                var sertifikat = DomeneUtility.GetAvsenderEnhetstesterSertifikat();
                var signedXmlWithAgnosticId = new SignedXmlWithAgnosticId(xmlDokument, sertifikat);

                //Act
                var signingKey = signedXmlWithAgnosticId.SigningKey;

                //Assert
                Assert.IsTrue(signingKey is RSACryptoServiceProvider);
            }
            
            [TestMethod]
            [ExpectedException(typeof(SdpSecurityException))]
            public void FeilerMedSertifikatUtenPrivatnøkkel()
            {
                //Arrange
                var xmlDokument = XmlUtility.TilXmlDokument(TransportKvittering.TransportOkKvittertingFunksjoneltTestmiljø);
                var sertifikat = DomeneUtility.GetMottakerEnhetstesterSertifikat();

                //Act
                new SignedXmlWithAgnosticId(xmlDokument, sertifikat);
            }

            [TestMethod] public void FeilerMedPrivatnøkkelSomIkkeErRsa()
            {
                //Arrange
                var xmlDokument = XmlUtility.TilXmlDokument(TransportKvittering.TransportOkKvittertingFunksjoneltTestmiljø);

                //Act

                //Assert
                Assert.Fail();
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

            private void LeggHeaderSignaturNodeTilSignedXmlWithAgnosticId(XmlDocument kildeXmlDokument, SignedXmlWithAgnosticId signedXmlWithAgnosticId)
            {
                var headerSignaturNode = (XmlElement) kildeXmlDokument.DocumentElement.SelectSingleNode("/env:Envelope/env:Header/wsse:Security/ds:Signature", 
                    GetNamespaceManager(kildeXmlDokument));
                signedXmlWithAgnosticId.LoadXml(headerSignaturNode);
            }

            private void LeggBodySignaturNodeTilSignedXmlWithAgnosticId(XmlDocument kildeXmlDokument, SignedXmlWithAgnosticId signedXmlWithAgnosticId)
            {
                var standardBusinessDocumentNode = (XmlElement) kildeXmlDokument.SelectSingleNode("//ds:Signature", GetNamespaceManager(kildeXmlDokument));
                signedXmlWithAgnosticId.LoadXml(standardBusinessDocumentNode);
            }

            private object GetPublicKey(SignedXmlWithAgnosticId signedXmlWithAgnosticId)
            {
                return typeof(SignedXmlWithAgnosticId).GetMethod("GetPublicKey", BindingFlags.Instance | BindingFlags.NonPublic)
                    .Invoke(signedXmlWithAgnosticId, null);
            }

            [TestMethod]
            public void HenterKeyFraTransportkvittering()
            {
                //Arrange
                var xmlDokument = XmlUtility.TilXmlDokument(TransportKvittering.TransportOkKvittertingFunksjoneltTestmiljø);
                SignedXmlWithAgnosticId signedXmlWithAgnosticId = new SignedXmlWithAgnosticId(xmlDokument);

                LeggHeaderSignaturNodeTilSignedXmlWithAgnosticId(xmlDokument, signedXmlWithAgnosticId);

                //Act
                var signingKey = GetPublicKey(signedXmlWithAgnosticId);
                var signingKey2 = GetPublicKey(signedXmlWithAgnosticId);

                //Assert
                Assert.IsNotNull(signingKey);
                Assert.IsNull(signingKey2);
            }

            [TestMethod]
            public void HenterKeyFraMeldingskvitteringHeader()
            {
                //Arrange
                var xmlDokument = XmlUtility.TilXmlDokument(KvitteringsRespons.FunksjoneltTestmiljø);
                SignedXmlWithAgnosticId signedXmlWithAgnosticId = new SignedXmlWithAgnosticId(xmlDokument);

                LeggHeaderSignaturNodeTilSignedXmlWithAgnosticId(xmlDokument, signedXmlWithAgnosticId);

                //Act
                var signingKey = GetPublicKey(signedXmlWithAgnosticId);
                var signingKey2 = GetPublicKey(signedXmlWithAgnosticId);

                //Assert
                Assert.IsNotNull(signingKey);
                Assert.IsNull(signingKey2);
            }
            
            [TestMethod]
            public void HenterKeyFraMeldingskvitteringBody()
            {
                //Arrange
                var xmlDokument = XmlUtility.TilXmlDokument(KvitteringsRespons.FunksjoneltTestmiljø);
                SignedXmlWithAgnosticId signedXmlWithAgnosticId = new SignedXmlWithAgnosticId(xmlDokument);

                LeggBodySignaturNodeTilSignedXmlWithAgnosticId(xmlDokument, signedXmlWithAgnosticId);

                //Act
                var signingKey = GetPublicKey(signedXmlWithAgnosticId);
                var signingKey2 = GetPublicKey(signedXmlWithAgnosticId);

                //Assert
                Assert.IsNotNull(signingKey);
                Assert.IsNull(signingKey2);
            }
        }

    }
}