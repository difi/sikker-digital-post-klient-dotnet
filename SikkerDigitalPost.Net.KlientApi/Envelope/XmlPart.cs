using System.Xml;

namespace SikkerDigitalPost.Net.KlientApi.Envelope
{
    public abstract class XmlPart
    {
        protected readonly XmlDocument XmlDocument;

        protected XmlPart(XmlDocument xmlDocument)
        {
            XmlDocument = xmlDocument;
        }

        public abstract XmlElement Xml();

    }
}
