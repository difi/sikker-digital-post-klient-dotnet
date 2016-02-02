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

        public ForretningsmeldingEnvelopeValidator(){
            LeggTilXsdRessurs(NavneromUtility.SoapEnvelopeEnv12, HentRessurs("w3.soap-envelope.xsd"));
            LeggTilXsdRessurs(NavneromUtility.SoapEnvelope, HentRessurs("xmlsoap.envelope.xsd"));
            LeggTilXsdRessurs(NavneromUtility.EbXmlCore, HentRessurs("ebxml.ebms-header-3_0-200704.xsd"));
            LeggTilXsdRessurs(NavneromUtility.DifiSdpSchema10, HentRessurs("sdp-felles.xsd"));
            LeggTilXsdRessurs(NavneromUtility.DifiSdpSchema10, HentRessurs("sdp-melding.xsd"));
            LeggTilXsdRessurs(NavneromUtility.XmlDsig, HentRessurs("w3.xmldsig-core-schema.xsd"));
            LeggTilXsdRessurs(NavneromUtility.Xml1998, HentRessurs("w3.xml.xsd"));
            LeggTilXsdRessurs(NavneromUtility.XmlExcC14N, HentRessurs("w3.exc-c14n.xsd"));

            LeggTilXsdRessurs(NavneromUtility.StandardBusinessDocumentHeader, HentRessurs("SBDH20040506_02.StandardBusinessDocumentHeader.xsd"));
            LeggTilXsdRessurs(NavneromUtility.StandardBusinessDocumentHeader, HentRessurs("SBDH20040506_02.DocumentIdentification.xsd"));
            LeggTilXsdRessurs(NavneromUtility.StandardBusinessDocumentHeader, HentRessurs("SBDH20040506_02.Manifest.xsd"));
            LeggTilXsdRessurs(NavneromUtility.StandardBusinessDocumentHeader, HentRessurs("SBDH20040506_02.Partner.xsd"));
            LeggTilXsdRessurs(NavneromUtility.StandardBusinessDocumentHeader, HentRessurs("SBDH20040506_02.BusinessScope.xsd"));
            LeggTilXsdRessurs(NavneromUtility.StandardBusinessDocumentHeader, HentRessurs("SBDH20040506_02.BasicTypes.xsd"));

            LeggTilXsdRessurs(NavneromUtility.WssecurityUtility10, HentRessurs("wssecurity.oasis-200401-wss-wssecurity-utility-1.0.xsd"));
            LeggTilXsdRessurs(NavneromUtility.WssecuritySecext10, HentRessurs("wssecurity.oasis-200401-wss-wssecurity-secext-1.0.xsd"));
         
        }

        private XmlReader HentRessurs(string path)
        {
            var bytes = ResourceUtility.ReadAllBytes(true, path);
            return XmlReader.Create(new MemoryStream(bytes));
        }
    }
}
