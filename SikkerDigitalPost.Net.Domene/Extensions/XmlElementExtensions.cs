using System.Xml;

namespace SikkerDigitalPost.Net.Domene.Extensions
{
    public static class XmlElementExtensions
    {
        public static XmlElement AppendChildElement(this XmlElement parent, string childname, string nameSpace, XmlDocument document)
        {
            var child = document.CreateElement(childname, nameSpace);
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
