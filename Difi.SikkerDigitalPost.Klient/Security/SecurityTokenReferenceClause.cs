using System.Security.Cryptography.Xml;
using System.Xml;

namespace Difi.SikkerDigitalPost.Klient.Security
{
    internal class SecurityTokenReferenceClause : KeyInfoClause
    {
        public SecurityTokenReferenceClause(string uri)
        {
            Uri = uri;
        }

        public string Uri { get; set; }

        public override XmlElement GetXml()
        {
            return GetXml(new XmlDocument {PreserveWhitespace = true});
        }

        private XmlElement GetXml(XmlDocument xmlDocument)
        {
            var element1 = xmlDocument.CreateElement("SecurityTokenReference", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd");
            var element2 = xmlDocument.CreateElement("Reference", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd");
            element2.SetAttribute("URI", Uri);
            element2.SetAttribute("ValueType", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-x509-token-profile-1.0#X509v3");
            element1.AppendChild(element2);
            return element1;
        }

        public override void LoadXml(XmlElement element)
        {
        }
    }
}