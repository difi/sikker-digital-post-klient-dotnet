using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SikkerDigitalPost.Domene.Entiteter;
using SikkerDigitalPost.Domene.Entiteter.Aktører;
using SikkerDigitalPost.Domene.Entiteter.AsicE.Manifest;
using SikkerDigitalPost.Domene.Entiteter.AsicE.Signatur;
using SikkerDigitalPost.Domene.Entiteter.Post;
using SikkerDigitalPost.Domene.Entiteter.Varsel;
using SikkerDigitalPost.Klient;
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

        protected static Organisasjonsnummer OrgNrAvsender;
        protected static Behandlingsansvarlig Behandlingsansvarlig;

        protected static Organisasjonsnummer OrgNrMottaker;
        protected static Mottaker Mottaker;

        protected static DigitalPost DigitalPost;

        protected static Dokumentpakke Dokumentpakke;
        protected static Forsendelse Forsendelse;

        internal static Manifest Manifest;
        
        internal static Signatur Signatur;
        protected static X509Certificate2 Sertifikat;

        protected static Databehandler Databehandler;
        internal static AsicEArkiv Arkiv;
        internal static Envelope Envelope;

        public static void Initialiser()
        {
            TestDataMappe = Path.Combine(path1: Path.GetDirectoryName(Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory)), path2: TestDataMappe);

            VedleggsMappe = Path.Combine(TestDataMappe, VedleggsMappe);
            HoveddokumentMappe = Path.Combine(TestDataMappe, HoveddokumentMappe);

            Vedleggsstier = Directory.GetFiles(VedleggsMappe);
            _hoveddokument = Directory.GetFiles(HoveddokumentMappe)[0];
            
            var store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
            store.Open(OpenFlags.ReadOnly);
            Sertifikat = store.Certificates[0];
            store.Close();

            OrgNrAvsender = new Organisasjonsnummer("984661185");
            Behandlingsansvarlig = new Behandlingsansvarlig(OrgNrAvsender);

            OrgNrMottaker = new Organisasjonsnummer("984661185");
            Mottaker = new Mottaker("04036125433", "ove.jonsen#6K5A", Sertifikat, OrgNrMottaker.Iso6523());

            DigitalPost = new DigitalPost(Mottaker, "Ikke-sensitiv tittel");
            DigitalPost.EpostVarsel = new EpostVarsel("epost@sjafjell.no", "Dette er et epostvarsel. En trojansk ... hest.", 0, 7);
            DigitalPost.SmsVarsel = new SmsVarsel("45215454", "Dette er et smsvarsel. En trojansk ... telefon..", 3, 14);
            
            Dokumentpakke = GenererDokumentpakke();
            Forsendelse = new Forsendelse(Behandlingsansvarlig, DigitalPost, Dokumentpakke);

            Manifest = new Manifest(Mottaker, Behandlingsansvarlig, Forsendelse);
            var manifestbygger = new ManifestBygger(Manifest);
            
            manifestbygger.Bygg();

            Signatur = new Signatur(Sertifikat);
            var signaturbygger = new SignaturBygger(Signatur, Forsendelse);
            signaturbygger.Bygg();

            Databehandler = new Databehandler(OrgNrAvsender,Sertifikat);
            Arkiv = new AsicEArkiv(Dokumentpakke, Signatur, Manifest);
            Envelope = new Envelope(Forsendelse,Arkiv,Databehandler);
        }

        private static Dokument GenererHoveddokument()
        {
            return new Dokument(Path.GetFileName(_hoveddokument), _hoveddokument, "text/xml");
        }

        private static IEnumerable<Dokument> GenererVedlegg()
        {
            return new List<Dokument>(
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
