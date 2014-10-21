using System.Xml.Schema;

namespace SikkerDigitalPost.Klient.XmlValidering
{
    internal class KvitteringMottattEnvelopeValidering : Xmlvalidator
    {
        protected override XmlSchemaSet GenererSchemaSet()
        {
            var schemaSet = new XmlSchemaSet();

            schemaSet.Add(Navnerom.env, SoapEnvelopeXsdPath());
            schemaSet.Add(Navnerom.Ns4, EnvelopeXsdPath());

            schemaSet.Add(Navnerom.Ns6, EbmsHeaderXsdPath());
            schemaSet.Add(Navnerom.Ns7, EbmsSignalsXsdPath());

            schemaSet.Add(Navnerom.Ns5, XmlDsigCoreXsdPath());
            schemaSet.Add(Navnerom.enc, XmlXencXsdPath());
            schemaSet.Add(Navnerom.Ns8, XlinkXsdPath());
            schemaSet.Add("http://www.w3.org/XML/1998/namespace", XmlXsdPath());
            schemaSet.Add("http://www.w3.org/2001/10/xml-exc-c14n#", ExecC14nXsdPath());

            schemaSet.Add(Navnerom.wsu, WsSecurityUtilityXsdPath());
            schemaSet.Add(Navnerom.wsse, WsSecuritySecExt1_0XsdPath());

            return schemaSet;
        }
    }
}
