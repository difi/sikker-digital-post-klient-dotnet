using System.Xml;

namespace Difi.SikkerDigitalPost.Klient.Resources.Xml
{
    public class XmlUtility
    {
        public static XmlDocument ToXmlDocument(string kvittering)
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(kvittering);

            return xmlDocument;
        }
    }
}