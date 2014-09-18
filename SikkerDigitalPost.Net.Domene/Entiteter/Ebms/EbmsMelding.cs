namespace SikkerDigitalPost.Net.Domene.Entiteter.Ebms
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
