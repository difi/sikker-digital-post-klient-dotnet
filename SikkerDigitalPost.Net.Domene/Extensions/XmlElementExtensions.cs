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
    }
}
