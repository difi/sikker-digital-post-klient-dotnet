using System;
using System.IO;
using System.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SikkerDigitalPost.Net.Tests
{
    [TestClass]
    public class EnvelopeTester : TestBase
    {
        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            Initialiser();
        }

        [TestMethod]
        public void ValidereEnvelopeMotXsdValiderer()
        {
            var settings = new XmlReaderSettings();
            settings.XmlResolver = null;
            settings.Schemas.Add("http://begrep.difi.no/sdp/schema_v10", MeldingXsdSchema());
            settings.Schemas.Add("http://begrep.difi.no/sdp/schema_v10", FellesXsdSchema());
            settings.Schemas.Add("http://www.w3.org/2001/04/xmlenc#", XmlXencSchema());
            settings.Schemas.Add("http://www.w3.org/2000/09/xmldsig#", XmlDsigCoreSchema());
            settings.ValidationType = ValidationType.Schema;
            Envelope.SkrivTilFil(@"Z:\Development\Digipost\envelope.xml");
            try
            {
                var reader = XmlReader.Create(new MemoryStream(Envelope.Bytes()), settings);
                var document = new XmlDocument();
                document.Load(reader);
            }
            catch (Exception e)
            {
                var message = String.Format("Validering feilet: {0}, Indre feilmelding: {1}", e.Message, e.InnerException);
                Assert.Fail(message);
            }
        }

        private string XmlDsigCoreSchema()
        {
            return XsdPath("xmldsig-core-schema.xsd");
        }

        private string XmlXencSchema()
        {
            return XsdPath("xenc-schema.xsd");
        }

        private string FellesXsdSchema()
        {
            return XsdPath("sdp-felles.xsd");
        }

        private string MeldingXsdSchema()
        {
            return XsdPath("sdp-melding.xsd");
        }

        private string XsdPath(string filnavn)
        {
            return Path.Combine(TestDataMappe, "xsd", filnavn);
        }
    }
}
