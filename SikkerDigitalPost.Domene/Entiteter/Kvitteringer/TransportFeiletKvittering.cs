using System;
using System.Xml;
using SikkerDigitalPost.Domene.Enums;
using SikkerDigitalPost.Domene.Exceptions;

namespace SikkerDigitalPost.Domene.Entiteter.Kvitteringer
{
    /// <summary>
    /// Transportkvittering som indikerer at noe har gått galt ved sending av en melding. 
    /// </summary>
    public class TransportFeiletKvittering : Transportkvittering
    {
        public string Feilkode { get; private set; }

        public string Kategori { get; private set; }

        public string Opprinnelse { get; private set; }

        public string Alvorlighetsgrad { get; private set; }

        public string Beskrivelse { get; private set; }

        public Feiltype Skyldig { get; private set; }
        
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
