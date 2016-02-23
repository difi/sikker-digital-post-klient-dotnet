using System;

namespace Difi.SikkerDigitalPost.Klient.Domene.Enums
{
    /// <summary>
    ///     Utskriftstype avgjør hvilken utskriftsjobb brevet blir en del av. Det er to forskjellige utskriftsjobber,
    ///     én for farge utskrift og en for sort-hvitt. Utskriftstype avgjør fargen på alle ark inklusive forsidearket.
    ///     Hver utskriftstype har forskjellig pris.
    /// </summary>
    public enum Utskriftsfarge
    {
        SortHvitt,
        Farge
    }

    internal static class UtskriftsfargeHelper
    {
        internal static string EnumToString(Utskriftsfarge utskriftsfarge)
        {
            switch (utskriftsfarge)
            {
                case Utskriftsfarge.SortHvitt:
                    return "SORT_HVIT";
                case Utskriftsfarge.Farge:
                    return "FARGE";
                default:
                    throw new ArgumentOutOfRangeException(nameof(utskriftsfarge));
            }
        }
    }
}