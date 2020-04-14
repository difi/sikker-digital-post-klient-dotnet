namespace Difi.SikkerDigitalPost.Klient.SBDH
{
    public class ForretningsMelding
    {
        public ForretningsMeldingType type  { get; set; }

        public string hoveddokument { get; set; }

        private string avsenderId { get; set; }
        private string fakturaReferanse { get; set; }
    }

    public enum ForretningsMeldingType
    {
        DIGITAL,
        PRINT,
    }
}
