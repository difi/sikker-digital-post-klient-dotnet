using System.Xml;
using SikkerDigitalPost.Net.Domene.Entiteter;
using SikkerDigitalPost.Net.KlientApi.Envelope.Body;

namespace SikkerDigitalPost.Net.KlientApi.Envelope.EnvelopeBody
{

    public class BodyElement : XmlPart
    {
        public BodyElement(XmlDocument dokument, Forsendelse forsendelse) : base(dokument, forsendelse)
        {
            
        }

        public override XmlElement Xml()
        {
            var bodyElement = XmlDocument.CreateElement("env", "body", Navnerom.NsXmlnsEnv);
            bodyElement.SetAttribute("xmlns:wsu", Navnerom.NsWsu);
            bodyElement.SetAttribute("id", Navnerom.NsWsu, Navnerom.NsWsuId);

            var sbdElement = new StandardBusinessDocument(XmlDocument, Forsendelse);
            bodyElement.AppendChild(sbdElement.Xml());
            
            return bodyElement;
        }
    }
}
