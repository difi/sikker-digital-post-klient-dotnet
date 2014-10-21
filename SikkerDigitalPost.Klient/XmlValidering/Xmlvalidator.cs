using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Schema;

namespace SikkerDigitalPost.Klient.XmlValidering
{
    internal abstract class Xmlvalidator
    {
        /// <summary>
        /// Denne strengen beskriver en feil som blir oppdaget ved xsd-validering av ws-security, men som vi ikke får gjort noe med.
        /// </summary>
        private const string ToleratedError = "It is an error if there is a member of the attribute uses of a type definition with type xs:ID or derived from xs:ID and another attribute with type xs:ID matches an attribute wildcard.";
        private const string ErrorToleratedPrefix = "The 'PrefixList' attribute is invalid - The value '' is invalid according to its datatype 'http://www.w3.org/2001/XMLSchema:NMTOKENS' - The attribute value cannot be empty.";
        private const string WarningMessage = "\tWarning: Matching schema not found. No validation occurred.";
        private const string ErrorMessage = "\tValidation error:";

        private static bool _harWarnings;
        private static bool _harErrors;

        private static string _validationMessages;

        public string ValideringsVarsler { get; private set; }

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
                    if (!e.Message.Equals(ToleratedError) && !e.Message.Equals(ErrorToleratedPrefix))
                        _harErrors = true;
                    else
                        _validationMessages +=
                            "Feilen over er ikke noe vi håndterer, og er heller ikke årsaken til at validering feilet\n";
                    break;
            }
        }

        protected XmlReader SoapEnvelopeXsdPath()
        {
            return XsdResource(@"w3.soap-envelope.xsd");
        }

        protected XmlReader EnvelopeXsdPath()
        {
            return XsdResource(@"xmlsoap.envelope.xsd");
        }

        protected XmlReader XmlXsdPath()
        {
            return XsdResource(@"w3.xml.xsd");
        }

        protected XmlReader ExecC14nXsdPath()
        {
            return XsdResource(@"w3.exc-c14n.xsd");
        }

        protected XmlReader EbmsHeaderXsdPath()
        {
            return XsdResource(@"ebxml.ebms-header-3_0-200704.xsd");
        }

        protected XmlReader XmlDsigCoreXsdPath()
        {
            return XsdResource(@"w3.xmldsig-core-schema.xsd");
        }

        protected XmlReader XmlXencXsdPath()
        {
            return XsdResource(@"w3.xenc-schema.xsd");
        }

        protected XmlReader FellesXsdPath()
        {
            return XsdResource("sdp-felles.xsd");
        }

        protected XmlReader MeldingXsdPath()
        {
            return XsdResource("sdp-melding.xsd");
        }

        protected XmlReader StandardBusinessDocumentHeaderXsdPath()
        {
            return XsdResource(@"SBDH20040506_02.StandardBusinessDocumentHeader.xsd");
        }

        protected XmlReader DocumentIdentificationXsdPath()
        {
            return XsdResource(@"SBDH20040506_02.DocumentIdentification.xsd");
        }

        protected XmlReader SbdhManifestXsdPath()
        {
            return XsdResource(@"SBDH20040506_02.Manifest.xsd");
        }

        protected XmlReader SdpManifestXsdPath()
        {
            return XsdResource("sdp-manifest.xsd");
        }

        protected XmlReader PartnerXsdPath()
        {
            return XsdResource(@"SBDH20040506_02.Partner.xsd");
        }

        protected XmlReader BusinessScopeXsdPath()
        {
            return XsdResource(@"SBDH20040506_02.BusinessScope.xsd");
        }

        protected XmlReader BasicTypesXsdPath()
        {
            return XsdResource(@"SBDH20040506_02.BasicTypes.xsd");
        }

        protected XmlReader WsSecurityUtilityXsdPath()
        {
            return XsdResource(@"wssecurity.oasis-200401-wss-wssecurity-utility-1.0.xsd");
        }

        protected XmlReader WsSecuritySecExt1_0XsdPath()
        {
            return XsdResource(@"wssecurity.oasis-200401-wss-wssecurity-secext-1.0.xsd");
        }

        protected XmlReader WsSecuritySecExt1_1XsdPath()
        {
            return XsdResource(@"wssecurity.oasis-wss-wssecurity-secext-1.1.xsd");
        }

        protected XmlReader AsicEXsdPath()
        {
            return XsdResource("w3.ts_102918v010201.xsd");
        }

        protected XmlReader XsdResource(string xsdResource)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = String.Format("{0}.{1}", "SikkerDigitalPost.Klient.XmlValidering.xsd", xsdResource);

            Stream stream = assembly.GetManifestResourceStream(resourceName);
            StreamReader streamReader = new StreamReader(stream);
            XmlReader xmlReader = XmlReader.Create(streamReader);

            return xmlReader;
        }

        protected XmlReader XmlDsigCoreSchema()
        {
            return XsdResource(@"w3.xmldsig-core-schema.xsd");
        }

        protected XmlReader EbmsSignalsXsdPath()
        {
            return XsdResource(@"ebxml.ebbp-signals-2.0.xsd");
        }

        protected XmlReader XlinkXsdPath()
        {
            return XsdResource(@"w3.xlink.xsd");
        }

        protected XmlReader XadesXsdPath()
        {
            return XsdResource(@"w3.XAdES.xsd");
        }
    }
}
