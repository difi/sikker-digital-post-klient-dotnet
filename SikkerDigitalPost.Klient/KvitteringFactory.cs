using System;
using System.Xml;
using SikkerDigitalPost.Domene.Entiteter.Kvitteringer;
using SikkerDigitalPost.Klient.Envelope;

namespace SikkerDigitalPost.Klient
{
    internal class KvitteringFactory
    {

        public static Kvittering Get(string xml)
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xml);

            if (IsLevertkvittering(xmlDocument))
            {

            }
            else if (IsFeiletkvittering(xmlDocument))
            {

            }
            else if (IsÅpningskvittering(xmlDocument))
            {

            }
            return null;

        }

        private static bool IsLevertkvittering(XmlDocument document)
        {
            return DocumentHasNode(document, "ns6:Receipt");
        }

        private static bool IsFeiletkvittering(XmlDocument document)
        {
            return DocumentHasNode(document, "env:Fault");
        }

        private static bool IsÅpningskvittering(XmlDocument document)
        {
            return DocumentHasNode(document, "kvittering");
        }

        private static bool DocumentHasNode(XmlDocument document, string node)
        {
            var rot = document.DocumentElement;
            string nodeString = String.Format("//{0}", node);
            var receiptNode = rot.SelectSingleNode(nodeString, NamespaceManager(document));

            return receiptNode != null;
        }


        private static XmlNamespaceManager NamespaceManager(XmlDocument document)
        {
            XmlNamespaceManager manager = new XmlNamespaceManager(document.NameTable);
            manager.AddNamespace("env", Navnerom.env);
            manager.AddNamespace("eb", Navnerom.eb);
            manager.AddNamespace("ns6", Navnerom.Ns6);

            //Endre
            manager.AddNamespace("env", Navnerom.env);
            manager.AddNamespace("env", Navnerom.env);
            manager.AddNamespace("env", Navnerom.env);
            manager.AddNamespace("env", Navnerom.env);

            return manager;
        }






    }
}
