using System;
using SikkerDigitalPost.Domene.Entiteter.Ebms;
using SikkerDigitalPost.Domene.Enums;

namespace SikkerDigitalPost.Domene.Entiteter.Kvitteringer
{
    public class VarslingFeiletKvittering : Kvittering
    {
        public readonly Varslingskanal Varslingskanal;
        public string Beskrivelse { get; set; }

        public VarslingFeiletKvittering(DateTime tidspunkt, Varslingskanal varslingskanal)
        {
            Varslingskanal = varslingskanal;
        }
    }

    //public class OldVarslingFeiletKvittering : Forretningskvittering
    //{
        //public readonly Varslingskanal Varslingskanal;
        //public string Beskrivelse { get; set; }

        //public OldVarslingFeiletKvittering(Varslingskanal varslingskanal, EbmsApplikasjonskvittering applikasjonskvittering) : base(applikasjonskvittering)
        //{
        //    Varslingskanal = varslingskanal;
        //}

        //public override string ToString()
        //{
        //    return String.Format("{0}{konversasjonsId={1}, varslingskanal={2}, beskrivelse='{3}'}.",
        //        GetType().Name, KonversasjonsId, Varslingskanal, Beskrivelse);
        //}
    //}
}
