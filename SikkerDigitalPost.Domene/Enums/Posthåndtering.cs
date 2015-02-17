using System;

namespace SikkerDigitalPost.Domene.Enums
{
    public enum Posthåndtering
    {
        DirekteRetur,
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
                    throw new ArgumentOutOfRangeException("posthåndtering");
            }
        }
    }
}
