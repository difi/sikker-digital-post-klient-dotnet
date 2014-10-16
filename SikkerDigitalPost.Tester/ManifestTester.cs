using System;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SikkerDigitalPost.Domene;
using SikkerDigitalPost.Klient;
using SikkerDigitalPost.Klient.Envelope;

namespace SikkerDigitalPost.Tester
{
    [TestClass]
    public class ManifestTester : TestBase
    {
        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            Initialiser();
        }

        private static bool _harFeilet;

        [TestMethod]
        public void ValidereManifestMotXsdValiderer()
        {
            var settings = new XmlReaderSettings();
            settings.Schemas.Add(Navnerom.Ns9, ManifestXsdPath());
            //settings.Schemas.Add(Navnerom.Ns9, FellesXsdPath());
            //settings.Schemas.Add(Navnerom.Ns5, XmlDsigCoreSchema());
            settings.ValidationType = ValidationType.Schema;
            settings.ValidationFlags = XmlSchemaValidationFlags.ReportValidationWarnings;
            settings.ValidationEventHandler += ValidationEventHandler;

            try
            {
                var reader = XmlReader.Create(new MemoryStream(Manifest.Bytes), settings);
                var document = new XmlDocument();
                document.Load(reader);
                Assert.IsFalse(_harFeilet);
            }
            catch (Exception e)
            {
                var message = String.Format("Validering feilet: {0} Inndre feilmelding: {1}", e.Message,e.InnerException);
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

        private string ManifestXsdPath()
        {
            return XsdFil("sdp-manifest.xsd");
        }

        private string FellesXsdPath()
        {
            return XsdFil("sdp-felles.xsd");
        }

        private string XmlDsigCoreSchema()
        {
            return XsdFil(@"w3/xmldsig-core-schema.xsd");
            
        }

        private string XsdFil(string bareFilnavn)
        {
            return Path.Combine(TestDataMappe, "xsd", bareFilnavn);
        }
    }
}
