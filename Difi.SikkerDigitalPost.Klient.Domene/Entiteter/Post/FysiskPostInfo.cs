using System;
using System.Linq.Expressions;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Aktører;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.FysiskPost;
using Difi.SikkerDigitalPost.Klient.Domene.Enums;

namespace Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Post
{
    /// <summary>
    /// Inneholder nødvendig informasjon for å sende et brev digitalt til Digipost eller Eboks, slik 
    /// som mottaker, posttype, om brevet skal skrives ut med farge, hvordan det skal håndteres hvis
    /// det ikke blir levert og en returmottaker.
    /// </summary>
    public class FysiskPostInfo : PostInfo
    {
        public Posttype Posttype { get; set; }

        public Utskriftsfarge Utskriftsfarge { get; set; }

        public Posthåndtering Posthåndtering { get; set; }

        private FysiskPostMottakerAbstrakt _returmottakerAbstrakt;

        [Obsolete("Typen på ReturMottaker er nå endret til Returpostmottaker. OBS! Vil bli fjernet fom. neste versjon.")]
        public FysiskPostMottaker ReturMottaker
        {
            get { return _returmottakerAbstrakt as FysiskPostMottaker; }
            set { _returmottakerAbstrakt = value; }
        }
        
        public FysiskPostReturmottaker Returpostmottaker
        {
            get { return new FysiskPostReturmottaker(_returmottakerAbstrakt.Navn,_returmottakerAbstrakt.Adresse); }
        }

        [Obsolete("Denne konstruktøren skal ikke brukes. Bruk Konstruktøren med FysiskPosttMottaker og FysiskPostReturMottaker. OBS! Vil bli fjernet fom. neste versjon.")]
        public FysiskPostInfo(PostMottaker mottaker, Posttype posttype, Utskriftsfarge utskriftsfarge, Posthåndtering posthåndtering, FysiskPostMottaker returMottaker) : base(mottaker)
        {
            Posttype = posttype;
            Utskriftsfarge = utskriftsfarge;
            Posthåndtering = posthåndtering;
            _returmottakerAbstrakt = returMottaker;
        }

        public FysiskPostInfo(FysiskPostMottaker mottaker, Posttype posttype, Utskriftsfarge utskriftsfarge, Posthåndtering posthåndtering, FysiskPostReturmottaker returmottaker)
            : base(mottaker)
        {
            Posttype = posttype;
            Utskriftsfarge = utskriftsfarge;
            Posthåndtering = posthåndtering;
            _returmottakerAbstrakt = returmottaker;
        }
    }
}
