using System;
using System.Text.Json.Serialization;

namespace Difi.SikkerDigitalPost.Klient.SBDH
{
    public abstract class ForretningsMelding
    {
        [JsonIgnore]
        public ForretningsMeldingType type  { get; set; }

        public string hoveddokument { get; set; }

        public string avsenderId { get; set; }
        public string fakturaReferanse { get; set; }

        public ForretningsMelding(ForretningsMeldingType meldingType)
        {
            this.type = meldingType;
        }
    }

    public enum ForretningsMeldingType
    {
        digital,
        print,
    }
}
