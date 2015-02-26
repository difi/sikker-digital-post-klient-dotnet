using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SikkerDigitalPost.Domene.Entiteter;
using SikkerDigitalPost.Domene.Entiteter.Aktører;
using SikkerDigitalPost.Domene.Entiteter.Post;
using SikkerDigitalPost.Domene.Enums;
using SikkerDigitalPost.Klient;
using SikkerDigitalPost.Klient.Utilities;

namespace SikkerDigitalPost.Testklient
{
    public class Tråder
    {
        private static int AntallBrev = 10;

        private readonly SikkerDigitalPostKlient _klient;
        private readonly PostkasseInnstillinger _postkasseInnstillinger;
        private readonly Behandlingsansvarlig _behandlingsansvarlig;

        public Tråder(Databehandler databehandler, Klientkonfigurasjon klientkonfigurasjon)
        {
            _klient = new SikkerDigitalPostKlient(databehandler, klientkonfigurasjon);
            _postkasseInnstillinger = PostkasseInnstillinger.GetPosten();
            _behandlingsansvarlig = new Behandlingsansvarlig(new Organisasjonsnummer(_postkasseInnstillinger.OrgNummerBehandlingsansvarlig));
        }

        public void SendMelding(Forsendelse forsendelse)
        {
            //Console.WriteLine("Tråd {0} SENDER forsendelse {1} til {2}...", Thread.CurrentThread.Name, forsendelse.Dokumentpakke.Hoveddokument.Tittel, forsendelse.DigitalPostInfo.Mottaker.Postkasseadresse);
            var transportkvittering = _klient.Send(forsendelse);
            Console.WriteLine("Tråd {0} er FERDIG med forsendelse {1} og fikk en {2}", Thread.CurrentThread.Name, forsendelse.Dokumentpakke.Hoveddokument.Tittel, transportkvittering.GetType().Name);
        }

        internal async void SendMeldinger()
        {
            Action<Forsendelse> sendMeldingAction = (s) => SendMelding(s);

            var marit = new DigitalPostMottaker("25053700003", "marit.kjesnes#19BD", _postkasseInnstillinger.Mottakersertifikat, _postkasseInnstillinger.OrgnummerPostkasse);
            var forsendelserTilDangfart = GetForsendelser("MaritTråd", AntallBrev, marit, "Marit", "MaritBrev");

            var joni = new DigitalPostMottaker("13013500002", "joni.sneve#0VAS", _postkasseInnstillinger.Mottakersertifikat, _postkasseInnstillinger.OrgnummerPostkasse);
            var forsendelserTilJoni = GetForsendelser("JoniTråd", AntallBrev, joni, "Joni", "JoniBrev");

            var jarand = new DigitalPostMottaker("01013300001", "jarand.bjarte.t.k.grindheim#6KMG", _postkasseInnstillinger.Mottakersertifikat, _postkasseInnstillinger.OrgnummerPostkasse);
            var forsendelserTilJarandBjarte = GetForsendelser("Jarand-BjarteTråd", AntallBrev, jarand, "Jarand", "Jarand-BjarteBrev");

            BlockingCollection<Forsendelse> alleForsendelser = new BlockingCollection<Forsendelse>();

            while (true)
            {
                if (forsendelserTilDangfart.Count == 0)
                    break;
                
                alleForsendelser.Add(forsendelserTilDangfart.Take());
                alleForsendelser.Add(forsendelserTilJoni.Take());
                alleForsendelser.Add(forsendelserTilJarandBjarte.Take());
            }

            var tasks = alleForsendelser.Select(p => new Action(() => sendMeldingAction(p))).ToArray();
            await Task.Run(() => Parallel.Invoke(_opts, tasks));
        }

        private readonly ParallelOptions _opts = new ParallelOptions() { MaxDegreeOfParallelism = 12};

        public BlockingCollection<Forsendelse> GetForsendelser(string trådnavn, int count, DigitalPostMottaker digitalPostMottaker, string fornavn, string tittel)
        {
            BlockingCollection<Forsendelse> forsendelser = new BlockingCollection<Forsendelse>();

            for (int i = 0; i < count; i++)
            {
                forsendelser.Add(GetForsendelse(digitalPostMottaker, fornavn, tittel, i));
            }
            return forsendelser;
        }

        public Forsendelse GetForsendelse(DigitalPostMottaker digitalPostMottaker, string fornavn, string tittel, int løpenummer)
        {
            //Digital Post
            var digitalPost = new DigitalPostInfo(digitalPostMottaker, String.Format("{0} {1} ({2})", fornavn, tittel, løpenummer), Sikkerhetsnivå.Nivå3, åpningskvittering: false);
            
            //Forsendelse
            string mpcId = "hest";
            var dokumentpakke = Dokumentpakke(fornavn, tittel, løpenummer);
            var forsendelse = new Forsendelse(_behandlingsansvarlig, digitalPost, dokumentpakke, Prioritet.Prioritert, mpcId, "NO");

            return forsendelse;
        }

        private Dokumentpakke Dokumentpakke(string navn, string tittel, int løpenummer)
        {
            string hoveddokument = FileUtility.AbsolutePath("testdata", "hoveddokument", "Hoved"+navn+".txt");
            string vedlegg = FileUtility.AbsolutePath("testdata", "vedlegg", "Vedlegg"+navn+".txt");

            var dokumentpakke = new Dokumentpakke(new Dokument(String.Format("{0} {1} ({2})", navn, tittel, løpenummer), hoveddokument, "text/plain"));
            dokumentpakke.LeggTilVedlegg(new Dokument(navn + "Vedlegg", vedlegg, "text/plain", "EN"));

            return dokumentpakke;
        }
    }
}
