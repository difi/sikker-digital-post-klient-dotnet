using System.Collections.Generic;

namespace SikkerDigitalPost.Net.Domene.Entiteter
{
    public class Dokumentpakke
    {
        public Dokument Hoveddokument { get; set; }
        public IEnumerable<Dokument> Vedlegg { get; set; }
    }
}
