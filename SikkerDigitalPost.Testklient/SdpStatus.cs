//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Threading;
//using SikkerDigitalPost.Net.Domene.Entiteter;
//using SikkerDigitalPost.Net.Domene.Entiteter.Kvitteringer;

//namespace SikkerDigitalPost.Net.KlientApi
//{
    
//    /// <summary>
//    /// Ekstremt forenklet håndtering av status på sending av digital post.
//    /// I en reell sender vil dette være en nøkkelkomponent som håndterer alt rundt resultatet av sending av digital post.
//    /// Dette vil typisk inkludere oppdatering av status i database/fagsystem, automatisk feilhåndtering, rapportering til
//    /// manuell feilhåndtering og så videre.
//    /// Se mulig tilstandsdiagram (http://begrep.difi.no/SikkerDigitalPost/forretningslag/avsender_tilstanddiagram) for forsendelser.
//    /// </summary>
//    public class SdpStatus
//    {
//        private readonly Dictionary<Forretningskvittering, Forretningskvittering> _status;
//        private Queue<Thread> _kø;

//        public SdpStatus()
//        {
//            _status = new Dictionary<Forretningskvittering, Forretningskvittering>();
//            _kø = new Queue<Thread>();
//        }

//        public void IkkeSendtPåGrunnAvKapasitet(Forsendelse forsendelse)
//        {
//            _status.Add(forsendelse.KonversasjonsId, "IkkeSendtPåGrunnAvKapasitet");
//        }

//        public void LagtTilKø(Forsendelse forsendelse)
//        {
//            _status.Add(forsendelse.KonversasjonsId, "LagtTilKø");
//        }

//        public void Sendt(Forsendelse forsendelse)
//        {
//            _status.Add(forsendelse.KonversasjonsId, "Sendt");
//        }

//        public void Feilet(Forsendelse forsendelse, Exception e)
//        {
//            _status.Add(forsendelse.KonversasjonsId, String.Format("Exception {0} ({1})", e.GetType().Name, e.Message));
//        }

//        public void Kvittering(Forretningskvittering forretningskvittering)
//        {
//            _status.Add(forretningskvittering.KonversasjonsId, forretningskvittering.ToString());
//        }

//        public Forretningskvittering Status()
//        {
//            StringBuilder stringbygger = new StringBuilder();

//            foreach (var nøkkelVerdipar in _status)
//            {
//                stringbygger.Append(nøkkelVerdipar.Key).Append(": ").Append(nøkkelVerdipar.Value).Append("\n"); 
//            }
//            return stringbygger.ToString();
//        }

//        public Forretningskvittering Køstatusstring()
//        {
//            if (_kø == null)
//            {
//                return String.Format("Kan ikke sjekke køstørrelse, køen er ikke satt.");
//            }

//            StringBuilder stringbygger = new StringBuilder();
//            stringbygger.Append("Antall elementer i køen: ").Append(_kø.Count).Append("\n");
//            //.Append("Gjenstående kapasitet: ").Append(_kø.)

//            foreach (var thread in _kø)
//            {
//                stringbygger.Append(thread).Append("\n");
//            }
//            return stringbygger.ToString();
//        }

//        public void SetQueue(Queue<Thread> kø)
//        {
//            _kø = kø;
//        }
//    }
//}
