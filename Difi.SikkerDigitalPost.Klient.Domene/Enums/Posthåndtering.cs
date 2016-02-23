using System;

namespace Difi.SikkerDigitalPost.Klient.Domene.Enums
{
    /// <summary>
    ///     Valg av håndteringmetode for retur post. Dette blir valgt når post sendes til utskrift og avgjør
    ///     hvordan returadresse og EA-(Elektronisk adresseoppdatering) logo m.m. behandles.
    /// </summary>
    public enum Posthåndtering
    {
        /// <summary>
        ///     Returpost blir sendt direkte til adressen angitt som returpost adressen, ingen videre oppfølging.
        /// </summary>
        DirekteRetur,

        /// <summary>
        ///     All post får lagt på et EA merke og en strekkode. Returpost blir sendt til Posten sin EA tjeneste
        ///     der strekkoden blir scannet, melding om returpost blir sendt til Avsender og brevet blir makulert.
        /// </summary>
        MakuleringMedMelding
    }

    internal static class PosthåndteringHelper
    {
        internal static string EnumToString(Posthåndtering posthåndtering)
        {
            switch (posthåndtering)
            {
                case Posthåndtering.DirekteRetur:
                    return "DIREKTE_RETUR";
                case Posthåndtering.MakuleringMedMelding:
                    return "MAKULERING_MED_MELDING";
                default:
                    throw new ArgumentOutOfRangeException(nameof(posthåndtering));
            }
        }
    }
}