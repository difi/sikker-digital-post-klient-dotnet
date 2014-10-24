﻿using System;
using System.Collections.Generic;
using System.Linq;
using SikkerDigitalPost.Domene.Exceptions;

namespace SikkerDigitalPost.Domene.Entiteter.Post
{
    public class Dokumentpakke
    {
        public Dokument Hoveddokument { get; private set; }
        public List<Dokument> Vedlegg { get; private set; }

        /// <param name="hoveddokument">Dokumentpakkens hoveddokument</param>
        public Dokumentpakke(Dokument hoveddokument)
        {
            if (hoveddokument.Bytes.Length == 0)
            {
                throw new KonfigurasjonsException("Du prøver å legge til et hoveddokument som er tomt. Dette er ikke tillatt.");
            }
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
            int startId = Vedlegg.Count();
            for (int i = 0; i < dokumenter.Count(); i++)
            {
                var dokument = dokumenter.ElementAt(i);
                if (dokument.Bytes.Length == 0)
                    throw new KonfigurasjonsException(string.Format("Dokumentet {0} som du prøver å legge til som vedlegg er tomt. Det er ikke tillatt å sende tomme dokumenter.", dokument.Filnavn));
                dokument.Id = string.Format("Id_{0}", i + 3 + startId);
                Vedlegg.Add(dokument);
            }
        }
    }
}
