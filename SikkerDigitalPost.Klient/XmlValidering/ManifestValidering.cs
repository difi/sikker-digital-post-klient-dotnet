using System.Xml.Schema;
using SikkerDigitalPost.Klient.Envelope;

namespace SikkerDigitalPost.Klient.XmlValidering
{
    internal class ManifestValidering : Xmlvalidator
    {
        protected override XmlSchemaSet GenererSchemaSet()
        {
            var schemaSet = new XmlSchemaSet();

            schemaSet.Add(Navnerom.Ns9, SdpManifestXsdPath());
            schemaSet.Add(Navnerom.Ns9, FellesXsdPath());
            schemaSet.Add(Navnerom.Ns5, XmlDsigCoreXsdPath());


            return schemaSet;
        }
    }
}
