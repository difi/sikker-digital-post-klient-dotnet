using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Difi.SikkerDigitalPost.Klient.Domene.Enums
{
    /// <summary>
    ///     Valg av håndteringmetode for retur post. Dette blir valgt når post sendes til utskrift og avgjør
    ///     hvordan returadresse og EA-(Elektronisk adresseoppdatering) logo m.m. behandles.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    public enum Posthåndtering
    {
        /// <summary>
        ///     Returpost blir sendt direkte til adressen angitt som returpost adressen, ingen videre oppfølging.
        /// </summary>
        [EnumMember(Value = "DIREKTE_RETUR")]
        DirekteRetur,

        /// <summary>
        ///     All post får lagt på et EA merke og en strekkode. Returpost blir sendt til Posten sin EA tjeneste
        ///     der strekkoden blir scannet, melding om returpost blir sendt til Avsender og brevet blir makulert.
        /// </summary>
        [EnumMember(Value = "MAKULERING_MED_MELDING")]
        MakuleringMedMelding
    }

}
