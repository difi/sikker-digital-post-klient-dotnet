using System;
using System.Xml;
using SikkerDigitalPost.Domene.Enums;

namespace SikkerDigitalPost.Domene.Entiteter.Kvitteringer
{
    public class Feilmelding : Kvittering
    {
        public readonly Feiltype Feiltype;
        public string Feilkode { get; set; }
        public string Detaljer { get; set; }

        public Feilmelding(DateTime tidspunkt, Feiltype feiltype)
        {
            Tidspunkt = tidspunkt;
            Feiltype = feiltype;
        }

        internal Feilmelding(XmlDocument xmlDocument, XmlNamespaceManager namespaceManager)
        {
            Tidspunkt = Convert.ToDateTime(DocumentNode(xmlDocument, namespaceManager, "//ns6:Timestamp").InnerText);
            var feiltype = DocumentNode(xmlDocument, namespaceManager, "//env:Value").InnerText;
            
            Feiltype = feiltype.ToLower().Equals(Feiltype.Klient.ToString().ToLower()) 
                ? Feiltype.Klient 
                : Feiltype.Server;

            Feilkode = DocumentNode(xmlDocument, namespaceManager, "//ns6:Error").Attributes["errorCode"].Value;

            Detaljer = DocumentNode(xmlDocument, namespaceManager, "//ns6:Description").InnerText;
        }
    }
}
