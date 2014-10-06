using System;
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

        public override string ToString()
        {
            return String.Format("{0}{konversasjonsId={1}, varslingskanal={2}, beskrivelse='{3}'}.",
                GetType().Name, KonversasjonsId, Varslingskanal, Beskrivelse);
        }
    }
}
