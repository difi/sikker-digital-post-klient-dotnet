using System.Security.Cryptography;
using System.Xml;
using Difi.SikkerDigitalPost.Klient.Domene.Exceptions;
using Difi.SikkerDigitalPost.Klient.Security;
using Difi.SikkerDigitalPost.Klient.Tester.testdata.meldinger;
using Difi.SikkerDigitalPost.Klient.Utilities;
using Difi.SikkerDigitalPost.Klient.XmlValidering;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Difi.SikkerDigitalPost.Klient.Tester.XmlValidering
{
    [TestClass()]
    public class ResponsvalidatorTester
    {
        //Send melding
        private readonly string _sendtMeldingXmlTestMiljø = SendtMelding.FunksjoneltTestMiljø;
        private readonly string _responsTransportkvitteringXmlTestmiljø = TransportKvittering.TransportOkKvittertingFunksjoneltTestmiljø;

        //Hent kvittering
        private readonly string _sendtKvitteringsforespørsel = Kvitteringsforespørsel.FunksjoneltTestmiljø;
        private readonly string _responsKvitteringsForespørsel = KvitteringsRespons.FunksjoneltTestmiljø;
        private readonly string _responsKvitterings = KvitteringsRespons.FunksjoneltTestmiljø;

        private readonly string _tomKøKvittering = KvitteringsRespons.TomKøResponsFunksjoneltTestmiljø;

        private static XmlDocument TilXmlDokument(string xml)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xml);

            return xmlDocument;
        }

        private static void AddRsaSha256AlgorithmToCryptoConfig()
        {
            const string signatureMethod = "http://www.w3.org/2001/04/xmldsig-more#rsa-sha256";

            if (CryptoConfig.CreateFromName(signatureMethod) == null)
                CryptoConfig.AddAlgorithm(typeof(RsaPkCs1Sha256SignatureDescription), signatureMethod);
        }


        [TestClass]
        public class KonstruktørMethod : ResponsvalidatorTester
        {
            [TestMethod]
            public void EnkelKonstruktør()
            {
                ////Arrange
                var sendtMelding = TilXmlDokument(_sendtMeldingXmlTestMiljø);
                var respons = TilXmlDokument(_responsTransportkvitteringXmlTestmiljø);
                var miljø = Miljø.FunksjoneltTestmiljø;
                Responsvalidator responsvalidator = new Responsvalidator(respons, sendtMelding, miljø.Sertifikatvalidator);

                //Act

                //Assert
                Assert.AreEqual(miljø.Sertifikatvalidator, responsvalidator.Sertifikatvalidator);
                Assert.AreEqual(sendtMelding, responsvalidator.SendtMelding);
                Assert.AreEqual(respons, responsvalidator.Respons);
            }
        }

        [TestClass]
        public class ValiderTransportkvitteringMethod : ResponsvalidatorTester
        {
            [TestMethod]
            public void TestsertifikatValiderer()
            {
                //Arrange
                AddRsaSha256AlgorithmToCryptoConfig();

                var dokumentPakkeId = "4fa27c07-8a0f-45a9-954e-c658f6c480af@meldingsformidler.sdp.difi.no";
                var miljø = Miljø.FunksjoneltTestmiljø;
                var sendtMeldingXmlDocument = TilXmlDokument(SendtMelding.FunksjoneltTestMiljøMedInput(dokumentPakkeId));
                var mottattTransportKvittering = TilXmlDokument(_responsTransportkvitteringXmlTestmiljø);

                Responsvalidator responsvalidator = new Responsvalidator(mottattTransportKvittering, sendtMeldingXmlDocument, miljø.Sertifikatvalidator);
                GuidUtility guidUtility = new GuidUtility()
                {
                    BinarySecurityTokenId = "X509-513ffecb-cd7e-4bb3-a4c5-47eff314683f",
                    BodyId = "soapBody",
                    DokumentpakkeId = dokumentPakkeId,
                    EbMessagingId = "id-68ae7123-bf5c-4d15-835c-4a6b91106e77",
                    StandardBusinessDocumentHeaderId = "388214db-29cc-43c7-9543-877e017e5bb4",
                    TimestampId = "TS-76740c34-88d2-4bb6-82d2-9e9f0e474087"
                };

                //Act
                responsvalidator.ValiderTransportkvittering(guidUtility);

                //Assert
            }

            [TestMethod]
            [ExpectedException(typeof(SdpSecurityException))]
            public void FeilSecurityBinaryITransportKvitteringSkalKasteException()
            {
                //Arrange
                AddRsaSha256AlgorithmToCryptoConfig();

                const string securityBinaryKvitteringRequest =
                    "MIIE7jCCA9agAwIBAgIKGBj1bv99Jpi+EzANBgkqhkiG9w0BAQsFADBRMQswCQYDVQQGEwJOTzEdMBsGA1UECgwUQnV5cGFzcyBBUy05ODMxNjMzMjcxIzAhBgNVBAMMGkJ1eXBhc3MgQ2xhc3MgMyBUZXN0NCBDQSAzMB4XDTE0MDQyNDEyMzExMVoXDTE3MDQyNDIxNTkwMFowVTELMAkGA1UEBhMCTk8xGDAWBgNVBAoMD1BPU1RFTiBOT1JHRSBBUzEYMBYGA1UEAwwPUE9TVEVOIE5PUkdFIEFTMRIwEAYDVQQFEwk5ODQ2NjExODUwggEiMA0GCSqGSIb3DQEBAQUAA4IBDwAwggEKAoIBAQDLTnQryf2bmiyQ9q3ylQ6xMl7EhGIbjuziXkRTfL+M94m3ceAiko+r2piefKCiquLMK4j+UDcOapUtLC4dT4c6GhRH4FIOEn5aNS2I/njTenBypWka/VEhQUj7zvIh5G4UXIDIXYvLd7gideeMtkX24KUh2XVlh+PcqLGHirqBwVfFiTn5SKhr/ojhYYEb2xxTk3AY9nLd1MMffKQwUWmfoTos4scREYGI2R2vWxKWPcDqk+jig2DISWSJSuerz3HMYAAmp+Gjt0oFJNiyOFaFyGwT3DvqwOMQWwWXdmLh1NxMgTpghXAaXae76ucm9GDQ9E7ytf+JA096RWoi+5GtAgMBAAGjggHCMIIBvjAJBgNVHRMEAjAAMB8GA1UdIwQYMBaAFD+u9XgLkqNwIDVfWvr3JKBSAfBBMB0GA1UdDgQWBBTVyVLqcjWf1Qd0gsmCTrhXiWeqVDAOBgNVHQ8BAf8EBAMCBLAwFgYDVR0gBA8wDTALBglghEIBGgEAAwIwgbsGA1UdHwSBszCBsDA3oDWgM4YxaHR0cDovL2NybC50ZXN0NC5idXlwYXNzLm5vL2NybC9CUENsYXNzM1Q0Q0EzLmNybDB1oHOgcYZvbGRhcDovL2xkYXAudGVzdDQuYnV5cGFzcy5uby9kYz1CdXlwYXNzLGRjPU5PLENOPUJ1eXBhc3MlMjBDbGFzcyUyMDMlMjBUZXN0NCUyMENBJTIwMz9jZXJ0aWZpY2F0ZVJldm9jYXRpb25MaXN0MIGKBggrBgEFBQcBAQR+MHwwOwYIKwYBBQUHMAGGL2h0dHA6Ly9vY3NwLnRlc3Q0LmJ1eXBhc3Mubm8vb2NzcC9CUENsYXNzM1Q0Q0EzMD0GCCsGAQUFBzAChjFodHRwOi8vY3J0LnRlc3Q0LmJ1eXBhc3Mubm8vY3J0L0JQQ2xhc3MzVDRDQTMuY2VyMA0GCSqGSIb3DQEBCwUAA4IBAQCmMpAGaNplOgx3b4Qq6FLEcpnMOnPlSWBC7pQEDWx6OtNUHDm56fBoyVQYKR6LuGfalnnOKuB/sGSmO3eYlh7uDK9WA7bsNU/W8ZiwYwF6PBRui2rrqYk3kj4NLTNlyh/AOO1a2FDFHu369W0zcjj5ns7qs0K3peXtLX8pVxA8RmjwdGe69P/2r6s2A5CBj7oXZJD0Yo2dJFdsZzonT900sUi+MWzlhj3LxU5/684NWc2NI6ZPof/dyYpy3K/AFzpDLWGSmaDO66hPl7EfoJxEiX0DNBaQzNIyRFPh0ir0jM+32ZQ4goR8bAtyhKeTyA/4+Qx1WQXS3wURCC0lsbMh";
                const string securityBinaryKvittering =
                    "MIIE7jCCA9agAwIBAgIKGBj1bv99Jpi+EzANBgkqhkiG9w0BAQsFADBRMQswCQYDVQQGEwJOTzEdMBsGA1UECgwUQnV5cGFzcyBBUy05ODMxNjMzMjcxIzAhBgNVBAMMGkJ1eXBhc3MgQ2xhc3MgMyBUZXN0NCBDQSAzMB4XDTE0MDQyNDEyMzExMVoXDTE3MDQyNDIxNTkwMFowVTELMAkGA1UEBhMCTk8xGDAWBgNVBAoMD1BPU1RFTiBOT1JHRSBBUzEYMBYGA1UEAwwPUE9TVEVOIE5PUkdFIEFTMRIwEAYDVQQFEwk5ODQ2NjExODUwggEiMA0GCSqGSIb3DQEBAQUAA4IBDwAwggEKAoIBAQDLTnQryf2bmiyQ9q3ylQ6xMl7EhGIbjuziXkRTfL+M94m3ceAiko+r2piefKCiquLMK4j+UDcOapUtLC4dT4c6GhRH4FIOEn5aNS2I/njTenBypWka/VEhQUj7zvIh5G4UXIDIXYvLd7gideeMtkX24KUh2XVlh+PcqLGHirqBwVfFiTn5SKhr/ojhYYEb2xxTk3AY9nLd1MMffKQwUWmfoTos4scREYGI2R2vWxKWPcDqk+jig2DISWSJSuerz3HMYAAmp+Gjt0oFJNiyOFaFyGwT3DvqwOMQWwWXdmLh1NxMgTpghXAaXae76ucm9GDQ9E7ytf+JA096RWoi+5GtAgMBAAGjggHCMIIBvjAJBgNVHRMEAjAAMB8GA1UdIwQYMBaAFD+u9XgLkqNwIDVfWvr3JKBSAfBBMB0GA1UdDgQWBBTVyVLqcjWf1Qd0gsmCTrhXiWeqVDAOBgNVHQ8BAf8EBAMCBLAwFgYDVR0gBA8wDTALBglghEIBGgEAAwIwgbsGA1UdHwSBszCBsDA3oDWgM4YxaHR0cDovL2NybC50ZXN0NC5idXlwYXNzLm5vL2NybC9CUENsYXNzM1Q0Q0EzLmNybDB1oHOgcYZvbGRhcDovL2xkYXAudGVzdDQuYnV5cGFzcy5uby9kYz1CdXlwYXNzLGRjPU5PLENOPUJ1eXBhc3MlMjBDbGFzcyUyMDMlMjBUZXN0NCUyMENBJTIwMz9jZXJ0aWZpY2F0ZVJldm9jYXRpb25MaXN0MIGKBggrBgEFBQcBAQR+MHwwOwYIKwYBBQUHMAGGL2h0dHA6Ly9vY3NwLnRlc3Q0LmJ1eXBhc3Mubm8vb2NzcC9CUENsYXNzM1Q0Q0EzMD0GCCsGAQUFBzAChjFodHRwOi8vY3J0LnRlc3Q0LmJ1eXBhc3Mubm8vY3J0L0JQQ2xhc3MzVDRDQTMuY2VyMA0GCSqGSIb3DQEBCwUAA4IBAQCmMpAGaNplOgx3b4Qq6FLEcpnMOnPlSWBC7pQEDWx6OtNUHDm56fBoyVQYKR6LuGfalnnOKuB/sGSmO3eYlh7uDK9WA7bsNU/W8ZiwYwF6PBRui2rrqYk3kj4NLTNlyh/AOO1a2FDFHu369W0zcjj5ns7qs0K3peXtLX8pVxA8RmjwdGe69P/2r6s2A5CBj7oXZJD0Yo2dJFdsZzonT900sUi+MWzlhj3LxU5/684NWc2NI6ZPof/dyYpy3K/AFzpDLWGSmaDO66hPl7EfoJxEiX0DNBaQzNIyRFPh0ir0jM+32ZQ4goR8bAtyhKeTyA/4+Qx1WQXS3wURCC0lsbMk";

                Assert.IsTrue(securityBinaryKvitteringRequest != securityBinaryKvittering);//skal være forskjellig

                var miljø = Miljø.FunksjoneltTestmiljø;
                var sendtMeldingXmlDocument = TilXmlDokument(SendtMelding.FunksjoneltTestMiljøMedInput(securityBinary: securityBinaryKvitteringRequest));

                var mottattTransportKvittering = TilXmlDokument(TransportKvittering.TransportOkKvittertingFunksjoneltTestmiljøMedInput(securityBinary: securityBinaryKvittering));
                Responsvalidator responsvalidator = new Responsvalidator(mottattTransportKvittering, sendtMeldingXmlDocument, miljø.Sertifikatvalidator);
                GuidUtility guidUtility = new GuidUtility()
                {
                    BinarySecurityTokenId = "X509-513ffecb-cd7e-4bb3-a4c5-47eff314683f",
                    BodyId = "soapBody",
                    DokumentpakkeId = "4fa27c07-8a0f-45a9-954e-c658f6c480af@meldingsformidler.sdp.difi.no",
                    EbMessagingId = "id-68ae7123-bf5c-4d15-835c-4a6b91106e77",
                    StandardBusinessDocumentHeaderId = "388214db-29cc-43c7-9543-877e017e5bb4",
                    TimestampId = "TS-76740c34-88d2-4bb6-82d2-9e9f0e474087"
                };

                //Act
                responsvalidator.ValiderTransportkvittering(guidUtility);

                //Assert
            }

            [TestMethod]
            [ExpectedException(typeof(SdpSecurityException))]
            public void FeilTransportkvitteringSkalKasteSecurityException()
            {
                //Arrange
                AddRsaSha256AlgorithmToCryptoConfig();

                var miljø = Miljø.FunksjoneltTestmiljø;
                var sendtMeldingXmlDocument = TilXmlDokument(SendtMelding.FunksjoneltTestMiljø);

                var mottattTransportKvittering = TilXmlDokument(TransportKvittering.TransportOkKvitteringFikkTilbakeFeilKvittering);
                Responsvalidator responsvalidator = new Responsvalidator(mottattTransportKvittering, sendtMeldingXmlDocument, miljø.Sertifikatvalidator);

                GuidUtility guidUtility = new GuidUtility()
                {
                    BinarySecurityTokenId = "X509-513ffecb-cd7e-4bb3-a4c5-47eff314683f",
                    BodyId = "soapBody",
                    DokumentpakkeId = "4fa27c07-8a0f-45a9-954e-c658f6c480af@meldingsformidler.sdp.difi.no",
                    EbMessagingId = "id-68ae7123-bf5c-4d15-835c-4a6b91106e77",
                    StandardBusinessDocumentHeaderId = "388214db-29cc-43c7-9543-877e017e5bb4",
                    TimestampId = "TS-76740c34-88d2-4bb6-82d2-9e9f0e474087"
                };

                //Act
                responsvalidator.ValiderTransportkvittering(guidUtility);

                //Assert
            }

            [TestMethod]
            [ExpectedException(typeof(SdpSecurityException))]
            public void FeilDokumentpakkeIdITransportkvitteringSkalKasteSecurityException()
            {
                //Arrange
                const string dokumentPakkeIdRequest = "4fa27c07-8a0f-45a9-954e-c658f6c480af@meldingsformidler.sdp.difi.no";
                const string dokumentPakkeIdRespons = "hei27c07-8a0f-45a9-954e-c658f6c480af@meldingsformidler.sdp.difi.no";
                AddRsaSha256AlgorithmToCryptoConfig();


                var miljø = Miljø.FunksjoneltTestmiljø;
                var sendtMeldingXmlDocument = TilXmlDokument(SendtMelding.FunksjoneltTestMiljøMedInput(dokumentPakkeId: dokumentPakkeIdRequest));

                var mottattTransportKvittering = TilXmlDokument(TransportKvittering.TransportOkKvittertingFunksjoneltTestmiljøMedInput(dokumentPakkeId: dokumentPakkeIdRespons));
                Responsvalidator responsvalidator = new Responsvalidator(mottattTransportKvittering, sendtMeldingXmlDocument, miljø.Sertifikatvalidator);

                GuidUtility guidUtility = new GuidUtility()
                {
                    BinarySecurityTokenId = "X509-513ffecb-cd7e-4bb3-a4c5-47eff314683f",
                    BodyId = "soapBody",
                    DokumentpakkeId = dokumentPakkeIdRequest,
                    EbMessagingId = "id-68ae7123-bf5c-4d15-835c-4a6b91106e77",
                    StandardBusinessDocumentHeaderId = "388214db-29cc-43c7-9543-877e017e5bb4",
                    TimestampId = "TS-76740c34-88d2-4bb6-82d2-9e9f0e474087"
                };

                //Act
                responsvalidator.ValiderTransportkvittering(guidUtility);

                //Assert
            }

            [TestMethod]
            public void KrasjerVedUtbyttetDokuemtpakkeId()
            {
                //Arrange
                AddRsaSha256AlgorithmToCryptoConfig();

                var miljø = Miljø.FunksjoneltTestmiljø;
                var sendtMeldingXmlDocument = TilXmlDokument(_sendtMeldingXmlTestMiljø);
                var transportkvitteringXmlDokument = TilXmlDokument(TransportKvittering.TransportOkKvitteringMedByttetDokumentpakkeIdFunksjoneltTestmiljø);
                Responsvalidator responsvalidator = new Responsvalidator(transportkvitteringXmlDokument, sendtMeldingXmlDocument, miljø.Sertifikatvalidator);
                GuidUtility guidUtility = new GuidUtility()
                {
                    BinarySecurityTokenId = "X509-513ffecb-cd7e-4bb3-a4c5-47eff314683f",
                    BodyId = "soapBody",

                    DokumentpakkeId = "4fa27c07-8a0f-45a9-954e-c658f6c480af@meldingsformidler.sdp.difi.no",
                    EbMessagingId = "id-68ae7123-bf5c-4d15-835c-4a6b91106e77",
                    StandardBusinessDocumentHeaderId = "388214db-29cc-43c7-9543-877e017e5bb4",
                    TimestampId = "TS-76740c34-88d2-4bb6-82d2-9e9f0e474087"
                };

                //Act
                responsvalidator.ValiderTransportkvittering(guidUtility);

                //Assert
                Assert.Fail();
            }

        }


        [TestClass]
        public class ValiderMeldingsKvitteringMethod : ResponsvalidatorTester
        {
            [TestMethod]
            public void TestsertifikatValiderer()
            {
                //Arrange
                AddRsaSha256AlgorithmToCryptoConfig();
                var miljø = Miljø.FunksjoneltTestmiljø;

                XmlDocument sendtKvitteringsForespørsel = new XmlDocument();
                sendtKvitteringsForespørsel.LoadXml(_sendtKvitteringsforespørsel);
                Responsvalidator responsvalidator = new Responsvalidator(respons: TilXmlDokument(_responsKvitteringsForespørsel), sendtMelding: sendtKvitteringsForespørsel, sertifikatvalidator: miljø.Sertifikatvalidator);

                //Act
                responsvalidator.ValiderMeldingskvittering();

                //Assert
            }
        }

        [TestClass]
        public class ValiderTomKøKvitteringMethod : ResponsvalidatorTester
        {
            [TestMethod]
            public void TestsertifikatValiderer()
            {
                //Arrange
                AddRsaSha256AlgorithmToCryptoConfig();

                var miljø = Miljø.FunksjoneltTestmiljø;
                XmlDocument sendtKvitteringsForespørsel = new XmlDocument();
                sendtKvitteringsForespørsel.LoadXml(_sendtKvitteringsforespørsel);
                Responsvalidator responsvalidator = new Responsvalidator(TilXmlDokument(_tomKøKvittering), sendtKvitteringsForespørsel, miljø.Sertifikatvalidator);

                //Act
                responsvalidator.ValiderTomkøkvittering();

                //Assert
            }
        }
    }
}