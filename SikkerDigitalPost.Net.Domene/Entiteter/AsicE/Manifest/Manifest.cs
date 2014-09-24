﻿using SikkerDigitalPost.Net.Domene.Entiteter.Interface;

namespace SikkerDigitalPost.Net.Domene.Entiteter.AsicE.Manifest
{
    public class Manifest : IAsiceVedlegg
    {
        public Manifest(Mottaker mottaker, Behandlingsansvarlig avsender, Forsendelse forsendelse)
        {
            Avsender = avsender;
            Forsendelse = forsendelse;
            Mottaker = mottaker;
            var bygger = new ManifestBygger(this);
            Bytes = bygger.Bygg();
        }

        public Behandlingsansvarlig Avsender { get; private set; }

        public Forsendelse Forsendelse { get; set; }

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
