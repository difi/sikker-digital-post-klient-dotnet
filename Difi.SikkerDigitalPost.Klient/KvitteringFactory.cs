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
//        public static Kvittering GetKvittering(string xml)
//        {
//            var xmlDocument = new XmlDocument();
//            xmlDocument.LoadXml(xml);
//
//            return GetKvittering(xmlDocument);
//        }

        public static Kvittering GetKvittering(IntegrasjonspunktKvittering integrasjonspunktKvittering)
        {
            if (integrasjonspunktKvittering.status == IntegrasjonspunktKvitteringType.SENDT ||
                integrasjonspunktKvittering.status == IntegrasjonspunktKvitteringType.OPPRETTET)
            {
                return null;
            }
            
            if (integrasjonspunktKvittering.rawReceipt != null)
            {
                return GetKvitteringFromIntegrasjonsPunktKvittering(integrasjonspunktKvittering);
            } else if (integrasjonspunktKvittering.status == IntegrasjonspunktKvitteringType.LEVETID_UTLOPT)
            {
                return new Feilmelding(integrasjonspunktKvittering.messageId.ToString(), integrasjonspunktKvittering.conversationId, "", "");
            }
            else if (integrasjonspunktKvittering.status == IntegrasjonspunktKvitteringType.ANNET)
            {
                return new Feilmelding(integrasjonspunktKvittering.messageId.ToString(), integrasjonspunktKvittering.conversationId, "", "");
            }
            else
            {
                return new Feilmelding(integrasjonspunktKvittering.messageId.ToString(), integrasjonspunktKvittering.conversationId, "", "");
            }
        }

        private static Kvittering GetKvitteringFromIntegrasjonsPunktKvittering(IntegrasjonspunktKvittering integrasjonspunktKvittering)
        {
            var kvittering = (Kvittering) LagForretningskvittering(integrasjonspunktKvittering) ?? LagTransportkvittering(integrasjonspunktKvittering);

            if (kvittering != null)
                return kvittering;

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(integrasjonspunktKvittering.rawReceipt);
            
            var ingenKvitteringstypeFunnetException = new XmlParseException(
                "Klarte ikke å finne ut hvilken type Kvittering som ble tatt inn. Sjekk rådata for mer informasjon.")
            {
                Xml = xmlDocument
            };

            throw ingenKvitteringstypeFunnetException;
        }
        
//        public static Kvittering GetKvittering(XmlDocument xmlDocument)
//        {
//            var kvittering = (Kvittering) LagForretningskvittering(xmlDocument) ?? LagTransportkvittering(xmlDocument);
//
//            if (kvittering != null)
//                return kvittering;
//
//            var ingenKvitteringstypeFunnetException = new XmlParseException(
//                "Klarte ikke å finne ut hvilken type Kvittering som ble tatt inn. Sjekk rådata for mer informasjon.")
//            {
//                Xml = xmlDocument
//            };
//
//            throw ingenKvitteringstypeFunnetException;
//        }

        private static Forretningskvittering LagForretningskvittering(IntegrasjonspunktKvittering kvittering)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(kvittering.rawReceipt);
            
            if (IsLeveringskvittering(xmlDocument))
                return Kvitteringsparser.TilLeveringskvittering(kvittering);

            if (IsVarslingFeiletkvittering(xmlDocument))
                return Kvitteringsparser.TilVarslingFeiletKvittering(kvittering);

            if (IsFeilmelding(xmlDocument))
                return Kvitteringsparser.TilFeilmelding(kvittering);

            if (IsÅpningskvittering(xmlDocument))
                return Kvitteringsparser.TilÅpningskvittering(kvittering);

            if (IsMottaksKvittering(xmlDocument))
                return Kvitteringsparser.TilMottakskvittering(kvittering);

            if (IsReturpost(xmlDocument))
                return Kvitteringsparser.TilReturpostkvittering(kvittering);

            return null;
        }

        private static Transportkvittering LagTransportkvittering(IntegrasjonspunktKvittering kvittering)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(kvittering.rawReceipt);
            
            if (IsTransportOkKvittering(xmlDocument))
                return Kvitteringsparser.TilTransportOkKvittering(kvittering);

            if (IsTransportFeiletKvittering(xmlDocument))
                return Kvitteringsparser.TilTransportFeiletKvittering(kvittering);

            if (IsTomKøKvittering(xmlDocument))
                return Kvitteringsparser.TilTomKøKvittering(kvittering);

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