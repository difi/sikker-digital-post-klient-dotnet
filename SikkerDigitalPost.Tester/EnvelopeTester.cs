using System;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SikkerDigitalPost.Klient.Envelope;

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

        private static bool _harFeilet;
        private const string TolerateThisError = "It is an error if there is a member of the attribute uses of a type definition with type xs:ID or derived from xs:ID and another attribute with type xs:ID matches an attribute wildcard.";

        [TestMethod]
        public void ValidereEnvelopeMotXsdValiderer()
        {
            var settings = new XmlReaderSettings();
            settings.XmlResolver = null;
            
            settings.Schemas.Add(Navnerom.env, SoapEnvelopeXsdPath());
            settings.Schemas.Add(Navnerom.Ns4, EnvelopeXsdPath());

            settings.Schemas.Add(Navnerom.Ns6, EbmsHeaderXsdPath());
            
            settings.Schemas.Add(Navnerom.Ns9, FellesXsdPath());
            settings.Schemas.Add(Navnerom.Ns9, MeldingXsdPath());
            
            settings.Schemas.Add(Navnerom.Ns5, XmlDsigCoreXsdPath());
            settings.Schemas.Add(Navnerom.enc, XmlXencXsdPath());
            settings.Schemas.Add("http://www.w3.org/XML/1998/namespace", XmlXsdPath());
            settings.Schemas.Add("http://www.w3.org/2001/10/xml-exc-c14n#", ExecC14nXsdPath());
            
            settings.Schemas.Add(Navnerom.Ns3, StandardBusinessDocumentHeaderXsdPath());
            settings.Schemas.Add(Navnerom.Ns3, DocumentIdentificationXsdPath());
            settings.Schemas.Add(Navnerom.Ns3, ManifestXsdPath());
            settings.Schemas.Add(Navnerom.Ns3, PartnerXsdPath());
            settings.Schemas.Add(Navnerom.Ns3, BusinessScopeXsdPath());
            settings.Schemas.Add(Navnerom.Ns3, BasicTypesXsdPath());

            settings.Schemas.Add(Navnerom.wsu, WsSecurityUtilityXsdPath());
            settings.Schemas.Add(Navnerom.wsse, WsSecuritySecExt1_0XsdPath());

            settings.ValidationType = ValidationType.Schema;
            settings.ValidationFlags = XmlSchemaValidationFlags.ReportValidationWarnings;
            settings.ValidationEventHandler += ValidationEventHandler;
            try
            {
                var reader = XmlReader.Create(new MemoryStream(Envelope.Bytes), settings);
                var document = new XmlDocument();
                document.Load(reader);
                Assert.IsFalse(_harFeilet);
            }
            catch (Exception e)
            {
                var message = String.Format("Validering feilet: {0}, Indre feilmelding: {1}", e.Message, e.InnerException);
                Assert.Fail(message);
            }
        }

        private static void ValidationEventHandler(object sender, ValidationEventArgs e)
        {
            if (e.Severity == XmlSeverityType.Warning)
                Console.WriteLine("\tWarning: Matching schema not found.  No validation occurred. " + e.Message);
            else if (e.Severity == XmlSeverityType.Error)
            {
                Console.WriteLine("\tValidation error: " + e.Message);
                if (e.Message.Equals(TolerateThisError))
                {
                    Console.WriteLine("\tThe error above is the only one we will allow to occur.");
                    return;
                }
            }
            _harFeilet = true;
        }

        private string SoapEnvelopeXsdPath()
        {
            return XsdPath(@"w3/soap-envelope.xsd");
        }

        private string EnvelopeXsdPath()
        {
            return XsdPath(@"xmlsoap/envelope.xsd");
        }

        private string XmlXsdPath()
        {
            return XsdPath(@"w3/xml.xsd");
        }

        private string ExecC14nXsdPath()
        {
            return XsdPath(@"w3/exc-c14n.xsd");
        }

        private string EbmsHeaderXsdPath()
        {
            return XsdPath(@"ebxml/ebms-header-3_0-200704.xsd");
        }

        private string XmlDsigCoreXsdPath()
        {
            return XsdPath(@"w3/xmldsig-core-schema.xsd");
        }

        private string XmlXencXsdPath()
        {
            return XsdPath(@"w3/xenc-schema.xsd");
        }

        private string FellesXsdPath()
        {
            return XsdPath("sdp-felles.xsd");
        }

        private string MeldingXsdPath()
        {
            return XsdPath("sdp-melding.xsd");
        }

        private string StandardBusinessDocumentHeaderXsdPath()
        {
            return XsdPath(@"SBDH20040506-02/StandardBusinessDocumentHeader.xsd");
        }

        private string DocumentIdentificationXsdPath()
        {
            return XsdPath(@"SBDH20040506-02/DocumentIdentification.xsd");
        }

        private string ManifestXsdPath()
        {
            return XsdPath(@"SBDH20040506-02/Manifest.xsd");
        }

        private string PartnerXsdPath()
        {
            return XsdPath(@"SBDH20040506-02/Partner.xsd");
        }

        private string BusinessScopeXsdPath()
        {
            return XsdPath(@"SBDH20040506-02/BusinessScope.xsd");
        }

        private string BasicTypesXsdPath()
        {
            return XsdPath(@"SBDH20040506-02/BasicTypes.xsd");
        }

        private string WsSecurityUtilityXsdPath()
        {
            return XsdPath(@"wssecurity/oasis-200401-wss-wssecurity-utility-1.0.xsd");
        }

        private string WsSecuritySecExt1_0XsdPath()
        {
            return XsdPath(@"wssecurity/oasis-200401-wss-wssecurity-secext-1.0.xsd");
        }

        private string WsSecuritySecExt1_1XsdPath()
        {
            return XsdPath(@"wssecurity/oasis-wss-wssecurity-secext-1.1.xsd");
        }
        
        private string XsdPath(string filnavn)
        {
            return Path.Combine(TestDataMappe, "xsd", filnavn);
        }
    }
}
