using System.Collections.Generic;
using System.IO;
using SikkerDigitalPost.Net.Domene.Entiteter.Post;
using SikkerDigitalPost.Net.Domene.Enums;

namespace SikkerDigitalPost.Net.Domene.Entiteter.Ebms
{
    public class EbmsApplikasjonskvittering : EbmsUtgåendeMelding
    {
        public readonly StandardForretningsDokument StandardForretningsDokument;
        internal readonly IEnumerable<Sha256Reference> Referanser = new List<Sha256Reference>();
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