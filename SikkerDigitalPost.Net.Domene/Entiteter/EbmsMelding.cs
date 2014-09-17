using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SikkerDigitalPost.Net.Domene.Entiteter
{
    public abstract class EbmsMelding
    {
        public readonly string MeldingsId;
        public readonly string OpprinnelsesMeldingId;

        protected EbmsMelding(string meldingsId, string opprinnelsesMeldingId)
        {
            MeldingsId = meldingsId;
            OpprinnelsesMeldingId = opprinnelsesMeldingId;
        }
    }
}
