using System;
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
            Hoveddokument.Id = "Id_2";
        }

        /// <summary>
        /// Legger til vedlegg i tillegg til allerede eksisterende vedlegg.
        /// </summary>
        /// <param name="dokumenter"></param>
        public void LeggTilVedlegg(params Dokument[] dokumenter)
        {
            LeggTilVedlegg(dokumenter.ToList());
        }

        /// <summary>
        /// Legger til vedlegg i tillegg til allerede eksisterende vedlegg.
        /// </summary>
        /// <param name="dokumenter"></param>
        public void LeggTilVedlegg(IEnumerable<Dokument> dokumenter)
        {
            Dokument nåværendeDokument;
            for (int i = 0; i < dokumenter.Count(); i++)
            {
                nåværendeDokument = dokumenter.ElementAt(i);
                nåværendeDokument.Id = String.Format("Id_{0}", i + 3);
            }

            Vedlegg.AddRange(dokumenter);
        }
    }
}
