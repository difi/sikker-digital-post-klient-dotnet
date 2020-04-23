using Difi.SikkerDigitalPost.Klient.Domene.Enums;

namespace Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer.Transport
{
    /// <summary>
    ///     Transportkvittering som indikerer at noe har gått galt ved sending av en melding.
    /// </summary>
    public class TransportFeiletKvittering : Transportkvittering
    {
        /// <summary>
        ///     Spesifikk feilkode for transporten.
        /// </summary>
        public string Feilkode { get; set; }

        /// <summary>
        ///     Kategori/id for hvilken feil som oppstod.
        /// </summary>
        public string Kategori { get; set; }

        /// <summary>
        ///     Opprinnelse for feilmeldingen.
        /// </summary>
        public string Opprinnelse { get; set; }

        /// <summary>
        ///     Hvor alvorlig er feilen som oppstod
        /// </summary>
        public string Alvorlighetsgrad { get; set; }

        /// <summary>
        ///     En mer detaljert beskrivelse av hva som gikk galt.
        /// </summary>
        public string Beskrivelse { get; set; }

        /// <summary>
        ///     Hvem man antar har skyld i feilen.
        /// </summary>
        public Feiltype Skyldig { get; set; }

        public new string ToString()
        {
            return $"{base.ToString()}, Feilkode: {Feilkode}, Kategori: {Kategori}, Opprinnelse: {Opprinnelse}, Alvorlighetsgrad: {Alvorlighetsgrad}, Beskrivelse: {Beskrivelse}, Skyldig: {Skyldig}";
        }
    }
}