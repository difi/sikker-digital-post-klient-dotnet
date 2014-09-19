using System.Collections.Generic;
using System.Linq;


namespace SikkerDigitalPost.Net.Domene.Entiteter
{
    public class Dokumentpakke
    {
        public Dokument Hoveddokument { get; private set; }
        public List<Dokument> Vedlegg { get; private set; }


        public Dokumentpakke(Dokument hoveddokument)
        {
            Vedlegg = new List<Dokument>();
            Hoveddokument = hoveddokument;
        }

        public void LeggTilVedlegg(IEnumerable<Dokument> dokumenter)
        {
            Vedlegg.AddRange(dokumenter);
        }

        public void LeggTilVedlegg(params Dokument[] dokumenter)
        {
            Vedlegg.AddRange(dokumenter.ToList());
        }
    }
}
