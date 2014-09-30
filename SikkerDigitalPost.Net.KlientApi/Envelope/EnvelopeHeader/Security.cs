using System.Xml;

namespace SikkerDigitalPost.Net.KlientApi.Envelope.EnvelopeHeader
{
    class Security
    {
        private readonly XmlDocument _dokument;

        public Security(XmlDocument dokument)
        {
            _dokument = dokument;
        }

        public XmlElement Xml()
        {
            var securityElement = _dokument.CreateElement("wsse", "Security", Navnerom.wsse);
            securityElement.SetAttribute("xmlns:wsu", Navnerom.wsu);
            securityElement.AppendChild(BinarySecurityToken());
            return securityElement;
        }

        public XmlElement BinarySecurityToken()
        {
            var binarySecurityToken = _dokument.CreateElement("wsse", "BinarySecurityToken", Navnerom.wsse);
            binarySecurityToken.SetAttribute("id", Navnerom.wsu, "ID_SKAL_SETTES_INN_HER");
            return binarySecurityToken;
        }
    }
}
