using System.Security.Cryptography;
using System.Xml;
using Difi.SikkerDigitalPost.Klient.Domene.Exceptions;
using Difi.SikkerDigitalPost.Klient.Security;
using Difi.SikkerDigitalPost.Klient.Tester.testdata.meldinger;
using Difi.SikkerDigitalPost.Klient.Tester.Utilities;
using Difi.SikkerDigitalPost.Klient.Utilities;
using Difi.SikkerDigitalPost.Klient.XmlValidering;
using Xunit;

namespace Difi.SikkerDigitalPost.Klient.Tester.XmlValidering
{
    public class ResponseValidatorTests
    {
        private static void AddRsaSha256AlgorithmToCryptoConfig()
        {
            const string signatureMethod = "http://www.w3.org/2001/04/xmldsig-more#rsa-sha256";

            if (CryptoConfig.CreateFromName(signatureMethod) == null)
                CryptoConfig.AddAlgorithm(typeof (RsaPkCs1Sha256SignatureDescription), signatureMethod);
        }

        public class ConstructorMethod : ResponseValidatorTests
        {
            [Fact]
            public void InitializesWithProperties()
            {
                //Arrange
                var sentMessage = XmlUtility.TilXmlDokument(SendtMelding.FunksjoneltTestMiljø);
                var response = XmlUtility.TilXmlDokument(TransportKvittering.TransportOkKvittertingFunksjoneltTestmiljø);
                var miljø = Miljø.FunksjoneltTestmiljø;
                var certificateValidationProperties = new CertificateValidationProperties(miljø.GodkjenteKjedeSertifikater, DomainUtility.OrganisasjonsnummerMeldingsformidler());

                var responseValidator = new ResponseValidator(sentMessage, response, certificateValidationProperties);

                //Act

                //Assert
                Assert.Equal(sentMessage, responseValidator.SentMessage);
                Assert.Equal(response, responseValidator.ResponseMessage);
            }
        }

        public class ValiderTransportkvitteringMethod : ResponseValidatorTests
        {
            [Fact(Skip = "Hardkodet xml, må regenerere")]
            public void TestsertifikatValiderer()
            {
                //Arrange
                AddRsaSha256AlgorithmToCryptoConfig();

                var miljø = Miljø.FunksjoneltTestmiljø;
                var sentMessage = XmlUtility.TilXmlDokument(SendtMelding.FunksjoneltTestMiljø);
                var certificateValidationProperties = new CertificateValidationProperties(miljø.GodkjenteKjedeSertifikater, DomainUtility.OrganisasjonsnummerMeldingsformidler());
                var responseValidator = new ResponseValidator(sentMessage, XmlUtility.TilXmlDokument(TransportKvittering.TransportOkKvittertingFunksjoneltTestmiljø), certificateValidationProperties);
                var guidUtility = new GuidUtility
                {
                    BinarySecurityTokenId = "X509-368aad63-6d28-44e1-864a-a7b31908c67a",
                    BodyId = "soapBody",
                    DokumentpakkeId = "74b77e2b-f409-4954-a2df-fdfa462d2339@meldingsformidler.sdp.difi.no",
                    EbMessagingId = "id-68ae7123-bf5c-4d15-835c-4a6b91106e77",
                    MessageId = "388214db-29cc-43c7-9543-877e017e5bb4",
                    TimestampId = "TS-76740c34-88d2-4bb6-82d2-9e9f0e474087"
                };

                //Act
                responseValidator.ValidateTransportReceipt(guidUtility);

                //Assert
            }

            [Fact]
            public void FeilDokumentpakkeIdITransportkvitteringSkalKasteSecurityException()
            {
                //Arrange
                const string idRequest = "4fa27c07-8a0f-45a9-954e-c658f6c480af@meldingsformidler.sdp.difi.no";
                const string idResponse = "hei27c07-8a0f-45a9-954e-c658f6c480af@meldingsformidler.sdp.difi.no";
                AddRsaSha256AlgorithmToCryptoConfig();

                var miljø = Miljø.FunksjoneltTestmiljø;
                var sentMessage = XmlUtility.TilXmlDokument(SendtMelding.FunksjoneltTestMiljøMedInput());

                var transportReceipt = XmlUtility.TilXmlDokument(TransportKvittering.TransportOkKvittertingFunksjoneltTestmiljøMedInput(idResponse));

                var certificateValidationProperties = new CertificateValidationProperties(miljø.GodkjenteKjedeSertifikater, DomainUtility.OrganisasjonsnummerMeldingsformidler());
                var responseValidator = new ResponseValidator(sentMessage, transportReceipt, certificateValidationProperties);

                var guidUtility = new GuidUtility
                {
                    BinarySecurityTokenId = "X509-513ffecb-cd7e-4bb3-a4c5-47eff314683f",
                    BodyId = "soapBody",
                    DokumentpakkeId = idRequest,
                    EbMessagingId = "id-68ae7123-bf5c-4d15-835c-4a6b91106e77",
                    MessageId = "388214db-29cc-43c7-9543-877e017e5bb4",
                    TimestampId = "TS-76740c34-88d2-4bb6-82d2-9e9f0e474087"
                };

                //Act
                Assert.Throws<SecurityException>(() => responseValidator.ValidateTransportReceipt(guidUtility));
            }

            [Fact]
            public void FeilSecurityBinaryITransportKvitteringSkalKasteException()
            {
                //Arrange
                AddRsaSha256AlgorithmToCryptoConfig();

                //h at the end is replaced with a b
                const string corruptSecurityBinaryResponse = "MIIE7jCCA9agAwIBAgIKGBj1bv99Jpi+EzANBgkqhkiG9w0BAQsFADBRMQswCQYDVQQGEwJOTzEdMBsGA1UECgwUQnV5cGFzcyBBUy05ODMxNjMzMjcxIzAhBgNVBAMMGkJ1eXBhc3MgQ2xhc3MgMyBUZXN0NCBDQSAzMB4XDTE0MDQyNDEyMzExMVoXDTE3MDQyNDIxNTkwMFowVTELMAkGA1UEBhMCTk8xGDAWBgNVBAoMD1BPU1RFTiBOT1JHRSBBUzEYMBYGA1UEAwwPUE9TVEVOIE5PUkdFIEFTMRIwEAYDVQQFEwk5ODQ2NjExODUwggEiMA0GCSqGSIb3DQEBAQUAA4IBDwAwggEKAoIBAQDLTnQryf2bmiyQ9q3ylQ6xMl7EhGIbjuziXkRTfL+M94m3ceAiko+r2piefKCiquLMK4j+UDcOapUtLC4dT4c6GhRH4FIOEn5aNS2I/njTenBypWka/VEhQUj7zvIh5G4UXIDIXYvLd7gideeMtkX24KUh2XVlh+PcqLGHirqBwVfFiTn5SKhr/ojhYYEb2xxTk3AY9nLd1MMffKQwUWmfoTos4scREYGI2R2vWxKWPcDqk+jig2DISWSJSuerz3HMYAAmp+Gjt0oFJNiyOFaFyGwT3DvqwOMQWwWXdmLh1NxMgTpghXAaXae76ucm9GDQ9E7ytf+JA096RWoi+5GtAgMBAAGjggHCMIIBvjAJBgNVHRMEAjAAMB8GA1UdIwQYMBaAFD+u9XgLkqNwIDVfWvr3JKBSAfBBMB0GA1UdDgQWBBTVyVLqcjWf1Qd0gsmCTrhXiWeqVDAOBgNVHQ8BAf8EBAMCBLAwFgYDVR0gBA8wDTALBglghEIBGgEAAwIwgbsGA1UdHwSBszCBsDA3oDWgM4YxaHR0cDovL2NybC50ZXN0NC5idXlwYXNzLm5vL2NybC9CUENsYXNzM1Q0Q0EzLmNybDB1oHOgcYZvbGRhcDovL2xkYXAudGVzdDQuYnV5cGFzcy5uby9kYz1CdXlwYXNzLGRjPU5PLENOPUJ1eXBhc3MlMjBDbGFzcyUyMDMlMjBUZXN0NCUyMENBJTIwMz9jZXJ0aWZpY2F0ZVJldm9jYXRpb25MaXN0MIGKBggrBgEFBQcBAQR+MHwwOwYIKwYBBQUHMAGGL2h0dHA6Ly9vY3NwLnRlc3Q0LmJ1eXBhc3Mubm8vb2NzcC9CUENsYXNzM1Q0Q0EzMD0GCCsGAQUFBzAChjFodHRwOi8vY3J0LnRlc3Q0LmJ1eXBhc3Mubm8vY3J0L0JQQ2xhc3MzVDRDQTMuY2VyMA0GCSqGSIb3DQEBCwUAA4IBAQCmMpAGaNplOgx3b4Qq6FLEcpnMOnPlSWBC7pQEDWx6OtNUHDm56fBoyVQYKR6LuGfalnnOKuB/sGSmO3eYlh7uDK9WA7bsNU/W8ZiwYwF6PBRui2rrqYk3kj4NLTNlyh/AOO1a2FDFHu369W0zcjj5ns7qs0K3peXtLX8pVxA8RmjwdGe69P/2r6s2A5CBj7oXZJD0Yo2dJFdsZzonT900sUi+MWzlhj3LxU5/684NWc2NI6ZPof/dyYpy3K/AFzpDLWGSmaDO66hPl7EfoJxEiX0DNBaQzNIyRFPh0ir0jM+32ZQ4goR8bAtyhKeTyA/4+Qx1WQXS3wURCC0lsbMb";

                var miljø = Miljø.FunksjoneltTestmiljø;
                var sendtMeldingXmlDocument = XmlUtility.TilXmlDokument(SendtMelding.FunksjoneltTestMiljøMedInput());

                var receivedTransportReceipt = XmlUtility.TilXmlDokument(TransportKvittering.TransportOkKvittertingFunksjoneltTestmiljøMedInput(securityBinary: corruptSecurityBinaryResponse));

                var certificateValidationProperties = new CertificateValidationProperties(miljø.GodkjenteKjedeSertifikater, DomainUtility.OrganisasjonsnummerMeldingsformidler());
                var responseValidator = new ResponseValidator(sendtMeldingXmlDocument, receivedTransportReceipt,certificateValidationProperties);
                var guidUtility = new GuidUtility
                {
                    BinarySecurityTokenId = "X509-513ffecb-cd7e-4bb3-a4c5-47eff314683f",
                    BodyId = "soapBody",
                    DokumentpakkeId = "4fa27c07-8a0f-45a9-954e-c658f6c480af@meldingsformidler.sdp.difi.no",
                    EbMessagingId = "id-68ae7123-bf5c-4d15-835c-4a6b91106e77",
                    MessageId = "388214db-29cc-43c7-9543-877e017e5bb4",
                    TimestampId = "TS-76740c34-88d2-4bb6-82d2-9e9f0e474087"
                };

                //Act
                Assert.Throws<SecurityException>(() => responseValidator.ValidateTransportReceipt(guidUtility));
            }

            [Fact]
            public void IncorrectTransportReceiptThrowsSecurityException()
            {
                //Arrange
                AddRsaSha256AlgorithmToCryptoConfig();

                var miljø = Miljø.FunksjoneltTestmiljø;
                var sentMessage = XmlUtility.TilXmlDokument(SendtMelding.FunksjoneltTestMiljø);

                var receivedTransportReceipt = XmlUtility.TilXmlDokument(TransportKvittering.TransportOkKvitteringMedByttetDokumentpakkeIdFunksjoneltTestmiljø);

                var certificateValidationProperties = new CertificateValidationProperties(miljø.GodkjenteKjedeSertifikater, DomainUtility.OrganisasjonsnummerMeldingsformidler());
                var responseValidator = new ResponseValidator(sentMessage, receivedTransportReceipt,certificateValidationProperties);

                var guidUtility = new GuidUtility
                {
                    BinarySecurityTokenId = "X509-513ffecb-cd7e-4bb3-a4c5-47eff314683f",
                    BodyId = "soapBody",
                    DokumentpakkeId = "4fa27c07-8a0f-45a9-954e-c658f6c480af@meldingsformidler.sdp.difi.no",
                    EbMessagingId = "id-68ae7123-bf5c-4d15-835c-4a6b91106e77",
                    MessageId = "388214db-29cc-43c7-9543-877e017e5bb4",
                    TimestampId = "TS-76740c34-88d2-4bb6-82d2-9e9f0e474087"
                };

                //Act
                Assert.Throws<SecurityException>(() => responseValidator.ValidateTransportReceipt(guidUtility));
            }
        }

        public class ValidateMessageReceiptMethod : ResponseValidatorTests
        {
            [Fact(Skip = "Hardkodet xml, må regenerere")]
            public void TestCertificateValidates()
            {
                //Arrange
                AddRsaSha256AlgorithmToCryptoConfig();
                var miljø = Miljø.FunksjoneltTestmiljø;

                var sentReceiptRequest = new XmlDocument();
                sentReceiptRequest.LoadXml(Kvitteringsforespørsel.FunksjoneltTestmiljø);

                var certificateValidationProperties = new CertificateValidationProperties(miljø.GodkjenteKjedeSertifikater, DomainUtility.OrganisasjonsnummerMeldingsformidler());
                var responseValidator = new ResponseValidator(sentReceiptRequest, XmlUtility.TilXmlDokument(KvitteringsRespons.FunksjoneltTestmiljø), certificateValidationProperties);

                //Act
                responseValidator.ValidateMessageReceipt();

                //Assert
            }
        }

        public class ValidateEmptyQueueReceiptMethod : ResponseValidatorTests
        {
            [Fact(Skip = "Hardkodet xml, må regenerere")]
            public void TestCertificateValidates()
            {
                //Arrange
                AddRsaSha256AlgorithmToCryptoConfig();
                Miljø miljø = Miljø.FunksjoneltTestmiljø;

                var sentReceiptRequest = new XmlDocument();
                sentReceiptRequest.LoadXml(Kvitteringsforespørsel.FunksjoneltTestmiljø);

                var certificateValidationProperties = new CertificateValidationProperties(miljø.GodkjenteKjedeSertifikater, DomainUtility.OrganisasjonsnummerMeldingsformidler());
                var responseValidator = new ResponseValidator(sentReceiptRequest, XmlUtility.TilXmlDokument(KvitteringsRespons.TomKøResponsFunksjoneltTestmiljø), certificateValidationProperties);

                //Act
                responseValidator.ValidateEmptyQueueReceipt();

                //Assert
            }
        }
    }
}