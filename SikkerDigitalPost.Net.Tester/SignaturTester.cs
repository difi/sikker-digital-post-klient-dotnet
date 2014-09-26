using System;
using System.IO;
using System.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SikkerDigitalPost.Net.Tests
{
    [TestClass]
    public class SignaturTester : TestBase
    {
        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            Initialiser();
        }

        [TestMethod]
        public void ValidereSignaturMotXsdValiderer()
        {
            var settings = new XmlReaderSettings();
            settings.Schemas.Add("http://uri.etsi.org/01903/v1.3.2#", SignaturXsdPath());
            settings.Schemas.Add("http://www.w3.org/2000/09/xmldsig#", XmlDsigCoreSchema());
            settings.ValidationType = ValidationType.Schema;
            
            try
            {
                var reader = XmlReader.Create(new MemoryStream(Signatur.Bytes), settings);
                var document = new XmlDocument();
                document.Load(reader);
            }
            catch (Exception e)
            {
                var message = String.Format("Validering feilet: {0} Inndre feilmelding: {1}", e.Message, e.InnerException);
                Assert.Fail(message);
            }
        }

        private string XmlDsigCoreSchema()
        {
            return XsdPath("xmldsig-core-schema.xsd");
        }

        private string SignaturXsdPath()
        {
            return XsdPath("XAdES.xsd");
        }

        private string XsdPath(string filnavn)
        {
            return Path.Combine(TestDataMappe, "xsd", filnavn);
        }
    }
}
