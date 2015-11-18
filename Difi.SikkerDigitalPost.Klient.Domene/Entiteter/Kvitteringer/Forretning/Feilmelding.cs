using System;
using System.Xml;
using Difi.SikkerDigitalPost.Klient.Domene.Enums;
using Difi.SikkerDigitalPost.Klient.Domene.Exceptions;

namespace Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer.Forretning
{
    /// <summary>
    /// En feilmelding fra postkasseleverandør med informasjon om en forretningsfeil knyttet til en digital post forsendelse.
    /// Les mer på http://begrep.difi.no/SikkerDigitalPost/1.0.2/meldinger/FeilMelding.
    /// </summary>
    public class Feilmelding : Forretningskvittering
    {
        /// <summary>
        /// Beskriver hvor feilen ligger. Enten Klient eller Server.
        /// </summary>
        public Feiltype Skyldig { get; protected set; }

        public string Detaljer { get; protected set; }

        public DateTime TidspunktFeilet { get; protected set; }
        public Feilmelding() { }
        internal Feilmelding(XmlDocument xmlDocument, XmlNamespaceManager namespaceManager):base(xmlDocument,namespaceManager)
        {
            try
            {
                TidspunktFeilet = Convert.ToDateTime(DocumentNode("//ns9:tidspunkt").InnerText);

                var feiltype = DocumentNode("//ns9:feiltype").InnerText;
                Skyldig = feiltype.ToLower().Equals(Feiltype.Klient.ToString().ToLower())
                    ? Feiltype.Klient
                    : Feiltype.Server;

                Detaljer = DocumentNode("//ns9:detaljer").InnerText;
            }
            catch (Exception e)
            {
                throw new XmlParseException("Feil under bygging av Feilmelding-kvittering. Klarte ikke finne alle felter i xml.", e);
            }
        }

        public override string ToString()
        {
            return String.Format("{0} med meldingsId {1}: \nTidspunkt: {2}. \nTidspunkt feilet: {3}. \nSkyldig: {4}. \nDetaljer: {5}. \nKonversasjonsId: {6}. \nRefererer til melding med id: {7}", 
                GetType().Name, MeldingsId, Tidspunkt, TidspunktFeilet, Skyldig, Detaljer, KonversasjonsId, ReferanseTilMeldingId);
        }
    }
}
