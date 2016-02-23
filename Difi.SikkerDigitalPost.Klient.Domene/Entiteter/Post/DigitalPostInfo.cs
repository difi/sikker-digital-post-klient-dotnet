using System;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Aktører;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Varsel;
using Difi.SikkerDigitalPost.Klient.Domene.Enums;

namespace Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Post
{
    /// <summary>
    ///     Inneholder nødvendig informasjon for å sende et brev digitalt til Digipost eller Eboks, slik
    ///     som mottaker, tittel, sikkerhetsnivå og om åpningskvittering skal aktiveres.
    /// </summary>
    public class DigitalPostInfo : PostInfo
    {
        /// <summary>
        ///     Dato og tidspunkt for når en melding skal tilgjengeliggjøres for Innbygger i Innbygger sin postkasse.
        ///     Forsendelsen vil leveres til postkassen før dette tidspunkt, men ikke være synlig/tilgjengelig for innbygger.
        ///     Standardverdi er nå.
        /// </summary>
        public DateTime Virkningstidspunkt = DateTime.Now;

        /// <param name="mottaker">Mottaker av digital post.</param>
        /// <param name="ikkeSensitivTittel">
        ///     Ikke-sensitiv tittel på brevet. Denne tittelen vil være synlig under transport av
        ///     meldingen, og kan vises i mottakerens postkasse.
        /// </param>
        /// <param name="sikkerhetsnivå">
        ///     Nødvendig autentiseringsnivå som kreves av mottaker i postkassen for å åpne brevet.
        ///     Standardverdi er Nivå4.
        /// </param>
        /// <param name="åpningskvittering">Ønskes kvittering når brevet blir åpnet av mottaker? Standard er false.</param>
        public DigitalPostInfo(DigitalPostMottaker mottaker, string ikkeSensitivTittel, Sikkerhetsnivå sikkerhetsnivå = Sikkerhetsnivå.Nivå4, bool åpningskvittering = false)
            : base(mottaker)
        {
            IkkeSensitivTittel = ikkeSensitivTittel;
            Sikkerhetsnivå = sikkerhetsnivå;
            Åpningskvittering = åpningskvittering;
        }

        /// <summary>
        ///     Ønskes kvittering når brevet blir åpnet av mottaker? Mottaker må akseptere at det sendes en åpningskvittering
        ///     til avsender for å få lest den digitale posten. Mangel på åpningskvittering betyr at mottaker ikke har lest posten.
        ///     Bruk av åpningskvittering er priset for avsender. Se mer info på
        ///     dhttp://begrep.difi.no/SikkerDigitalPost/1.2.0.RC1/meldinger/AapningsKvittering.
        ///     Standard er false.
        /// </summary>
        public bool Åpningskvittering { get; set; }

        /// <summary>
        ///     Nødvendig autentiseringsnivå som kreves av mottaker i postkassen for å åpne brevet.
        ///     Standardverdi er Nivå4
        /// </summary>
        public Sikkerhetsnivå Sikkerhetsnivå { get; set; }

        /// <summary>
        ///     Ikke-sensitiv tittel på brevet.
        ///     Denne tittelen vil være synlig under transport av meldingen, og kan vises i mottakerens postkasse.
        /// </summary>
        public string IkkeSensitivTittel { get; set; }

        /// <summary>
        ///     Minimum e-postvarsel som skal sendes til mottakker av brevet. Postkassen kan velge å sende andre varsler i tillegg.
        ///     Standard er standardoppførselen til postkasseleverandøren.
        /// </summary>
        public EpostVarsel EpostVarsel { get; set; }

        /// <summary>
        ///     Minimum sms-varsel som skal sendes til mottaker av brevet. Postkassen kan velge å sende andre varsler i tillegg.
        ///     Standard er standardoppførselen til postkasseleverandøren.
        /// </summary>
        public SmsVarsel SmsVarsel { get; set; }
    }
}