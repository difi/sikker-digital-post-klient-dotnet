using System.Reflection;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Xml;
using ApiClientShared;
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

            [TestMethod] public void FeilerMedPrivatnøkkelSomIkkeErRsaIKKEIMPLEMENTERT()
            {
                //Denne testen er ikke skrevet fordi vi ikke har klart å lage et sertifikat som bruker
                //DSACryptoProvider. Script `GenererSertifikatScripts.txt` inneholder info om hvordan.
                //Feilmelding `bad data` kommer, så noe er galt. Jeg mener det likevel er viktig å påpeke
                //at vi bør ha testdekning på dette. Kanskje kan fremtiden løse dette problemet?
                // Aleksander 02.11.2015

                //////////////////////////////////////////////
                //P12 container med privatekey og sertifikat som holder i 10 år, DSA kryptering
                //////////////////////////////////////////////

                //Lag dsa-parametere
                // openssl dsaparam -out dsap.pem 2048

                //Lag privatnøkkel og sertifikat
                //openssl req -x509 - newkey dsa: dsap.pem - keyout key.pem -out certificate.pem - days 3650 - nodes - subj "/C=NO/ST=Oslo/L=Posthuset/O=Digipost testsertifikat Name/OU=Org/CN=www.digiphoest.no"

                //Pakk inn i container
                //openssl pkcs12 -export -out certificate.pfx - inkey key.pem -in certificate.pem
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