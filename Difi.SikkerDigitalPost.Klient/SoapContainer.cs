using System;
using System.Collections.Generic;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Interface;

namespace Difi.SikkerDigitalPost.Klient
{
    internal class SoapContainer
    {
        public readonly string Boundary;

        public SoapContainer(ISoapVedlegg envelope)
        {
            Envelope = envelope;
            Boundary = Guid.NewGuid().ToString();
            Vedlegg = new List<ISoapVedlegg>();
        }

        public IList<ISoapVedlegg> Vedlegg { get; set; }

        public ISoapVedlegg Envelope { get; set; }
    }
}