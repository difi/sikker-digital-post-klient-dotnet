using System;
using System.Xml;
using SikkerDigitalPost.Domene.Enums;
using SikkerDigitalPost.Domene.Exceptions;

namespace SikkerDigitalPost.Domene.Entiteter.Kvitteringer
{
    public class TransportFeiletKvittering : Transportkvittering
    {
        public readonly string Feilkode;

        public readonly string Kategori;

        public readonly string Opprinnelse;

        public readonly string Alvorlighetsgrad;

        public readonly string Beskrivelse;

        public readonly Feiltype Skyldig;
        
        internal TransportFeiletKvittering(XmlDocument document, XmlNamespaceManager namespaceManager) : base(document, namespaceManager)
        {
            try{
                var errorNode = DocumentNode("//ns6:Error");
                Feilkode = errorNode.Attributes["errorCode"].Value;
                Kategori = errorNode.Attributes["category"].Value;
                Opprinnelse = errorNode.Attributes["origin"].Value;
                Alvorlighetsgrad = errorNode.Attributes["severity"].Value;
                Beskrivelse = DocumentNode("//ns6:Description").InnerText;
                var skyldig = DocumentNode("//env:Value").InnerText;
                Skyldig = skyldig == Feiltype.Klient.ToString()
                    ? Feiltype.Klient
                    : Feiltype.Server;
            }
            catch (Exception e)
            {
                throw new XmlParseException(
                    "Feil under bygging av TransportFeilet-kvittering. Klarte ikke finne alle felter i xml.", e);
            }
        }


    }
}
