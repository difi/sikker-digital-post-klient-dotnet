using System.Xml.Schema;
using SikkerDigitalPost.Klient.Envelope;

namespace SikkerDigitalPost.Klient.XmlValidering
{
    internal class SignaturValidering : XmlValidering
    {
        protected override XmlSchemaSet GenererSchemaSet()
        {
            var schemaSet = new XmlSchemaSet();

            schemaSet.Add(Navnerom.Ns10, AsicEXsdPath());
            schemaSet.Add(Navnerom.Ns11, XAdESXsdPath());
            schemaSet.Add(Navnerom.Ns5, XmlDsigCoreSchema());

            return schemaSet;
        }
    }
}
