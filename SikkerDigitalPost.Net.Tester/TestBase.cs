using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SikkerDigitalPost.Net.Domene.Entiteter;
using SikkerDigitalPost.Net.Domene.Entiteter.AsicE.Manifest;

namespace SikkerDigitalPost.Net.Tests
{
    [TestClass]
    public class TestBase
    {
        private static string _testDataMappe = "testdata";
        private static string _vedleggsMappe = "vedlegg";
        private static string _hoveddokumentMappe = "hoveddokument";

        private static string _hoveddokument;
        public static string[] Vedleggsstier;
        protected static Dokument Hoveddokument;
        protected static IEnumerable<Dokument> Vedlegg;

        protected static string SignaturFil = "signatur.xml";

        protected static Organisasjonsnummer OrgNrAvsender;
        protected static Behandlingsansvarlig Behandlingsansvarlig;

        protected static Organisasjonsnummer OrgNrMottaker;
        protected static Mottaker Mottaker;

        protected static DigitalPost DigitalPost;

        protected static Dokumentpakke Dokumentpakke;
        protected static Forsendelse Forsendelse;

        protected static Manifest Manifest;

        public static void Initialiser()
        {
            _testDataMappe = Path.Combine(path1: Path.GetDirectoryName(Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory)), path2: _testDataMappe);

            _vedleggsMappe = Path.Combine(_testDataMappe, _vedleggsMappe);
            _hoveddokumentMappe = Path.Combine(_testDataMappe, _hoveddokumentMappe);

            Vedleggsstier = Directory.GetFiles(_vedleggsMappe);
            _hoveddokument = Directory.GetFiles(_hoveddokumentMappe)[0];
            SignaturFil = Directory.GetFiles(_testDataMappe).Single(f => f.Contains(SignaturFil));

            Hoveddokument = GenererHoveddokument();
            Vedlegg = GenererVedlegg();

            OrgNrAvsender = new Organisasjonsnummer("984661185");
            Behandlingsansvarlig = new Behandlingsansvarlig(OrgNrAvsender);

            OrgNrMottaker = new Organisasjonsnummer("984661185");
            Mottaker = new Mottaker("04036125433", "ove.jonsen#6K5A", new X509Certificate2(), OrgNrMottaker.Iso6523());

            DigitalPost = new DigitalPost(Mottaker, "Ikke-sensitiv tittel");

            Dokumentpakke = new Dokumentpakke(new Dokument("Hoveddokument", _hoveddokument, "text/docx"));
            Dokumentpakke.LeggTilVedlegg(Vedlegg);

            Forsendelse = new Forsendelse(Behandlingsansvarlig, DigitalPost, Dokumentpakke);

            Manifest = new Manifest(Mottaker, Behandlingsansvarlig, Forsendelse);
        }

        static Dokument GenererHoveddokument()
        {
            return new Dokument(Path.GetFileName(_hoveddokument), _hoveddokument, "text/xml");
        }

        static IEnumerable<Dokument> GenererVedlegg()
        {
            return new List<Dokument>(
                    Vedleggsstier.Select(
                        v => new Dokument(Path.GetFileNameWithoutExtension(v), v, "text/" + Path.GetExtension(_hoveddokument))));
        }

        protected Dokumentpakke GenererDokumentpakke()
        {
            var dokumentpakke = new Dokumentpakke(GenererHoveddokument());
            dokumentpakke.LeggTilVedlegg(GenererVedlegg());
            return dokumentpakke;
        }


    }
}
