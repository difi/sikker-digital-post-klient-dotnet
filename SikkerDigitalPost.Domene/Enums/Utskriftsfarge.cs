using System;

namespace SikkerDigitalPost.Domene.Enums
{
    public enum Utskriftsfarge
    {
        SortHvitt,
        Farge
    }

    internal static class UtskriftsfargeHelper
    {
        internal static string ToString(Utskriftsfarge utskriftsfarge)
        {
            switch (utskriftsfarge)
            {
                case Utskriftsfarge.SortHvitt:
                    return "SORT_HVIT";
                case Utskriftsfarge.Farge:
                    return "FARGE";
                default:
                    throw new ArgumentOutOfRangeException("utskriftsfarge");
            }
        }
    }
}
