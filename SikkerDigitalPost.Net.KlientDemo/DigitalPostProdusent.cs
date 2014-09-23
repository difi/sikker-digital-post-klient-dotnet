using System;
using System.Collections.Generic;
using System.Threading;
using SikkerDigitalPost.Net.Domene.Entiteter;
using SikkerDigitalPost.Net.KlientApi;

namespace SikkerDigitalPost.Net.KlientDemo
{
    /// <summary>
    /// Putter jobber i en kø for sending. Bør ha meklanismer for å sørge for å ikke produsere brev fortere enn de sendes.
    /// </summary>
    public class DigitalPostProdusent
    {
        private readonly Forsendelseskilde _forsendelseskilde;
        private readonly SikkerDigitalPostKlient _klient;
        private readonly SdpStatus _sdpStatus;

        private int _sendeIntervall;
        private bool _kjører = false;

        //Logger
        //ThreadPoolExecutor
        private readonly Queue<Thread> _kø;

        public DigitalPostProdusent(Forsendelseskilde forsendelseskilde, SikkerDigitalPostKlient klient,
            SdpStatus sdpStatus)
        {
            _forsendelseskilde = forsendelseskilde;
            _klient = klient;
            _sdpStatus = sdpStatus;

            _kø = new Queue<Thread>();
            Thread tråd = new Thread(new ThreadStart(Run));
            tråd.Start();
        }

        private void Run()
        {
            if (_kjører)
            {
                throw new Exception(String.Format("Prøvde å starte, kjører allerede {0}", GetType().Name));
            }

            _kjører = true;
            while (_kjører)
            {
                // Hent forsendelse
                try
                {
                    Forsendelse forsendelse = _forsendelseskilde.LagBrev();

                    
                    

                }
                catch (Exception)
                {
                    
                    throw;
                }
            }
        }
    }
}
