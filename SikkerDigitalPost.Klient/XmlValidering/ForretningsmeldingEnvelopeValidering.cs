using System.Xml.Schema;
using SikkerDigitalPost.Klient.Envelope;

namespace SikkerDigitalPost.Klient.XmlValidering
{
    internal class ForretningsmeldingEnvelopeValidering : Xmlvalidator
    {
        protected override XmlSchemaSet GenererSchemaSet()
        {
            var schemaSet = new XmlSchemaSet();

            schemaSet.Add(Navnerom.env, SoapEnvelopeXsdPath());
            schemaSet.Add(Navnerom.Ns4, EnvelopeXsdPath());

            schemaSet.Add(Navnerom.Ns6, EbmsHeaderXsdPath());

            schemaSet.Add(Navnerom.Ns9, FellesXsdPath());
            schemaSet.Add(Navnerom.Ns9, MeldingXsdPath());

            schemaSet.Add(Navnerom.Ns5, XmlDsigCoreXsdPath());
            //schemaSet.Add(Navnerom.enc, XmlXencXsdPath());
            schemaSet.Add("http://www.w3.org/XML/1998/namespace", XmlXsdPath());
            schemaSet.Add("http://www.w3.org/2001/10/xml-exc-c14n#", ExecC14nXsdPath());

            schemaSet.Add(Navnerom.Ns3, StandardBusinessDocumentHeaderXsdPath());
            schemaSet.Add(Navnerom.Ns3, DocumentIdentificationXsdPath());
            schemaSet.Add(Navnerom.Ns3, SbdhManifestXsdPath());
            schemaSet.Add(Navnerom.Ns3, PartnerXsdPath());
            schemaSet.Add(Navnerom.Ns3, BusinessScopeXsdPath());
            schemaSet.Add(Navnerom.Ns3, BasicTypesXsdPath());

            schemaSet.Add(Navnerom.wsu, WsSecurityUtilityXsdPath());
            schemaSet.Add(Navnerom.wsse, WsSecuritySecExt1_0XsdPath());

            return schemaSet;
        }
    }
}