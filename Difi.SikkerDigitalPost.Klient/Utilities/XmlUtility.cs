using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
