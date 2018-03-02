using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Interface;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Post.Utvidelser;
using Difi.SikkerDigitalPost.Klient.Domene.Exceptions;

namespace Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Post
{
    public class Dokumentpakke
    {
        private readonly List<Dokument> _vedlegg;

        private readonly List<DataDokument> _dataDokumenter;

        /// <param name="hoveddokument">Dokumentpakkens hoveddokument</param>
        public Dokumentpakke(Dokument hoveddokument)
        {
            SjekkIkkeTomFil(hoveddokument);

            _vedlegg = new List<Dokument>();
            _dataDokumenter = new List<DataDokument>();
            Hoveddokument = hoveddokument;
            Hoveddokument.Id = "Id_2";

            if (Hoveddokument.DataDokument != null)
            {
                LeggTilDataDokument(Hoveddokument.DataDokument, 2);
            }
        }

        public Dokument Hoveddokument { get; }

        public IReadOnlyList<Dokument> Vedlegg => new ReadOnlyCollection<Dokument>(_vedlegg);

        public IReadOnlyList<DataDokument> DataDokumenter => new ReadOnlyCollection<DataDokument>(_dataDokumenter);

        /// <summary>
        ///     Legger til vedlegg i tillegg til allerede eksisterende vedlegg.
        /// </summary>
        /// <param name="dokumenter"></param>
        public void LeggTilVedlegg(params Dokument[] dokumenter)
        {
            LeggTilVedlegg(dokumenter.ToList());
        }

        /// <summary>
        ///     Legger til vedlegg i tillegg til allerede eksisterende vedlegg.
        /// </summary>
        /// <param name="dokumenter"></param>
        public void LeggTilVedlegg(IEnumerable<Dokument> dokumenter)
        {
            var startId = Vedlegg.Count;
            for (var i = 0; i < dokumenter.Count(); i++)
            {
                var dokument = dokumenter.ElementAt(i);

                SjekkIngenDuplikater(dokument);

                SjekkIkkeTomFil(dokument);

                var id = i + 3 + startId;
                dokument.Id = $"Id_{id}";

                if (dokument.DataDokument != null)
                {
                    LeggTilDataDokument(dokument.DataDokument, id);
                }

                _vedlegg.Add(dokument);
            }
        }

        private void LeggTilDataDokument(DataDokument dokument, int id)
        {
            SjekkIngenDuplikater(dokument);

            SjekkIkkeTomFil(dokument);

            dokument.Id = $"Id_{id}_Data";

            _dataDokumenter.Add(dokument);
        }

        private void SjekkIngenDuplikater(IAsiceAttachable dokument)
        {
            var likeFiler = Vedlegg.Any(v => dokument.Filnavn == v.Filnavn) || dokument.Filnavn == Hoveddokument.Filnavn || DataDokumenter.Any(d => dokument.Filnavn == d.Filnavn);

            if (likeFiler)
            {
                throw new KonfigurasjonsException(
                    $"Dokumentet {dokument.Filnavn} som du prøver å legge til, eksisterer allerede med samme filnavn. Det er ikke tillatt å ha likt filnavn på to vedlegg, eller vedlegg med samme filnavn som hoveddokument.");
            }
        }

        private static void SjekkIkkeTomFil(IAsiceAttachable dokument)
        {
            if (dokument.Bytes.Length == 0)
            {
                throw new KonfigurasjonsException(
                    $"Dokumentet {dokument.Filnavn} som du prøver å legge til som vedlegg er tomt. Det er ikke tillatt å sende tomme dokumenter.");
            }
        }

    }
}