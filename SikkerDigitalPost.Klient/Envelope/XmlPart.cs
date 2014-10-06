using System.Xml;

namespace SikkerDigitalPost.Klient.Envelope
{
    internal abstract class XmlPart
    {
        protected readonly Envelope Rot;
        
        protected XmlPart(Envelope rot)
        {
            Rot = rot;
        }

        public abstract XmlElement Xml();

    }
}
