using SikkerDigitalPost.Net.Domene.Entiteter.Ebms;
using SikkerDigitalPost.Net.Domene.Enums;

namespace SikkerDigitalPost.Net.Domene.Entiteter.Kvitteringer
{
    public class VarslingFeiletKvittering : Forretningskvittering
    {
        public readonly Varslingskanal Varslingskanal;
        public string Beskrivelse { get; set; }

        public VarslingFeiletKvittering(Varslingskanal varslingskanal, EbmsApplikasjonskvittering applikasjonskvittering) : base(applikasjonskvittering)
        {
            Varslingskanal = varslingskanal;
        }
    }
}
