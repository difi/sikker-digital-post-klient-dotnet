using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Xml;
using System.Xml.Schema;
using System.Xml.XPath;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SikkerDigitalPost.Net.Domene.Entiteter;
using SikkerDigitalPost.Net.Domene.Entiteter.AsicE.Manifest;

namespace SikkerDigitalPost.Net.Tests
{
    [TestClass]
    public class ManifestTester : TestBase
    {
        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            Initialiser();
        }

        [TestMethod]
        public void ValidereManifestMotXmlValiderer()
        {
            var settings = new XmlReaderSettings();
            settings.Schemas.Add("http://begrep.difi.no/sdp/schema_v10", ManifestXsdPath());
            settings.Schemas.Add("http://begrep.difi.no/sdp/schema_v10", FellesXsdPath());
            settings.Schemas.Add("http://www.w3.org/2000/09/xmldsig#", XmlDsigCoreSchema());
            settings.ValidationType = ValidationType.Schema;
            
            try
            {
                var reader = XmlReader.Create(new MemoryStream(Manifest.Bytes), settings);
                var document = new XmlDocument();
                document.Load(reader);
            }
            catch (Exception e)
            {
                var message = String.Format("Validering feilet: {0} Inndre feilmelding: {1}", e.Message,e.InnerException);
                Assert.Fail(message);
            }
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
            return XsdFil("xmldsig-core-schema.xsd");
            
        }

        private string XsdFil(string bareFilnavn)
        {
            return Path.Combine(TestDataMappe, "xsd", bareFilnavn);
        }
    }
}
