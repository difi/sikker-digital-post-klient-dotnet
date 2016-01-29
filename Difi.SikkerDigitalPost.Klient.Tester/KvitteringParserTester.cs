﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Difi.SikkerDigitalPost.Klient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using ApiClientShared;
using Difi.Felles.Utility.Utilities;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer.Forretning;

namespace Difi.SikkerDigitalPost.Klient.Tests
{
    [TestClass()]
    public class KvitteringParserTester
    {
        ResourceUtility ResourceUtility = new ResourceUtility("Difi.SikkerDigitalPost.Klient.Tester.Skjema.Eksempler.Kvitteringer");

        [TestClass]
        public class TilKvitteringMethod : KvitteringParserTester
        {
            [TestMethod]
            public void ParserLeveringskvittering()
            {
                //Arrange
                var xml = TilXmlDokument("Leveringskvittering.xml");
                const string konversjonsId = "716cffc1-58aa-4198-98df-281f4a1a1384";
                const string meldingsId = "5a93d7e9-e9e5-4013-ab19-c32d9eb0f3d0";
                const string referanseTilMeldingId = "03eafe0f-43ae-4184-82f6-ab194dd1b426";
                const string tidspunkt = "2015-11-10T08:37:24.695+01:00";

                //Act
                var leveringskvittering = KvitteringParser.TilLeveringskvittering(xml);

                //Assert
                Assert.AreEqual(konversjonsId, leveringskvittering.KonversasjonsId.ToString());
                Assert.AreEqual(meldingsId, leveringskvittering.MeldingsId);
                Assert.AreEqual(referanseTilMeldingId, leveringskvittering.ReferanseTilMeldingId);
                Assert.AreEqual(DateTime.Parse(tidspunkt), leveringskvittering.Levert);


                var leveringskvitteringRådata =
                    string.Format("<env:Envelope xmlns:env=\"http://www.w3.org/2003/05/soap-envelope\"><env:Header><wsse:Security xmlns:wsse=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd\" xmlns:wsu=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd\" env:mustUnderstand=\"true\"><wsse:BinarySecurityToken EncodingType=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#Base64Binary\" ValueType=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-x509-token-profile-1.0#X509v3\" wsu:Id=\"X509-9fc934a5-e075-4b32-93c8-e219e9c60ffc\">MIIE7jCCA9agAwIBAgIKGBj1bv99Jpi+EzANBgkqhkiG9w0BAQsFADBRMQswCQYDVQQGEwJOTzEdMBsGA1UECgwUQnV5cGFzcyBBUy05ODMxNjMzMjcxIzAhBgNVBAMMGkJ1eXBhc3MgQ2xhc3MgMyBUZXN0NCBDQSAzMB4XDTE0MDQyNDEyMzExMVoXDTE3MDQyNDIxNTkwMFowVTELMAkGA1UEBhMCTk8xGDAWBgNVBAoMD1BPU1RFTiBOT1JHRSBBUzEYMBYGA1UEAwwPUE9TVEVOIE5PUkdFIEFTMRIwEAYDVQQFEwk5ODQ2NjExODUwggEiMA0GCSqGSIb3DQEBAQUAA4IBDwAwggEKAoIBAQDLTnQryf2bmiyQ9q3ylQ6xMl7EhGIbjuziXkRTfL+M94m3ceAiko+r2piefKCiquLMK4j+UDcOapUtLC4dT4c6GhRH4FIOEn5aNS2I/njTenBypWka/VEhQUj7zvIh5G4UXIDIXYvLd7gideeMtkX24KUh2XVlh+PcqLGHirqBwVfFiTn5SKhr/ojhYYEb2xxTk3AY9nLd1MMffKQwUWmfoTos4scREYGI2R2vWxKWPcDqk+jig2DISWSJSuerz3HMYAAmp+Gjt0oFJNiyOFaFyGwT3DvqwOMQWwWXdmLh1NxMgTpghXAaXae76ucm9GDQ9E7ytf+JA096RWoi+5GtAgMBAAGjggHCMIIBvjAJBgNVHRMEAjAAMB8GA1UdIwQYMBaAFD+u9XgLkqNwIDVfWvr3JKBSAfBBMB0GA1UdDgQWBBTVyVLqcjWf1Qd0gsmCTrhXiWeqVDAOBgNVHQ8BAf8EBAMCBLAwFgYDVR0gBA8wDTALBglghEIBGgEAAwIwgbsGA1UdHwSBszCBsDA3oDWgM4YxaHR0cDovL2NybC50ZXN0NC5idXlwYXNzLm5vL2NybC9CUENsYXNzM1Q0Q0EzLmNybDB1oHOgcYZvbGRhcDovL2xkYXAudGVzdDQuYnV5cGFzcy5uby9kYz1CdXlwYXNzLGRjPU5PLENOPUJ1eXBhc3MlMjBDbGFzcyUyMDMlMjBUZXN0NCUyMENBJTIwMz9jZXJ0aWZpY2F0ZVJldm9jYXRpb25MaXN0MIGKBggrBgEFBQcBAQR+MHwwOwYIKwYBBQUHMAGGL2h0dHA6Ly9vY3NwLnRlc3Q0LmJ1eXBhc3Mubm8vb2NzcC9CUENsYXNzM1Q0Q0EzMD0GCCsGAQUFBzAChjFodHRwOi8vY3J0LnRlc3Q0LmJ1eXBhc3Mubm8vY3J0L0JQQ2xhc3MzVDRDQTMuY2VyMA0GCSqGSIb3DQEBCwUAA4IBAQCmMpAGaNplOgx3b4Qq6FLEcpnMOnPlSWBC7pQEDWx6OtNUHDm56fBoyVQYKR6LuGfalnnOKuB/sGSmO3eYlh7uDK9WA7bsNU/W8ZiwYwF6PBRui2rrqYk3kj4NLTNlyh/AOO1a2FDFHu369W0zcjj5ns7qs0K3peXtLX8pVxA8RmjwdGe69P/2r6s2A5CBj7oXZJD0Yo2dJFdsZzonT900sUi+MWzlhj3LxU5/684NWc2NI6ZPof/dyYpy3K/AFzpDLWGSmaDO66hPl7EfoJxEiX0DNBaQzNIyRFPh0ir0jM+32ZQ4goR8bAtyhKeTyA/4+Qx1WQXS3wURCC0lsbMh</wsse:BinarySecurityToken><wsu:Timestamp wsu:Id=\"TS-d5e2ecb4-5fe3-4a2f-b70b-6490d003888c\"><wsu:Created>2015-11-10T07:37:27.156Z</wsu:Created><wsu:Expires>2015-11-10T07:42:27.156Z</wsu:Expires></wsu:Timestamp><ds:Signature xmlns:ds=\"http://www.w3.org/2000/09/xmldsig#\" Id=\"SIG-e368ce00-d76d-4e4b-9f92-f9394fe85823\"><ds:SignedInfo><ds:CanonicalizationMethod Algorithm=\"http://www.w3.org/2001/10/xml-exc-c14n#\"><ec:InclusiveNamespaces xmlns:ec=\"http://www.w3.org/2001/10/xml-exc-c14n#\" PrefixList=\"env\"/></ds:CanonicalizationMethod><ds:SignatureMethod Algorithm=\"http://www.w3.org/2001/04/xmldsig-more#rsa-sha256\"/><ds:Reference URI=\"#id-a7550af9-b1b3-49b2-b45d-83f2fdc7af1a\"><ds:Transforms><ds:Transform Algorithm=\"http://www.w3.org/2001/10/xml-exc-c14n#\"><ec:InclusiveNamespaces xmlns:ec=\"http://www.w3.org/2001/10/xml-exc-c14n#\" PrefixList=\"\"/></ds:Transform></ds:Transforms><ds:DigestMethod Algorithm=\"http://www.w3.org/2001/04/xmlenc#sha256\"/><ds:DigestValue>r4fYH1zvBJAPFd5kOauJIykd9ZB9YJa7qrMUzYWFHbQ=</ds:DigestValue></ds:Reference><ds:Reference URI=\"#TS-d5e2ecb4-5fe3-4a2f-b70b-6490d003888c\"><ds:Transforms><ds:Transform Algorithm=\"http://www.w3.org/2001/10/xml-exc-c14n#\"><ec:InclusiveNamespaces xmlns:ec=\"http://www.w3.org/2001/10/xml-exc-c14n#\" PrefixList=\"wsse env\"/></ds:Transform></ds:Transforms><ds:DigestMethod Algorithm=\"http://www.w3.org/2001/04/xmlenc#sha256\"/><ds:DigestValue>dg5zfAZ089xTVdl3fFydERg0db6a5NIMrSj2cPqNsao=</ds:DigestValue></ds:Reference><ds:Reference URI=\"#id-828ee37f-7a4c-4713-bd47-2bb3aad7430a\"><ds:Transforms><ds:Transform Algorithm=\"http://www.w3.org/2001/10/xml-exc-c14n#\"><ec:InclusiveNamespaces xmlns:ec=\"http://www.w3.org/2001/10/xml-exc-c14n#\" PrefixList=\"\"/></ds:Transform></ds:Transforms><ds:DigestMethod Algorithm=\"http://www.w3.org/2001/04/xmlenc#sha256\"/><ds:DigestValue>aZKVVD6+q6Pg3hWnBx6acHomOA+mlVpmyAs+VhdWTHs=</ds:DigestValue></ds:Reference></ds:SignedInfo><ds:SignatureValue>IBkZrLiDsX+/DqdQ/HFoOEwwNWofLAWRrQDxL9zmXcrinb7SolyFceH5DxVKc0+4/+Ri5Q9cLDExxyPq579LSEpaIWcIkzZgGdSqdBqjyx5pIJ4P54gXnybsJOa5H5j5hoA6fcp77rKs+QCnsPHolS1NC/PrUyXT9iTaTBZCnUhidiDQ8DWilC3Hnl++aC/8AAKhS/yMHAUNM/rlqA5RBAOYG/yWquY6f1Yh6W6RFxTtdKMC/BAOEbKwGTK05qiD3WH2XCf0gaJ0O+Uz/MOX8A4ssa4dtjdEJboGe8r4xh1ubHp2K1zP0VoQtuPpVRsKLkjyoSSy71QiItV9twNRDw==</ds:SignatureValue><ds:KeyInfo Id=\"KI-a40aa6e6-84f6-4051-a097-05fb717a9359\"><wsse:SecurityTokenReference wsu:Id=\"STR-a7ef6891-1d02-474a-9814-c395cc15d1e1\"><wsse:Reference URI=\"#X509-9fc934a5-e075-4b32-93c8-e219e9c60ffc\" ValueType=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-x509-token-profile-1.0#X509v3\"/></wsse:SecurityTokenReference></ds:KeyInfo></ds:Signature></wsse:Security><eb:Messaging xmlns:eb=\"http://docs.oasis-open.org/ebxml-msg/ebms/v3.0/ns/core/200704/\" xmlns:wsu=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd\" env:mustUnderstand=\"true\" wsu:Id=\"id-828ee37f-7a4c-4713-bd47-2bb3aad7430a\"><ns6:UserMessage xmlns:ns10=\"http://uri.etsi.org/2918/v1.2.1#\" xmlns:ns11=\"http://uri.etsi.org/01903/v1.3.2#\" xmlns:ns2=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:ns3=\"http://www.unece.org/cefact/namespaces/StandardBusinessDocumentHeader\" xmlns:ns4=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:ns5=\"http://www.w3.org/2000/09/xmldsig#\" xmlns:ns6=\"http://docs.oasis-open.org/ebxml-msg/ebms/v3.0/ns/core/200704/\" xmlns:ns7=\"http://docs.oasis-open.org/ebxml-bp/ebbp-signals-2.0\" xmlns:ns8=\"http://www.w3.org/1999/xlink\" xmlns:ns9=\"http://begrep.difi.no/sdp/schema_v10\" mpc=\"urn:prioritert:queue1\"><ns6:MessageInfo><ns6:Timestamp>2015-11-10T08:37:27.153+01:00</ns6:Timestamp><ns6:MessageId>{1}</ns6:MessageId><ns6:RefToMessageId>{2}</ns6:RefToMessageId></ns6:MessageInfo><ns6:PartyInfo><ns6:From><ns6:PartyId type=\"urn:oasis:names:tc:ebcore:partyid-type:iso6523:9908\">9908:984661185</ns6:PartyId><ns6:Role>urn:sdp:meldingsformidler</ns6:Role></ns6:From><ns6:To><ns6:PartyId type=\"urn:oasis:names:tc:ebcore:partyid-type:iso6523:9908\">9908:984661185</ns6:PartyId><ns6:Role>urn:sdp:avsender</ns6:Role></ns6:To></ns6:PartyInfo><ns6:CollaborationInfo><ns6:AgreementRef>http://begrep.difi.no/SikkerDigitalPost/1.0/transportlag/Meldingsutveksling/FormidleDigitalPostForsendelse</ns6:AgreementRef><ns6:Service>SDP</ns6:Service><ns6:Action>KvitteringsForespoersel</ns6:Action><ns6:ConversationId>09fcbc2b-df82-440f-95cc-8987498ed8f3</ns6:ConversationId></ns6:CollaborationInfo><ns6:PayloadInfo><ns6:PartInfo/></ns6:PayloadInfo></ns6:UserMessage></eb:Messaging></env:Header><env:Body xmlns:wsu=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd\" wsu:Id=\"id-a7550af9-b1b3-49b2-b45d-83f2fdc7af1a\"><ns3:StandardBusinessDocument xmlns:ns3=\"http://www.unece.org/cefact/namespaces/StandardBusinessDocumentHeader\" xmlns:ns9=\"http://begrep.difi.no/sdp/schema_v10\"><ns3:StandardBusinessDocumentHeader><ns3:HeaderVersion>1.0</ns3:HeaderVersion><ns3:Sender><ns3:Identifier Authority=\"urn:oasis:names:tc:ebcore:partyid-type:iso6523:9908\">9908:984661185</ns3:Identifier></ns3:Sender><ns3:Receiver><ns3:Identifier Authority=\"urn:oasis:names:tc:ebcore:partyid-type:iso6523:9908\">9908:984661185</ns3:Identifier></ns3:Receiver><ns3:DocumentIdentification><ns3:Standard>urn:no:difi:sdp:1.0</ns3:Standard><ns3:TypeVersion>1.0</ns3:TypeVersion><ns3:InstanceIdentifier>09fcbc2b-df82-440f-95cc-8987498ed8f3</ns3:InstanceIdentifier><ns3:Type>kvittering</ns3:Type><ns3:CreationDateAndTime>2015-11-10T08:37:24.695+01:00</ns3:CreationDateAndTime></ns3:DocumentIdentification><ns3:BusinessScope><ns3:Scope><ns3:Type>ConversationId</ns3:Type><ns3:InstanceIdentifier>{0}</ns3:InstanceIdentifier><ns3:Identifier>urn:no:difi:sdp:1.0</ns3:Identifier></ns3:Scope></ns3:BusinessScope></ns3:StandardBusinessDocumentHeader><ns9:kvittering><Signature xmlns=\"http://www.w3.org/2000/09/xmldsig#\"><SignedInfo><CanonicalizationMethod Algorithm=\"http://www.w3.org/2001/10/xml-exc-c14n#\"/><SignatureMethod Algorithm=\"http://www.w3.org/2001/04/xmldsig-more#rsa-sha256\"/><Reference URI=\"\"><Transforms><Transform Algorithm=\"http://www.w3.org/2000/09/xmldsig#enveloped-signature\"/></Transforms><DigestMethod Algorithm=\"http://www.w3.org/2001/04/xmlenc#sha256\"/><DigestValue>JVUp11ScT3r4fJL3SqcEf9Mad/Z/UW+MlBhnStB8YxQ=</DigestValue></Reference></SignedInfo><SignatureValue>Ps22KO0ZkR9M6S+Ql/M2TpAt2B1jvx7kxbLF3Z1vN0WiZOJlZOa6pNiBQQ9ifO8QmW0HJFpDYaGy87Wd+tIKQfvjkIYB/0qfTnxsZMPVEyJCz2p5akJzT0+i5ebTcHNhRqOntSGPAuJzid6RWN3k7V/SIRUgigoM5MV4sfrV5eLgnKvXxi7EZsxjQX7GPoiKeP1llgO35VEcJ8150zd8jKBv+CP6dStMNV33zCJKur/8WpFN46hFGgJ1EEbSUeRY/HwBb29e0mxGKNXrAn1MzaPHp+9cfeSpdzsBgJ6swQ+YhyjdB4d/C8ev4ZhqrZZRnO50sJyfgNAwjSqizl11vg==</SignatureValue><KeyInfo><X509Data><X509Certificate>MIIE7jCCA9agAwIBAgIKGBZrmEgzTHzeJjANBgkqhkiG9w0BAQsFADBRMQswCQYDVQQGEwJOTzEdMBsGA1UECgwUQnV5cGFzcyBBUy05ODMxNjMzMjcxIzAhBgNVBAMMGkJ1eXBhc3MgQ2xhc3MgMyBUZXN0NCBDQSAzMB4XDTE0MDQyNDEyMzA1MVoXDTE3MDQyNDIxNTkwMFowVTELMAkGA1UEBhMCTk8xGDAWBgNVBAoMD1BPU1RFTiBOT1JHRSBBUzEYMBYGA1UEAwwPUE9TVEVOIE5PUkdFIEFTMRIwEAYDVQQFEwk5ODQ2NjExODUwggEiMA0GCSqGSIb3DQEBAQUAA4IBDwAwggEKAoIBAQCLCxU4oBhtGmJxXZWbdWdzO2uA3eRNW/kPdddL1HYl1iXLV/g+H2Q0ELadWLggkS+1kOd8/jKxEN++biMmmDqqCWbzNdmEd1j4lctSlH6M7tt0ywmXIYdZMz5kxcLAMNXsaqnPdikI9uPJZQEL3Kc8hXhXISvpzP7gYOvKHg41uCxu1xCZQOM6pTlNbxemBYqvES4fRh2xvB9aMjwkB4Nz8jrIsyoPI89i05OmGMkI5BPZt8NTa40Yf3yU+SQECW0GWalB5cxaTMeB01tqslUzBJPV3cQx+AhtQG4hkOhQnAMDJramSPVtwbEnqOjQ+lyNmg5GQ4FJO02ApKJTZDTHAgMBAAGjggHCMIIBvjAJBgNVHRMEAjAAMB8GA1UdIwQYMBaAFD+u9XgLkqNwIDVfWvr3JKBSAfBBMB0GA1UdDgQWBBQ1gsJfVC7KYGiWVLP7ZwzppyVYTTAOBgNVHQ8BAf8EBAMCBLAwFgYDVR0gBA8wDTALBglghEIBGgEAAwIwgbsGA1UdHwSBszCBsDA3oDWgM4YxaHR0cDovL2NybC50ZXN0NC5idXlwYXNzLm5vL2NybC9CUENsYXNzM1Q0Q0EzLmNybDB1oHOgcYZvbGRhcDovL2xkYXAudGVzdDQuYnV5cGFzcy5uby9kYz1CdXlwYXNzLGRjPU5PLENOPUJ1eXBhc3MlMjBDbGFzcyUyMDMlMjBUZXN0NCUyMENBJTIwMz9jZXJ0aWZpY2F0ZVJldm9jYXRpb25MaXN0MIGKBggrBgEFBQcBAQR+MHwwOwYIKwYBBQUHMAGGL2h0dHA6Ly9vY3NwLnRlc3Q0LmJ1eXBhc3Mubm8vb2NzcC9CUENsYXNzM1Q0Q0EzMD0GCCsGAQUFBzAChjFodHRwOi8vY3J0LnRlc3Q0LmJ1eXBhc3Mubm8vY3J0L0JQQ2xhc3MzVDRDQTMuY2VyMA0GCSqGSIb3DQEBCwUAA4IBAQCe67UOZ/VSwcH2ov1cOSaWslL7JNfqhyNZWGpfgX1c0Gh+KkO3eVkMSozpgX6M4eeWBWJGELMiVN1LhNaGxBU9TBMdeQ3SqK219W6DXRJ2ycBtaVwQ26V5tWKRN4UlRovYYiY+nMLx9VrLOD4uoP6fm9GE5Fj0vSMMPvOEXi0NsN+8MUm3HWoBeUCLyFpe7/EPsS/Wud5bb0as/E2zIztRodxfNsoiXNvWaP2ZiPWFunIjK1H/8EcktEW1paiPd8AZek/QQoG0MKPfPIJuqH+WJU3a8J8epMDyVfaek+4+l9XOeKwVXNSOP/JSwgpOJNzTdaDOM+uVuk75n2191Fd7</X509Certificate></X509Data></KeyInfo></Signature><ns9:tidspunkt>{3}</ns9:tidspunkt><ns9:levering/></ns9:kvittering></ns3:StandardBusinessDocument></env:Body></env:Envelope>", konversjonsId, meldingsId, referanseTilMeldingId, tidspunkt);

            }

            [TestMethod]
            public void ParserMottakskvittering()
            {
                //Arrange
                var varslingFeiletKvittering = TilXmlDokument("Mottakskvittering.xml");


                //Act

                //Assert
            }

            [TestMethod]
            public void ParserReturpostkvittering()
            {
                //Arrange
                var xml = TilXmlDokument("Returpostkvittering.xml");

                //Act

                //Assert
            }

            [TestMethod]
            public void ParserVarslingFeiletKvittering()
            {
                //Arrange
                var xml = TilXmlDokument("VarslingFeiletKvittering.xml");

                //Act

                //Assert
            }

            private XmlDocument TilXmlDokument(string kvittering)
            {
                return XmlUtility.TilXmlDokument(Encoding.UTF8.GetString(ResourceUtility.ReadAllBytes(true, kvittering)));
            }

            [TestMethod]
            public void ParserÅpningskvittering()
            {
                //Arrange
                //XmlUtility.TilXmlDokument(ResourceUtility.ReadAllBytes(true, k));

                //Act

                //Assert
            }

        }

    }
}