using System.Xml;

namespace Difi.SikkerDigitalPost.Klient.Domene.Extensions
{
    internal static class XmlElementExtensions
    {
        public static XmlElement AppendChildElement(this XmlElement parent, string childname, string namespaceUri, XmlDocument document)
        {
            var child = document.CreateElement(childname, namespaceUri);
            parent.AppendChild(child);
            return child;
        }

        public static XmlElement AppendChildElement(this XmlElement parent, string childname, string prefix, string namespaceUri, XmlDocument document)
        {
            var child = document.CreateElement(prefix, childname, namespaceUri);
            parent.AppendChild(child);
            return child;
        }

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