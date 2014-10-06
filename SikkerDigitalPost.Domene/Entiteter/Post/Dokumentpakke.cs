using System.Collections.Generic;
using System.Linq;

namespace SikkerDigitalPost.Domene.Entiteter.Post
{
    public class Dokumentpakke
    {
        public Dokument Hoveddokument { get; private set; }
        public List<Dokument> Vedlegg { get; private set; }

        /// <param name="hoveddokument">Dokumentpakkens hoveddokument</param>
        public Dokumentpakke(Dokument hoveddokument)
        {
            Vedlegg = new List<Dokument>();
            Hoveddokument = hoveddokument;
        }

        /// <summary>
        /// Legger til vedlegg til allerede eksisterende vedlegg.
        /// </summary>
        /// <param name="dokumenter"></param>
        public void LeggTilVedlegg(IEnumerable<Dokument> dokumenter)
        {
            Vedlegg.AddRange(dokumenter);
        }

        /// <summary>
        /// Legger til vedlegg til allerede eksisterende vedlegg.
        /// </summary>
        /// <param name="dokumenter"></param>
        public void LeggTilVedlegg(params Dokument[] dokumenter)
        {
            Vedlegg.AddRange(dokumenter.ToList());
        }
    }
}
