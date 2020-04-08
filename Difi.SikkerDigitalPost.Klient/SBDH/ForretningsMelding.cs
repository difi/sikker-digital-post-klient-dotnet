namespace Difi.SikkerDigitalPost.Klient.SBDH
{
    public class ForretningsMelding
    {
        private ForretningsMeldingType type;

        public string hoveddokument;

        private string avsenderId;
        private string fakturaReferanse;
    }

    public enum ForretningsMeldingType
    {
        DIGITAL,
        PRINT,
    }
}
