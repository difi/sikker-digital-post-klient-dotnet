using System.Collections.Generic;

namespace SikkerDigitalPost.Net.Domene
{
    public class Dokumentpakke
    {
        public Dokument Hoveddokument { get; set; }
        public IEnumerable<Dokument> Vedlegg { get; set; }
    }
}
