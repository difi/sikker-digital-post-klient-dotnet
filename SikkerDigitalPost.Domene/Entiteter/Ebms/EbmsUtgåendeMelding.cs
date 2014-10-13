using SikkerDigitalPost.Domene.Enums;

namespace SikkerDigitalPost.Domene.Entiteter.Ebms
{
    public abstract class EbmsUtgåendeMelding : EbmsMelding
    {
        public readonly Handling Handling;
        public readonly Prioritet Prioritet;
        public readonly string MpcId;
        protected EbmsAktør EbmsMottaker;

        protected EbmsUtgåendeMelding(EbmsAktør ebmsMottaker, string meldingsId, string opprinnelsesMeldingId, Handling handling, Prioritet prioritet, string mpcId) :
            base(meldingsId, opprinnelsesMeldingId)
        {
            Handling = handling;
            Prioritet = prioritet;
            MpcId = mpcId;
            EbmsMottaker = ebmsMottaker;
        }
    }
}
