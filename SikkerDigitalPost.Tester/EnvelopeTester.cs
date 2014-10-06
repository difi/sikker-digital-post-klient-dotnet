using System;
using System.IO;
using System.Xml;
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

        [TestMethod]
        public void ValidereEnvelopeMotXsdValiderer()
        {
            var settings = new XmlReaderSettings();
            settings.XmlResolver = null;
            settings.Schemas.Add(Navnerom.Ns9, MeldingXsdSchema());
            settings.Schemas.Add(Navnerom.Ns9, FellesXsdSchema());
            settings.Schemas.Add(Navnerom.enc, XmlXencSchema());
            settings.Schemas.Add(Navnerom.Ns5, XmlDsigCoreSchema());
            settings.ValidationType = ValidationType.Schema;

            try
            {
                var reader = XmlReader.Create(new MemoryStream(Envelope.Bytes), settings);
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
