using System.Xml;

namespace Difi.SikkerDigitalPost.Klient.Tester.Utilities
{
    internal static class XmlUtility
    {
        public static XmlDocument TilXmlDokument(string xml)
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xml);

            return xmlDocument;
        }
    }
}
