using System.Xml;

namespace SikkerDigitalPost.Net.KlientApi.Envelope.EnvelopeHeader
{
    class Security
    {
        private readonly XmlDocument _dokument;
        
        private readonly string wsse ="http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd";
        private readonly string wsu = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd";

        public Security(XmlDocument dokument)
        {
            _dokument = dokument;
        }

        public XmlElement Xml()
        {
            var securityElement = _dokument.CreateElement("wsse", "Security", wsse);
            securityElement.SetAttribute("xmlns:wsu", wsu);
            securityElement.AppendChild(BinarySecurityToken());
            return securityElement;
        }

        public XmlElement BinarySecurityToken()
        {
            var binarySecurityToken = _dokument.CreateElement("wsse", "BinarySecurityToken");
            binarySecurityToken.SetAttribute("xmlns", "wsse", wsse);
            return binarySecurityToken;
        }
    }
}
