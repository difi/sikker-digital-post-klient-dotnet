using System.Xml;

namespace Difi.SikkerDigitalPost.Klient.Utilities
{
    public class XmlUtility
    {
        public static XmlDocument TilXmlDokument(string kvittering)
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(kvittering);

            return xmlDocument;
        }
    }
}