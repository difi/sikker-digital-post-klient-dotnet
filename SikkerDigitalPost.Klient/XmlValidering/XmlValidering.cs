using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Schema;

namespace SikkerDigitalPost.Klient.XmlValidering
{
    internal abstract class XmlValidering
    {
        /// <summary>
        /// Denne strengen beskriver en feil som blir oppdaget ved xsd-validering av ws-security, men som vi ikke får gjort noe med.
        /// </summary>
        private const string ErrorTolerated = "It is an error if there is a member of the attribute uses of a type definition with type xs:ID or derived from xs:ID and another attribute with type xs:ID matches an attribute wildcard.";
        private const string WarningMessage = "\tWarning: Matching schema not found. No validation occurred.";
        private const string ErrorMessage = "\tValidation error:";
        private const string XsdMappe = @"../../../SikkerDigitalPost.Klient/XmlValidering/xsd";
        
        private static bool _harWarnings;
        private static bool _harErrors;

        private static string _validationMessages;
        public string ValideringsVarsler { get; set; }

        protected abstract XmlSchemaSet GenererSchemaSet();

        public bool ValiderDokumentMotXsd(string document)
        {
            var settings = new XmlReaderSettings();
            settings.Schemas.Add(GenererSchemaSet());
            settings.ValidationType = ValidationType.Schema;
            settings.ValidationFlags = XmlSchemaValidationFlags.ReportValidationWarnings;
            settings.ValidationEventHandler += ValidationEventHandler;
            
            var xmlReader = XmlReader.Create(new MemoryStream(Encoding.UTF8.GetBytes(document)), settings);

            while (xmlReader.Read()) { }
            ValideringsVarsler = _validationMessages;

            return _harErrors == false && _harWarnings == false;
        }

        private static void ValidationEventHandler(object sender, ValidationEventArgs e)
        {
            switch (e.Severity)
            {
                case XmlSeverityType.Warning:
                    _validationMessages += String.Format("{0} {1}\n", WarningMessage, e.Message);
                    _harWarnings = true;
                    break;
                case XmlSeverityType.Error:
                    _validationMessages += String.Format("{0} {1}\n", ErrorMessage, e.Message);
                    if (!e.Message.Equals(ErrorTolerated))
                        _harErrors = true;
                    else
                        _validationMessages +=
                            "Feilen over er ikke noe vi håndterer, og er heller ikke årsaken til at validering feilet";
                    break;
            }
        }

        protected string SoapEnvelopeXsdPath()
        {
            return XsdPath(@"w3/soap-envelope.xsd");
        }

        protected string EnvelopeXsdPath()
        {
            return XsdPath(@"xmlsoap/envelope.xsd");
        }

        protected string XmlXsdPath()
        {
            return XsdPath(@"w3/xml.xsd");
        }

        protected string ExecC14nXsdPath()
        {
            return XsdPath(@"w3/exc-c14n.xsd");
        }

        protected string EbmsHeaderXsdPath()
        {
            return XsdPath(@"ebxml/ebms-header-3_0-200704.xsd");
        }

        protected string XmlDsigCoreXsdPath()
        {
            return XsdPath(@"w3/xmldsig-core-schema.xsd");
        }

        protected string XmlXencXsdPath()
        {
            return XsdPath(@"w3/xenc-schema.xsd");
        }

        protected string FellesXsdPath()
        {
            return XsdPath("sdp-felles.xsd");
        }

        protected string MeldingXsdPath()
        {
            return XsdPath("sdp-melding.xsd");
        }

        protected string StandardBusinessDocumentHeaderXsdPath()
        {
            return XsdPath(@"SBDH20040506-02/StandardBusinessDocumentHeader.xsd");
        }

        protected string DocumentIdentificationXsdPath()
        {
            return XsdPath(@"SBDH20040506-02/DocumentIdentification.xsd");
        }

        protected string SBDHManifestXsdPath()
        {
            return XsdPath(@"SBDH20040506-02/Manifest.xsd");
        }

        protected string SdpManifestXsdPath()
        {
            return XsdPath("sdp-manifest.xsd");
        }

        protected string PartnerXsdPath()
        {
            return XsdPath(@"SBDH20040506-02/Partner.xsd");
        }

        protected string BusinessScopeXsdPath()
        {
            return XsdPath(@"SBDH20040506-02/BusinessScope.xsd");
        }

        protected string BasicTypesXsdPath()
        {
            return XsdPath(@"SBDH20040506-02/BasicTypes.xsd");
        }

        protected string WsSecurityUtilityXsdPath()
        {
            return XsdPath(@"wssecurity/oasis-200401-wss-wssecurity-utility-1.0.xsd");
        }

        protected string WsSecuritySecExt1_0XsdPath()
        {
            return XsdPath(@"wssecurity/oasis-200401-wss-wssecurity-secext-1.0.xsd");
        }

        protected string WsSecuritySecExt1_1XsdPath()
        {
            return XsdPath(@"wssecurity/oasis-wss-wssecurity-secext-1.1.xsd");
        }

        protected string AsicEXsdPath()
        {
            return XsdPath(@"w3/ts_102918v010201.xsd");
        }

        protected string XmlDsigCoreSchema()
        {
            return XsdPath(@"w3/xmldsig-core-schema.xsd");
        }

        protected string XAdESXsdPath()
        {
            return XsdPath(@"w3/XAdES.xsd");
        }

        private static string XsdPath(string filnavn)
        {
            var absoluteXsdSource = Path.GetFullPath(XsdMappe);
            string xsdPath = Path.Combine(absoluteXsdSource, filnavn);
            return xsdPath;
        }
    }
}
