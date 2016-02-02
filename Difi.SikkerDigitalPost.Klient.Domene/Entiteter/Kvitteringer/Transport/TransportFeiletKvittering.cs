using System;
using System.Xml;
using Difi.SikkerDigitalPost.Klient.Domene.Enums;
using Difi.SikkerDigitalPost.Klient.Domene.Exceptions;

namespace Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer.Transport
{
    /// <summary>
    /// Transportkvittering som indikerer at noe har gått galt ved sending av en melding. 
    /// </summary>
    public class TransportFeiletKvittering : Transportkvittering
    {
        /// <summary>
        /// Spesifikk feilkode for transporten.
        /// </summary>
        public string Feilkode { get; set; }

        /// <summary>
        /// Kategori/id for hvilken feil som oppstod.
        /// </summary>
        public string Kategori { get; set; }

        /// <summary>
        /// Opprinnelse for feilmeldingen.
        /// </summary>
        public string Opprinnelse { get; set; }

        /// <summary>
        /// Hvor alvorlig er feilen som oppstod
        /// </summary>
        public string Alvorlighetsgrad { get; set; }

        /// <summary>
        /// En mer detaljert beskrivelse av hva som gikk galt.
        /// </summary>
        public string Beskrivelse { get; set; }

        /// <summary>
        /// Hvem man antar har skyld i feilen.
        /// </summary>
        public Feiltype Skyldig { get; protected set; }

        public TransportFeiletKvittering()
        {
        }

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

        public new string ToString()
        {
            return String.Format("{0} med meldingsId {1}: \nTidspunkt: {2}. \nRefererer til melding med id: {3}",
                GetType().Name, MeldingsId, SendtTidspunkt, ReferanseTilMeldingId);
        }
    }
}
