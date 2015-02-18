using SikkerDigitalPost.Domene.Entiteter.Aktører;

namespace SikkerDigitalPost.Domene.Entiteter.FysiskPost
{
    public class FysiskPostMottaker : PostMottaker
    {
        public string Navn { get; set; }

        public NorskAdresse NorskAdresse { get; set; }
    }
}
