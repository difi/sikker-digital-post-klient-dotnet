using System;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SikkerDigitalPost.Klient.Envelope;

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
            var settings = new XmlReaderSettings();
            settings.Schemas.Add(Navnerom.Ns10, AsicEXsdPath());
            settings.Schemas.Add(Navnerom.Ns11, XAdESXsdPath());
            settings.Schemas.Add(Navnerom.Ns5, XmlDsigCoreSchema());

            settings.ValidationType = ValidationType.Schema;
            settings.ValidationFlags = XmlSchemaValidationFlags.ReportValidationWarnings;
            settings.ValidationEventHandler += ValidationEventHandler;
            
            try
            {
                var reader = XmlReader.Create(new MemoryStream(Signatur.Bytes), settings);
                var document = new XmlDocument();
                document.Load(reader);
                Assert.IsFalse(_harFeilet);
            }
            catch (Exception e)
            {
                var message = String.Format("Validering feilet: {0} Indre feilmelding: {1}", e.Message, e.InnerException);
                Assert.Fail(message);
            }
        }

        private static void ValidationEventHandler(object sender, ValidationEventArgs e)
        {
            if (e.Severity == XmlSeverityType.Warning)
                Console.WriteLine("\tWarning: Matching schema not found.  No validation occurred. " + e.Message);
            else if (e.Severity == XmlSeverityType.Error)
                Console.WriteLine("\tValidation error: " + e.Message);
            _harFeilet = true;
        }

        private string AsicEXsdPath()
        {
            return XsdPath(@"w3/ts_102918v010201.xsd");
        }

        private string XmlDsigCoreSchema()
        {
            return XsdPath(@"w3/xmldsig-core-schema.xsd");
        }

        private string XAdESXsdPath()
        {
            return XsdPath(@"w3/XAdES.xsd");
        }

        private string XsdPath(string filnavn)
        {
            return Path.Combine(TestDataMappe, "xsd", filnavn);
        }
    }
}
