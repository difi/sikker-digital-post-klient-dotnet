using System.Collections.Generic;
using System.IO;
using SikkerDigitalPost.Net.Domene.Entiteter.Ebms;
using SikkerDigitalPost.Net.Domene.Enums;

namespace SikkerDigitalPost.Net.Domene.Entiteter.Kvitteringer
{
    public class EbmsApplikasjonskvittering : EbmsUtgåendeMelding
    {
        public readonly StandardForretningsDokument StandardForretningsDokument;
        public readonly IEnumerable<Referanse> Referanser = new List<Referanse>();
        public EbmsAktør Avsender = null;
        public readonly StreamReader SfdStrøm;

        public EbmsApplikasjonskvittering(EbmsAktør avsender, string mpcId, EbmsAktør ebmsMottaker, Prioritet prioritet, string meldingsId, Handling handling, string opprinnelsesMeldingId, StandardForretningsDokument standardForretningsDokument, StreamReader sfdStrøm) : base(ebmsMottaker, meldingsId, opprinnelsesMeldingId, handling, prioritet, mpcId)
        {
            StandardForretningsDokument = standardForretningsDokument;
            Avsender = avsender;
            SfdStrøm = sfdStrøm;
        }
    }
}