using System.IO;
using System.Xml;
using ApiClientShared;
using Difi.Felles.Utility;
using Difi.SikkerDigitalPost.Klient.Utilities;

namespace Difi.SikkerDigitalPost.Klient.XmlValidering
{
    internal class ForretningsmeldingEnvelopeValidator : XmlValidator
    {
        private static readonly ResourceUtility ResourceUtility = new ResourceUtility("Difi.SikkerDigitalPost.Klient.XmlValidering.xsd");

        public ForretningsmeldingEnvelopeValidator()
        {
            AddXsd(NavneromUtility.SoapEnvelopeEnv12, GetResource("w3.soap-envelope.xsd"));
            AddXsd(NavneromUtility.SoapEnvelope, GetResource("xmlsoap.envelope.xsd"));
            AddXsd(NavneromUtility.EbXmlCore, GetResource("ebxml.ebms-header-3_0-200704.xsd"));
            AddXsd(NavneromUtility.DifiSdpSchema10, GetResource("sdp-felles.xsd"));
            AddXsd(NavneromUtility.DifiSdpSchema10, GetResource("sdp-melding.xsd"));
            AddXsd(NavneromUtility.XmlDsig, GetResource("w3.xmldsig-core-schema.xsd"));
            AddXsd(NavneromUtility.Xml1998, GetResource("w3.xml.xsd"));
            AddXsd(NavneromUtility.XmlExcC14N, GetResource("w3.exc-c14n.xsd"));

            AddXsd(NavneromUtility.StandardBusinessDocumentHeader, GetResource("SBDH20040506_02.StandardBusinessDocumentHeader.xsd"));
            AddXsd(NavneromUtility.StandardBusinessDocumentHeader, GetResource("SBDH20040506_02.DocumentIdentification.xsd"));
            AddXsd(NavneromUtility.StandardBusinessDocumentHeader, GetResource("SBDH20040506_02.Manifest.xsd"));
            AddXsd(NavneromUtility.StandardBusinessDocumentHeader, GetResource("SBDH20040506_02.Partner.xsd"));
            AddXsd(NavneromUtility.StandardBusinessDocumentHeader, GetResource("SBDH20040506_02.BusinessScope.xsd"));
            AddXsd(NavneromUtility.StandardBusinessDocumentHeader, GetResource("SBDH20040506_02.BasicTypes.xsd"));

            AddXsd(NavneromUtility.WssecurityUtility10, GetResource("wssecurity.oasis-200401-wss-wssecurity-utility-1.0.xsd"));
            AddXsd(NavneromUtility.WssecuritySecext10, GetResource("wssecurity.oasis-200401-wss-wssecurity-secext-1.0.xsd"));
        }

        private XmlReader GetResource(string path)
        {
            var bytes = ResourceUtility.ReadAllBytes(true, path);
            return XmlReader.Create(new MemoryStream(bytes));
        }
    }
}