using System;
using System.Linq;
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
        public class FindIdElementMethod : SignedXmlWithAgnosticIdTester
        {
            [TestMethod]
            public void FinnerIdElementUansettSkrivemåte()
            {
                var tests = new[]
                {
                    "<{0} {2}='{1}'></{0}>",
                    "<{0} {2}='{1}'></{0}>",
                    "<{0} {2}='{1}'></{0}>",
                    "<container><{0} {2}='{1}'></{0}></container>",
                    "<container><invalid Id='notThis' /><{0} {2}='{1}'></{0}></container>",
                    "<container><invalid ID='notThis'><{0} {2}='{1}'></{0}></invalid></container>",
                    "<container xmlns='http://example.org'><{0} {2}='{1}'></{0}></container>",
                    "<a:container xmlns:a='http://example.org'><{0} {2}='{1}'></{0}></a:container>",
                    "<a:container xmlns:a='http://example.org'><a:{0} {2}='{1}'></a:{0}></a:container>",
                    "<a:container xmlns:a='http://example.org'><b:{0} xmlns:b='http://nowhere.com' {2}='{1}'></b:{0}></a:container>",
                    "<a:container xmlns:a='http://example.org'><{0} xmlns='' {2}='{1}'></{0}></a:container>",
                    "<a:container xmlns:a='http://example.org'><{0} xmlns='' a:{2}='{1}'></{0}></a:container>"
                };

                foreach (var item in tests)
                {
                    foreach (var id in new string[] { "Id", "ID", "id" })
                    {
                        var xml = new XmlDocument() { PreserveWhitespace = true };
                        xml.LoadXml(string.Format(item, "element", "value", id));

                        var signed = new SignedXmlWithAgnosticId(xml);
                        var response = signed.GetIdElement(xml, "value");

                        Assert.IsNotNull(response);
                        Assert.IsTrue(
                            response.Attributes.OfType<XmlAttribute>().Any(a => a.LocalName == id && a.Value == "value"));
                    }
                }
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
                var signedXmlWithAgnosticId = new SignedXmlWithAgnosticId(xmlDokument);

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
                var signedXmlWithAgnosticId = new SignedXmlWithAgnosticId(xmlDokument);

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
                var signedXmlWithAgnosticId = new SignedXmlWithAgnosticId(xmlDokument);

                LeggBodySignaturNodeTilSignedXmlWithAgnosticId(xmlDokument, signedXmlWithAgnosticId);

                //Act
                var signingKey = GetPublicKey(signedXmlWithAgnosticId);
                var signingKey2 = GetPublicKey(signedXmlWithAgnosticId);

                //Assert
                Assert.IsNotNull(signingKey);
                Assert.IsNull(signingKey2);
            }

            [TestMethod]
            public void SignaturnodeOgBinarySecurityTokenErLike()
            {
                //Arrange
                var doc = new XmlDocument { PreserveWhitespace = false };
                var ResponeKvitteringMOttattForretningsmelding = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><env:Envelope xmlns:env=\"http://www.w3.org/2003/05/soap-envelope\">\r\n  <env:Header>\r\n    <wsse:Security xmlns:wsse=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd\" xmlns:wsu=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd\" env:mustUnderstand=\"true\">\r\n      <wsse:BinarySecurityToken EncodingType=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#Base64Binary\" ValueType=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-x509-token-profile-1.0#X509v3\" wsu:Id=\"X509-ecd9521a-6429-4c94-a23f-07157e36f963\">MIIC+zCCAeOgAwIBAgIEqrcM8zANBgkqhkiG9w0BAQsFADAnMSUwIwYDVQQDDBxEaWdpcG9zdCBUZXN0IENlcnQgQXV0aG9yaXR5MB4XDTE0MTAxMjEzMTI1NloXDTE1MDQxMzEzMTI1NlowFDESMBAGA1UEAwwJMTIzNDU2Nzg5MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAg8Y9dCm1sUitJdULB5RWzMwLW/CD42c6dzaa8AQzjXZr9xS7nNbnhty0y+srjk+SFu0llz5sRi7Gk8CHHrkY19/KmSLpa72f7G+mHNirYDgCZtTDdRbRWCw6NUJG26b2+rgDEWuHG9CNFmM9tuB6MrJ4yaVjzkJD26f6CWzC61awaguEJWzsue7Nu5GgAYbaaeUFni+EA2tTXXOtFqVPIcIOeGf7+jBR1bEIYWgr28O9nN2FT94GkVolnzf9j9ZfRFjfUkILD8piiCwJ6pZZ/0JPwsZgzYBv9WY4B2U4DupTnce15X4mqW3ixrOcYKMz2J6vvbuKnBdsV9hA7YC37QIDAQABo0IwQDAdBgNVHQ4EFgQUSNSjW1cox5Qh4QVfWNpqyqWA5PswHwYDVR0jBBgwFoAUqtY9y7k1XJVmr6GarUefSa0TznowDQYJKoZIhvcNAQELBQADggEBACenYWQeztec0CdEOzLRHbxx1FuFd9FNgHhDANtv7spsAepdUmD9fK0zA3Db2NctJV9BZx44FgqtRFjQ87B9xtkkDaId5z2Tw9ftGGxq9LiJAkeCgSdPbdkqMn8pc1BUrmaGu4hkuI+5vwwHTFZ4ZvI/ae5vYeECV0LsgAFi0beZ6JOgplvGirtgn484rBrtIodiTxDAMeHJDfTeYaatPKVMmuWYC3atmmjszoXsTp1F6jL+3SxB64i37XYwWgwSB1oC0kpIVBRSX4XKeBTw2x0z8pSM5Ud8yjpUd3tDACzw1FTFcjcsqpkPSNJVt2+zSdVspuEtyKQqh4seDZAy01s=</wsse:BinarySecurityToken>\r\n      <wsu:Timestamp wsu:Id=\"TS-35be0956-ad1a-4b41-a650-2b1a005e9de9\">\r\n        <wsu:Created>2014-10-13T13:12:59.849Z</wsu:Created>\r\n        <wsu:Expires>2014-10-13T13:17:59.849Z</wsu:Expires>\r\n      </wsu:Timestamp>\r\n      <ds:Signature xmlns:ds=\"http://www.w3.org/2000/09/xmldsig#\" Id=\"SIG-bac0f5a4-4594-4f42-bab1-5acfa1ff2a4b\">\r\n        <ds:SignedInfo>\r\n          <ds:CanonicalizationMethod Algorithm=\"http://www.w3.org/2001/10/xml-exc-c14n#\">\r\n            <ec:InclusiveNamespaces xmlns:ec=\"http://www.w3.org/2001/10/xml-exc-c14n#\" PrefixList=\"env\"/>\r\n          </ds:CanonicalizationMethod>\r\n          <ds:SignatureMethod Algorithm=\"http://www.w3.org/2001/04/xmldsig-more#rsa-sha256\"/>\r\n          <ds:Reference URI=\"#id-cfc59a25-a972-4ecc-b8c2-d568bfdeba8f\">\r\n            <ds:Transforms>\r\n              <ds:Transform Algorithm=\"http://www.w3.org/2001/10/xml-exc-c14n#\">\r\n                <ec:InclusiveNamespaces xmlns:ec=\"http://www.w3.org/2001/10/xml-exc-c14n#\" PrefixList=\"\"/>\r\n              </ds:Transform>\r\n            </ds:Transforms>\r\n            <ds:DigestMethod Algorithm=\"http://www.w3.org/2001/04/xmlenc#sha256\"/>\r\n            <ds:DigestValue>0r+2LLUhTpNgCnUz2SDAUBPdCMFUSeTWpW4QiAgO15A=</ds:DigestValue>\r\n          </ds:Reference>\r\n          <ds:Reference URI=\"#TS-35be0956-ad1a-4b41-a650-2b1a005e9de9\">\r\n            <ds:Transforms>\r\n              <ds:Transform Algorithm=\"http://www.w3.org/2001/10/xml-exc-c14n#\">\r\n                <ec:InclusiveNamespaces xmlns:ec=\"http://www.w3.org/2001/10/xml-exc-c14n#\" PrefixList=\"wsse env\"/>\r\n              </ds:Transform>\r\n            </ds:Transforms>\r\n            <ds:DigestMethod Algorithm=\"http://www.w3.org/2001/04/xmlenc#sha256\"/>\r\n            <ds:DigestValue>i6Y+T/GDrE4d+7A2nhdgDKZjUBHWLSv1+jIbLLP65P4=</ds:DigestValue>\r\n          </ds:Reference>\r\n          <ds:Reference URI=\"#id-3b7508d7-942f-45c5-9183-42dfd6fffaf6\">\r\n            <ds:Transforms>\r\n              <ds:Transform Algorithm=\"http://www.w3.org/2001/10/xml-exc-c14n#\">\r\n                <ec:InclusiveNamespaces xmlns:ec=\"http://www.w3.org/2001/10/xml-exc-c14n#\" PrefixList=\"\"/>\r\n              </ds:Transform>\r\n            </ds:Transforms>\r\n            <ds:DigestMethod Algorithm=\"http://www.w3.org/2001/04/xmlenc#sha256\"/>\r\n            <ds:DigestValue>c2KnUatnVYJ38Ebi5OsYDROyfAqPthXZ4QjTWglzgEc=</ds:DigestValue>\r\n          </ds:Reference>\r\n        </ds:SignedInfo>\r\n        <ds:SignatureValue>KJiWpOsWwRxEeoai8GUGoWrHRJcNt3kyvKG6hQMtqNAXjAF9uo3/l2iP8GwwesjrjmOCX0mBwb/l5UlQ3Q7/83AhYar7hysAM/pp7FiMkzae9OgP/g6Oiil/eyIPmkTYAW5JkbRr/stAEUNScmcSSxrGvqTK1wpI5eoGT5EmyBWeGZIpoL2HDp10SeuAQ7beKX0XRqP1uQ0iYjgP7ME0gfi15Xh9QjccmTF6aMZ6GjuD7Cw8G7St3a/UlbJLGLllXBgeYy9lB6Hy61hchrQW/ye35zefwGiBWbQlcEYWrNB7dgB3Tf65uO0H94l956Kw2LT/IByN1rDYOWduHAaNEQ==</ds:SignatureValue>\r\n        <ds:KeyInfo Id=\"KI-87bff0e0-f49c-42e7-900c-f1674148ce3e\">\r\n          <wsse:SecurityTokenReference wsu:Id=\"STR-2f12caa9-60e9-414e-bbb9-ccb5042917b7\">\r\n            <wsse:Reference URI=\"#X509-ecd9521a-6429-4c94-a23f-07157e36f963\" ValueType=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-x509-token-profile-1.0#X509v3\"/>\r\n          </wsse:SecurityTokenReference>\r\n        </ds:KeyInfo>\r\n      </ds:Signature>\r\n    </wsse:Security>\r\n    <eb:Messaging xmlns:eb=\"http://docs.oasis-open.org/ebxml-msg/ebms/v3.0/ns/core/200704/\" xmlns:wsu=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd\" env:mustUnderstand=\"true\" wsu:Id=\"id-3b7508d7-942f-45c5-9183-42dfd6fffaf6\">\r\n      <ns6:SignalMessage xmlns:ns10=\"http://uri.etsi.org/2918/v1.2.1#\" xmlns:ns11=\"http://uri.etsi.org/01903/v1.3.2#\" xmlns:ns2=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:ns3=\"http://www.unece.org/cefact/namespaces/StandardBusinessDocumentHeader\" xmlns:ns4=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:ns5=\"http://www.w3.org/2000/09/xmldsig#\" xmlns:ns6=\"http://docs.oasis-open.org/ebxml-msg/ebms/v3.0/ns/core/200704/\" xmlns:ns7=\"http://docs.oasis-open.org/ebxml-bp/ebbp-signals-2.0\" xmlns:ns8=\"http://www.w3.org/1999/xlink\" xmlns:ns9=\"http://begrep.difi.no/sdp/schema_v10\">\r\n        <ns6:MessageInfo>\r\n          <ns6:Timestamp>2014-10-13T15:12:59.841+02:00</ns6:Timestamp>\r\n          <ns6:MessageId>ef86727d-d10b-499a-b9c3-e6683187951a</ns6:MessageId>\r\n          <ns6:RefToMessageId>627c8082-6394-47a6-9107-a91e52240af2</ns6:RefToMessageId>\r\n        </ns6:MessageInfo>\r\n        <ns6:Receipt>\r\n          <ns7:NonRepudiationInformation>\r\n            <ns7:MessagePartNRInformation>\r\n              <ns5:Reference URI=\"cid:d6f0f811-69c4-4e03-a5a3-5ef02c4dfc11@meldingsformidler.sdp.difi.no\">\r\n                <ns5:Transforms>\r\n                  <ns5:Transform Algorithm=\"http://docs.oasis-open.org/wss/oasis-wss-SwAProfile-1.1#Attachment-Content-Signature-Transform\"/>\r\n                </ns5:Transforms>\r\n                <ns5:DigestMethod Algorithm=\"http://www.w3.org/2001/04/xmlenc#sha256\"/>\r\n                <ns5:DigestValue>kZLCW3NPy62+MtrcKAicYNsOOfkMwgzi5XM/VyYazAw=</ns5:DigestValue>\r\n              </ns5:Reference>\r\n            </ns7:MessagePartNRInformation>\r\n            <ns7:MessagePartNRInformation>\r\n              <ns5:Reference URI=\"#soapBody\">\r\n                <ns5:Transforms>\r\n                  <ns5:Transform Algorithm=\"http://www.w3.org/2001/10/xml-exc-c14n#\">\r\n                    <ec:InclusiveNamespaces xmlns:ds=\"http://www.w3.org/2000/09/xmldsig#\" xmlns:ec=\"http://www.w3.org/2001/10/xml-exc-c14n#\" xmlns:wsse=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd\" PrefixList=\"\"/>\r\n                  </ns5:Transform>\r\n                </ns5:Transforms>\r\n                <ns5:DigestMethod Algorithm=\"http://www.w3.org/2001/04/xmlenc#sha256\"/>\r\n                <ns5:DigestValue>uyFwocFL9AI27C6UvWulZxa1l5gr+NirsPaSXFVIyH0=</ns5:DigestValue>\r\n              </ns5:Reference>\r\n            </ns7:MessagePartNRInformation>\r\n          </ns7:NonRepudiationInformation>\r\n        </ns6:Receipt>\r\n      </ns6:SignalMessage>\r\n    </eb:Messaging>\r\n  </env:Header>\r\n  <env:Body xmlns:wsu=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd\" wsu:Id=\"id-cfc59a25-a972-4ecc-b8c2-d568bfdeba8f\"/>\r\n</env:Envelope>\r\n";
                doc.LoadXml(ResponeKvitteringMOttattForretningsmelding);

                var mgr = new XmlNamespaceManager(doc.NameTable);
                mgr.AddNamespace("wsse", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd");
                mgr.AddNamespace("ds", "http://www.w3.org/2000/09/xmldsig#");

                var signedXmlWithAgnosticId = new SignedXmlWithAgnosticId(doc);
                var signatureNode = (XmlElement)doc.SelectSingleNode("//ds:Signature", mgr);
                signedXmlWithAgnosticId.LoadXml(signatureNode);

                //Act
                var binarySecurityToken = doc.SelectSingleNode("//wsse:BinarySecurityToken", mgr);
                var key = new X509Certificate2(Convert.FromBase64String(binarySecurityToken.InnerText));

                var publicKey = typeof(SignedXmlWithAgnosticId).GetMethod("GetPublicKey", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(signedXmlWithAgnosticId, null) as AsymmetricAlgorithm;

                //Assert
                Assert.AreEqual(publicKey.ToXmlString(false), key.PublicKey.Key.ToXmlString(false));
            }

        }

    }
}