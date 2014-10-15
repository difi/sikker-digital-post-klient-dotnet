using System;
using System.Xml;
using SikkerDigitalPost.Domene.Enums;

namespace SikkerDigitalPost.Domene.Entiteter.Kvitteringer
{
    public class Feilmelding : Forretningskvittering
    {
        public readonly Feiltype Feiltype;

        public string Detaljer { get; set; }

        public readonly DateTime TidspunktFeilet;

        internal Feilmelding(XmlDocument xmlDocument, XmlNamespaceManager namespaceManager):base(xmlDocument,namespaceManager)
        {
            TidspunktFeilet = Convert.ToDateTime(DocumentNode("//ns9:tidspunkt").InnerText);
            
            var feiltype = DocumentNode("//ns9:feiltype").InnerText;
            Feiltype = feiltype.ToLower().Equals(Feiltype.Klient.ToString().ToLower()) 
                ? Feiltype.Klient 
                : Feiltype.Server;

            Detaljer = DocumentNode("//ns9:detaljer").InnerText;
        }
    }
}
