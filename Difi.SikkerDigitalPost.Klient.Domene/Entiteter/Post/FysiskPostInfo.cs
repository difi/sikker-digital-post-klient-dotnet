using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private readonly List<Printinstruksjon> _printinstruksjoner;

        public FysiskPostInfo(FysiskPostMottaker mottaker, Posttype posttype, Utskriftsfarge utskriftsfarge, Posthåndtering posthåndtering, FysiskPostReturmottaker returmottaker, List<Printinstruksjon> printinstruksjoner = null)
            : base(mottaker)
        {
            Posttype = posttype;
            Utskriftsfarge = utskriftsfarge;
            Posthåndtering = posthåndtering;
            ReturpostMottaker = returmottaker;
            _printinstruksjoner = printinstruksjoner ?? new List<Printinstruksjon>();
        }

        public FysiskPostReturmottaker ReturpostMottaker { get; set; }

        public Posttype Posttype { get; set; }

        public Utskriftsfarge Utskriftsfarge { get; set; }

        public Posthåndtering Posthåndtering { get; set; }

        public IReadOnlyList<Printinstruksjon> Printinstruksjoner => new ReadOnlyCollection<Printinstruksjon>(_printinstruksjoner);
    }
}