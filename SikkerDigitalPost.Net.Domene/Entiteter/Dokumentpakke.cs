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

        public void LeggTil(IEnumerable<Dokument> dokumenter)
        {
            Vedlegg.AddRange(dokumenter);
        }

        public void LeggTil(params Dokument[] dokumenter)
        {
            Vedlegg.AddRange(dokumenter.ToList());
        }

    }
}
