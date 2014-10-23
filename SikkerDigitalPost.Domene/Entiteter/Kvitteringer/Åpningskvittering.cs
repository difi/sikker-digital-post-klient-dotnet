using System;
using System.Xml;
using SikkerDigitalPost.Domene.Exceptions;

namespace SikkerDigitalPost.Domene.Entiteter.Kvitteringer
{
    /// <summary>
    /// En kvitteringsmelding til Avsender om at Mottaker har åpnet forsendelsen i sin postkasse.
    /// Mer informasjon finnes på http://begrep.difi.no/SikkerDigitalPost/1.0.2/meldinger/AapningsKvittering.
    /// </summary>
    public class Åpningskvittering : Forretningskvittering
    {
        /// <summary>
        /// Tidspunkt for når borger åpnet posten i sin postkasse.
        /// </summary>
        public DateTime Åpningstidspunkt { get; private set; }

        internal Åpningskvittering(XmlDocument xmlDocument, XmlNamespaceManager namespaceManager):base(xmlDocument,namespaceManager)
        {
            try
            {
                Åpningstidspunkt = Convert.ToDateTime(DocumentNode("//ns9:tidspunkt").InnerText);
            }
            catch (Exception e)
            {
                throw new XmlParseException("Feil under bygging av Åpningskvittering. Klarte ikke finne alle felter i xml.", e);
            }
        }

        public override string ToString()
        {
            return String.Format("{0} med meldingsId {1}: \nTidspunkt: {2}. \nÅpningstidspunkt: {3}. \nKonversasjonsId: {4}. \nRefererer til melding med id: {5}", 
                GetType().Name, MeldingsId, Tidspunkt, Åpningstidspunkt, KonversasjonsId, RefToMessageId);
        }
    }
}
