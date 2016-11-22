using System.Xml;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer.Forretning;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer.Transport;
using Difi.SikkerDigitalPost.Klient.Domene.Exceptions;
using Difi.SikkerDigitalPost.Klient.Utilities;

namespace Difi.SikkerDigitalPost.Klient
{
    public class KvitteringFactory
    {
        public static Kvittering GetKvittering(string xml)
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xml);

            return GetKvittering(xmlDocument);
        }

        public static Kvittering GetKvittering(XmlDocument xmlDocument)
        {
            var kvittering = (Kvittering) LagForretningskvittering(xmlDocument) ?? LagTransportkvittering(xmlDocument);

            if (kvittering != null)
                return kvittering;

            var ingenKvitteringstypeFunnetException = new XmlParseException(
                "Klarte ikke å finne ut hvilken type Kvittering som ble tatt inn. Sjekk rådata for mer informasjon.")
            {
                Xml = xmlDocument
            };

            throw ingenKvitteringstypeFunnetException;
        }

        private static Forretningskvittering LagForretningskvittering(XmlDocument xmlDocument)
        {
            if (IsLeveringskvittering(xmlDocument))
                return Kvitteringsparser.TilLeveringskvittering(xmlDocument);

            if (IsVarslingFeiletkvittering(xmlDocument))
                return Kvitteringsparser.TilVarslingFeiletKvittering(xmlDocument);

            if (IsFeilmelding(xmlDocument))
                return Kvitteringsparser.TilFeilmelding(xmlDocument);

            if (IsÅpningskvittering(xmlDocument))
                return Kvitteringsparser.TilÅpningskvittering(xmlDocument);

            if (IsMottaksKvittering(xmlDocument))
                return Kvitteringsparser.TilMottakskvittering(xmlDocument);

            if (IsReturpost(xmlDocument))
                return Kvitteringsparser.TilReturpostkvittering(xmlDocument);

            return null;
        }

        private static Transportkvittering LagTransportkvittering(XmlDocument xmlDocument)
        {
            if (IsTransportOkKvittering(xmlDocument))
                return Kvitteringsparser.TilTransportOkKvittering(xmlDocument);

            if (IsTransportFeiletKvittering(xmlDocument))
                return Kvitteringsparser.TilTransportFeiletKvittering(xmlDocument);

            if (IsTomKøKvittering(xmlDocument))
                return Kvitteringsparser.TilTomKøKvittering(xmlDocument);

            return null;
        }

        private static bool IsLeveringskvittering(XmlDocument document)
        {
            return DocumentHasNode(document, "ns9:levering");
        }

        private static bool IsVarslingFeiletkvittering(XmlDocument document)
        {
            return DocumentHasNode(document, "ns9:varslingfeilet");
        }

        private static bool IsFeilmelding(XmlDocument document)
        {
            return DocumentHasNode(document, "ns9:feil");
        }

        private static bool IsÅpningskvittering(XmlDocument document)
        {
            return DocumentHasNode(document, "ns9:aapning");
        }

        private static bool IsTomKøKvittering(XmlDocument document)
        {
            return DocumentHasNode(document, "ns6:Error[@shortDescription = 'EmptyMessagePartitionChannel']");
        }

        private static bool IsTransportOkKvittering(XmlDocument document)
        {
            return DocumentHasNode(document, "ns6:Receipt");
        }

        private static bool IsTransportFeiletKvittering(XmlDocument document)
        {
            return DocumentHasNode(document, "env:Fault");
        }

        private static bool IsMottaksKvittering(XmlDocument document)
        {
            return DocumentHasNode(document, "ns9:mottak");
        }

        private static bool IsReturpost(XmlDocument document)
        {
            return DocumentHasNode(document, "ns9:returpost");
        }

        private static bool DocumentHasNode(XmlDocument document, string node)
        {
            return DocumentNode(document, node) != null;
        }

        private static XmlNode DocumentNode(XmlDocument document, string node)
        {
            var rot = document.DocumentElement;
            string nodeString = $"//{node}";
            var targetNode = rot.SelectSingleNode(nodeString, NamespaceManager(document));

            return targetNode;
        }

        private static XmlNamespaceManager NamespaceManager(XmlDocument document)
        {
            var manager = new XmlNamespaceManager(document.NameTable);
            manager.AddNamespace("env", NavneromUtility.SoapEnvelopeEnv12);
            manager.AddNamespace("eb", NavneromUtility.EbXmlCore);
            manager.AddNamespace("ns3", NavneromUtility.StandardBusinessDocumentHeader);
            manager.AddNamespace("ns5", NavneromUtility.XmlDsig);
            manager.AddNamespace("ns6", NavneromUtility.EbXmlCore);
            manager.AddNamespace("ns9", NavneromUtility.DifiSdpSchema10);
            manager.AddNamespace("ds", NavneromUtility.XmlDsig);
            return manager;
        }
    }
}