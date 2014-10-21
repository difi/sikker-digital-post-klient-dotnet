using System;
using System.IO;
using System.Reflection;
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

        public string ValideringsVarsler { get; private set; }

        protected abstract XmlSchemaSet GenererSchemaSet();

        public bool ValiderDokumentMotXsd(string document)
        {
            //XmlValideringstest
            var assembly = Assembly.GetExecutingAssembly();
            var names = assembly.GetManifestResourceNames();

            string[] resourceNames = assembly.GetManifestResourceNames();
            foreach (string resourceName in resourceNames)
            {
                System.Diagnostics.Trace.WriteLine(resourceName);
            }

            //Slutt på test

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

        protected XmlReader SoapEnvelopeXsdPath()
        {
            return XsdResource(@"w3.soap-envelope.xsd");
        }

        //
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
        //
        //
        protected XmlReader FellesXsdPath()
        {
            return XsdResource("sdp-felles.xsd");
        }

        //
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

        protected XmlReader SBDHManifestXsdPath()
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

        //
        //

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
            var resourceName = String.Format("{0}.{1}", "SikkerDigitalPost.Klient.XmlValidering.xsd",xsdResource);

            Stream stream = assembly.GetManifestResourceStream(resourceName);
            StreamReader reader = new StreamReader(stream);
            XmlReader reader2 = XmlReader.Create(reader);

            return reader2;
        }

        protected XmlReader XmlDsigCoreSchema()
        {
            return XsdResource(@"w3.xmldsig-core-schema.xsd");
        }

        protected XmlReader XAdESXsdPath()
        {
            return XsdResource(@"w3.XAdES.xsd");
        }

        private static string XsdPath(string filnavn)
        {
            var absoluteXsdSource = Path.GetFullPath(XsdMappe);
            string xsdPath = Path.Combine(absoluteXsdSource, filnavn);
            return xsdPath;
        }
    }
}
