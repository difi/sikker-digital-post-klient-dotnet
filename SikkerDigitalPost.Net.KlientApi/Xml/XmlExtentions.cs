using System.Xml;

namespace SikkerDigitalPost.Net.Klient.Xml
{
    internal static class XmlExtentions
    {
        public static XmlElement AppendChild(this XmlElement element, string localName, string namespaceURI, string value = null)
        {
            var newElement = element.OwnerDocument.CreateElement(null, localName, namespaceURI);
            element.AppendChild(newElement);
            if (value != null)
                newElement.InnerText = value;

            return newElement;
        }

    }
}
