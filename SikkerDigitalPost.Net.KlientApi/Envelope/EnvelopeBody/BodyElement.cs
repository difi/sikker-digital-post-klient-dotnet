using System.Xml;

namespace SikkerDigitalPost.Net.KlientApi.Envelope.EnvelopeBody
{
    public class BodyElement
    {
        private XmlElement _bodyElement;

        public BodyElement()
        {
            
        }

        public void Xml(ref XmlElement bodyElement)
        {
            _bodyElement = bodyElement;
        }
    }
}
