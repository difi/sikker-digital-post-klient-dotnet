using SikkerDigitalPost.Net.Domene.Entiteter.Interface;

namespace SikkerDigitalPost.Net.Domene.Entiteter.AsicE.Manifest
{
    public class Manifest : IAsiceVedlegg
    {
        public Manifest(Mottaker mottaker, TekniskAvsender avsender, Dokumentpakke dokumentpakke)
        {
            Avsender = avsender;
            Dokumentpakke = dokumentpakke;
            Mottaker = mottaker;
        }

        public TekniskAvsender Avsender { get; private set; }

        public Dokumentpakke Dokumentpakke { get; private set; }

        public Mottaker Mottaker { get; private set; }

        public byte[] Bytes { get; private set; }

        public string Filnavn {
            get { return "manifest.xml"; }
        }

        public string Innholdstype {
            get { return "application/xml"; }
        }


    }
}
