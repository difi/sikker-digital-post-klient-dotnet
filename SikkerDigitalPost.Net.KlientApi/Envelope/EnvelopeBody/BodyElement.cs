using System.Xml;
using SikkerDigitalPost.Net.Domene.Entiteter;
using SikkerDigitalPost.Net.KlientApi.Envelope.Body;

namespace SikkerDigitalPost.Net.KlientApi.Envelope.EnvelopeBody
{
    public class BodyElement
    {
        private readonly XmlDocument _dokument;
        private readonly Forsendelse _forsendelse;

        private const string NsXmlnsEnv = "http://www.w3.org/2003/05/soap-envelope";
        private const string NsWsu = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd";
        private const string NsWsuId = "";

        public BodyElement(XmlDocument dokument, Forsendelse forsendelse)
        {
            _forsendelse = forsendelse;
            _dokument = dokument;
        }

        public XmlElement Xml()
        {
            var bodyElement = _dokument.CreateElement("env", "body", NsXmlnsEnv);
            bodyElement.SetAttribute("xmlns:wsu", NsWsu);
            bodyElement.SetAttribute("id", NsWsu, NsWsuId);

            var sbdElement = new StandardBusinessDocument(_dokument, _forsendelse);
            bodyElement.AppendChild(sbdElement.Xml());
            
            return bodyElement;
        }
    }
}
