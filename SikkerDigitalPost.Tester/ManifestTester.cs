using System;
using System.IO;
using System.Xml;
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

        [TestMethod]
        public void ValidereManifestMotXsdValiderer()
        {
            var settings = new XmlReaderSettings();
            settings.Schemas.Add(Navnerom.Ns9, ManifestXsdPath());
            settings.Schemas.Add(Navnerom.Ns9, FellesXsdPath());
            settings.Schemas.Add(Navnerom.Ns5, XmlDsigCoreSchema());
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
