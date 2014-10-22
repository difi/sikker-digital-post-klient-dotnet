using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SikkerDigitalPost.Domene.Entiteter;
using SikkerDigitalPost.Domene.Entiteter.Aktører;
using SikkerDigitalPost.Domene.Entiteter.Post;
using SikkerDigitalPost.Domene.Entiteter.Varsel;
using SikkerDigitalPost.Klient;
using SikkerDigitalPost.Klient.AsicE;
using SikkerDigitalPost.Klient.Envelope;

namespace SikkerDigitalPost.Tester
{
    [TestClass]
    public class TestBase
    {
        private static string _hoveddokument;
        
        protected static string[] Vedleggsstier;
        protected static string TestDataMappe = "testdata";
        protected static string VedleggsMappe = "vedlegg";
        protected static string HoveddokumentMappe = "hoveddokument";

        protected static Dokument Hoveddokument;
        protected static IEnumerable<Dokument> Vedlegg;
        protected static Dokumentpakke Dokumentpakke;

        protected static X509Certificate2 AvsenderSertifikat;
        protected static X509Certificate2 MottakerSertifikat;

        protected static Organisasjonsnummer OrgNrAvsender;
        protected static Behandlingsansvarlig Behandlingsansvarlig;
        protected static Databehandler Databehandler;

        protected static Organisasjonsnummer OrgNrMottaker;
        protected static Mottaker Mottaker;

        protected static DigitalPost DigitalPost;
        protected static Forsendelse Forsendelse;
        
        internal static AsicEArkiv Arkiv;
        internal static ForretningsmeldingEnvelope Envelope;
        internal static GuidHandler GuidHandler;

        public static void Initialiser()
        {
            //Dokumentpakke
            TestDataMappe = Path.Combine(path1: Path.GetDirectoryName(Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory)), path2: TestDataMappe);
            VedleggsMappe = Path.Combine(TestDataMappe, VedleggsMappe);
            HoveddokumentMappe = Path.Combine(TestDataMappe, HoveddokumentMappe);

            Vedleggsstier = Directory.GetFiles(VedleggsMappe);
            _hoveddokument = Directory.GetFiles(HoveddokumentMappe)[0];
            
            Dokumentpakke = GenererDokumentpakke();
            
            SettSertifikater();

            //Avsender og mottaker
            OrgNrAvsender = new Organisasjonsnummer("984661185");
            Behandlingsansvarlig = new Behandlingsansvarlig(OrgNrAvsender);
            Databehandler = new Databehandler(OrgNrAvsender, AvsenderSertifikat);

            OrgNrMottaker = new Organisasjonsnummer("984661185");
            Mottaker = new Mottaker("04036125433", "ove.jonsen#6K5A", MottakerSertifikat, OrgNrMottaker.Iso6523());
            
            //DigitalPost og forsendelse
            DigitalPost = new DigitalPost(Mottaker, "Ikke-sensitiv tittel");
            DigitalPost.EpostVarsel = new EpostVarsel("epost@sjafjell.no", "Dette er et epostvarsel. En trojansk ... hest.", 0, 7);
            DigitalPost.SmsVarsel = new SmsVarsel("45215454", "Dette er et smsvarsel. En trojansk ... telefon..", 3, 14);
            
            Forsendelse = new Forsendelse(Behandlingsansvarlig, DigitalPost, Dokumentpakke);
            
            //Guids, AsicEArkiv og Envelope
            GuidHandler = new GuidHandler();
            Arkiv = new AsicEArkiv(Forsendelse, GuidHandler, Databehandler.Sertifikat);
            Envelope = new ForretningsmeldingEnvelope(new EnvelopeSettings(Forsendelse, Arkiv, Databehandler, GuidHandler));
        }

        private static void SettSertifikater()
        {
            var storeMy = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            storeMy.Open(OpenFlags.ReadOnly);
            AvsenderSertifikat = storeMy.Certificates
                .Find(X509FindType.FindByThumbprint, "8702F5E55217EC88CF2CCBADAC290BB4312594AC", true)[0];
            storeMy.Close();

            var storeTrusted = new X509Store(StoreName.TrustedPeople, StoreLocation.CurrentUser);
            storeTrusted.Open(OpenFlags.ReadOnly);
            MottakerSertifikat =
                storeTrusted.Certificates
                .Find(X509FindType.FindByThumbprint, "B43CAAA0FBEE6C8DA85B47D1E5B7BCAB42AB9ADD", true)[0];
            storeTrusted.Close();
        }

        private static Dokument GenererHoveddokument()
        {
            return Hoveddokument = new Dokument(Path.GetFileName(_hoveddokument), _hoveddokument, "text/xml");
        }

        private static IEnumerable<Dokument> GenererVedlegg()
        {
            return Vedlegg = new List<Dokument>(
                    Vedleggsstier.Select(
                        v => new Dokument(Path.GetFileNameWithoutExtension(v), v, "text/" + Path.GetExtension(_hoveddokument))));
        }

        private static Dokumentpakke GenererDokumentpakke()
        {
            var dokumentpakke = new Dokumentpakke(GenererHoveddokument());
            dokumentpakke.LeggTilVedlegg(GenererVedlegg());
            return dokumentpakke;
        }
    }
}
