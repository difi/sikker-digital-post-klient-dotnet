﻿using System;
using SikkerDigitalPost.Domene.Entiteter.Ebms;

namespace SikkerDigitalPost.Domene.Entiteter.Kvitteringer
{
    public class Åpningskvittering : Forretningskvittering
    {
        public Åpningskvittering(EbmsApplikasjonskvittering applikasjonskvittering) : base(applikasjonskvittering)
        {
        }

        public override string ToString()
        {
            return String.Format("{0} {konversasjonsId={1}}", GetType(), KonversasjonsId);
        }
    }
}
