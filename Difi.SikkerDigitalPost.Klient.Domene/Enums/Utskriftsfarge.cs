using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Difi.SikkerDigitalPost.Klient.Domene.Enums
{
    /// <summary>
    ///     Utskriftstype avgjør hvilken utskriftsjobb brevet blir en del av. Det er to forskjellige utskriftsjobber,
    ///     én for farge utskrift og en for sort-hvitt. Utskriftstype avgjør fargen på alle ark inklusive forsidearket.
    ///     Hver utskriftstype har forskjellig pris.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumMemberConverter))]
    public enum Utskriftsfarge
    {
        [EnumMember(Value = "SORT_HVIT")]
        SortHvitt,
        [EnumMember(Value = "FARGE")]
        Farge
    }
}
