using System.Xml;
using SikkerDigitalPost.Net.Domene.Entiteter;
using SikkerDigitalPost.Net.KlientApi.Envelope.Body;

namespace SikkerDigitalPost.Net.KlientApi.Envelope.EnvelopeBody
{

    public class BodyElement : XmlPart
    {
        public BodyElement(XmlDocument dokument, Forsendelse forsendelse, AsicEArkiv asicEArkiv, Databehandler databehandler) : base(dokument, forsendelse, asicEArkiv, databehandler)
        {
            
        }

        public override XmlElement Xml()
        {
            var bodyElement = XmlDocument.CreateElement("env", "body", Navnerom.XmlnsEnv);
            bodyElement.SetAttribute("xmlns:wsu", Navnerom.wsu);
            bodyElement.SetAttribute("id", Navnerom.wsu, Navnerom.WsuId);

            var sbdElement = new StandardBusinessDocument(XmlDocument, Forsendelse, AsicEArkiv, Databehandler);
            bodyElement.AppendChild(sbdElement.Xml());
            
            return bodyElement;
        }
    }
}
