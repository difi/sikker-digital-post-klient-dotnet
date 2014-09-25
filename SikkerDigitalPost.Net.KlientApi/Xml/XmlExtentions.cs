using System.Xml;

namespace SikkerDigitalPost.Net.KlientApi.Xml
{
    internal static class XmlExtentions
    {
        public static XmlElement AppendChild(this XmlElement element, string localName, string namespaceUri, string value = null)
        {
            var newElement = element.OwnerDocument.CreateElement(null, localName, namespaceUri);
            element.AppendChild(newElement);
            if (value != null)
                newElement.InnerText = value;

            return newElement;
        }
    }
}
