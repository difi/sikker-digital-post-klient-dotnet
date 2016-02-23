using System;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.FysiskPost;
using Difi.SikkerDigitalPost.Klient.Domene.Enums;

namespace Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Post
{
    /// <summary>
    ///     Inneholder nødvendig informasjon for å sende et brev digitalt til Digipost eller Eboks, slik
    ///     som mottaker, posttype, om brevet skal skrives ut med farge, hvordan det skal håndteres hvis
    ///     det ikke blir levert og en returmottaker.
    /// </summary>
    public class FysiskPostInfo : PostInfo
    {
        private FysiskPostMottakerAbstrakt _returmottakerAbstrakt;

        public FysiskPostInfo(FysiskPostMottaker mottaker, Posttype posttype, Utskriftsfarge utskriftsfarge, Posthåndtering posthåndtering, FysiskPostReturmottaker returmottaker)
            : base(mottaker)
        {
            Posttype = posttype;
            Utskriftsfarge = utskriftsfarge;
            Posthåndtering = posthåndtering;
            _returmottakerAbstrakt = returmottaker;
        }

        public Posttype Posttype { get; set; }

        public Utskriftsfarge Utskriftsfarge { get; set; }

        public Posthåndtering Posthåndtering { get; set; }

        [Obsolete("Typen på ReturMottaker er nå endret til Returpostmottaker. OBS! Vil bli fjernet fom. neste versjon.")]
        public FysiskPostMottaker ReturMottaker
        {
            get { return _returmottakerAbstrakt as FysiskPostMottaker; }
            set { _returmottakerAbstrakt = value; }
        }

        public FysiskPostReturmottaker Returpostmottaker
            => new FysiskPostReturmottaker(_returmottakerAbstrakt.Navn, _returmottakerAbstrakt.Adresse);
    }
}