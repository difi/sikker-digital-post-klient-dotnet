using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Difi.SikkerDigitalPost.Klient.Api;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Aktører;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.FysiskPost;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer.Forretning;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Post;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Varsel;
using Difi.SikkerDigitalPost.Klient.Domene.Enums;
using Difi.SikkerDigitalPost.Klient.Tester.Api;
using Difi.SikkerDigitalPost.Klient.Utilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Difi.SikkerDigitalPost.Klient.Tester.Utilities
{
    internal static class DomainUtility
    {
        internal static readonly ResourceUtility ResourceUtility = new ResourceUtility(Assembly.GetExecutingAssembly(), "testdata");

        internal static Dokumentpakke GetDokumentpakkeWithoutAttachments()
        {
            return new Dokumentpakke(GetHoveddokumentSimple());
        }

        internal static Dokumentpakke GetDokumentpakkeMedEHFDokument()
        {
            return new Dokumentpakke(GetHoveddokumentEHF());
        }
        
        internal static Dokumentpakke GetDokumentpakkeWithMultipleVedlegg(int antall = 3)
        {
            var dokumentpakke = new Dokumentpakke(GetHoveddokumentSimple());
            dokumentpakke.LeggTilVedlegg(GetVedlegg(antall));
            return dokumentpakke;
        }

        internal static Dokument GetHoveddokumentSimple()
        {
            return new Dokument("Hoveddokument", ResourceUtility.ReadAllBytes("hoveddokument", "Hoveddokument.pdf"), "application/pdf");
        }
        
        internal static Dokument GetHoveddokumentEHF()
        {
            return new Dokument("ehf_BII05_T10_gyldig_faktura", ResourceUtility.ReadAllBytes("hoveddokument", "ehf_BII05_T10_gyldig_faktura.xml"), "application/ehf+xml");
        }

        internal static string[] GetVedleggFilesPaths()
        {
            const string vedleggsMappe = "vedlegg";

            return ResourceUtility.GetFiles(vedleggsMappe).ToArray();
        }

        internal static IEnumerable<Dokument> GetVedlegg(int antall = 5)
        {

            var vedleggTxt0 = new Dokument("Vedlegg", ResourceUtility.ReadAllBytes("vedlegg", "Vedlegg.txt"), "text/plain");
            var vedleggDocx = new Dokument("Vedleggsgris", ResourceUtility.ReadAllBytes("vedlegg", "VedleggsGris.docx"), "application/vnd.openxmlformats-officedocument.wordprocessingml.document");
            var vedleggPdf = new Dokument("Vedleggshjelm", ResourceUtility.ReadAllBytes("vedlegg", "VedleggsHjelm.pdf"), "application/pdf");
            var vedleggTxt1 = new Dokument("Vedlegg", ResourceUtility.ReadAllBytes("vedlegg", "Vedlegg.txt"), "text/plain");
            var vedleggTxt2 = new Dokument("Vedlegg", ResourceUtility.ReadAllBytes("vedlegg", "Vedlegg.txt"), "text/plain");

            var vedlegg = new[] {vedleggTxt0, vedleggDocx, vedleggPdf, vedleggTxt1, vedleggTxt2};

            if (antall <= 5)
            {
                return vedlegg.Take(antall);
            }
            else
            {
                var vedleggbatch = new List<Dokument>();
                for (var i = 0; i < antall; i++)
                {
                    var element = vedlegg.ElementAt(i % vedlegg.Length);
                    vedleggbatch.Add(new Dokument(element.Tittel, element.Bytes, element.MimeType, "NO", $"{i}-{element.Filnavn}"));
                }
                return vedleggbatch;
            }
        }

        internal static string GetMimeType(string fileName)
        {
            return "text/plain";
        }

        internal static Avsender GetAvsender()
        {
            return new Avsender(BringOrganisasjonsnummer());
        }

        internal static Organisasjonsnummer PostenOrganisasjonsnummer()
        {
            var posten = "984661185";
            return new Organisasjonsnummer(posten);
        }
        
        internal static Organisasjonsnummer BringOrganisasjonsnummer()
        {
            var bring = "988015814";
            return new Organisasjonsnummer(bring);
        }

        internal static string GetPersonnummerMottaker()
        {
            return "01043100358";
        }

        internal static string GetDigipostadresseMottaker()
        {
            return "dangfart.utnes#1BK5";
        }

        internal static DigitalPostMottaker GetDigitalPostMottaker()
        {
            return new DigitalPostMottaker(GetPersonnummerMottaker());
        }

        internal static FysiskPostMottaker GetFysiskPostMottaker()
        {
            return new FysiskPostMottaker("Testbruker i Tester .NET", new NorskAdresse("0001", "Testekommunen") { Adresselinje1 = "Testgate" });
        }

        internal static FysiskPostReturmottaker GetFysiskPostReturMottaker()
        {
            return new FysiskPostReturmottaker("Testbruker i Tester .NET", new NorskAdresse("0001", "Testekommunen"));
        }

        internal static Databehandler GetDatabehandler()
        {
            return new Databehandler(GetAvsender().Organisasjonsnummer);
        }

        internal static DigitalPostInfo GetDigitalPostInfoWithVarsel()
        {
            return new DigitalPostInfo(GetDigitalPostMottaker(), "Ikke-sensitiv tittel")
            {
                EpostVarsel = new EpostVarsel("tull@ball.no", "Dette er et epostvarsel fra Enhentstester .NET", 0, 7),
                SmsVarsel = new SmsVarsel("45215454", "Dette er et smsvarsel fra Enhetstester .NET", 3, 14)
            };
        }

        internal static DigitalPostInfo GetDigitalPostInfoSimple()
        {
            return new DigitalPostInfo(GetDigitalPostMottaker(), "Ikke-sensitiv tittel");
        }

        internal static FysiskPostInfo GetFysiskPostInfoSimple()
        {
            var fysiskPostMottakerPersonnummer = "27127000293";
            return new FysiskPostInfo(fysiskPostMottakerPersonnummer, GetFysiskPostMottaker(), Posttype.A, Utskriftsfarge.Farge,
                Posthåndtering.DirekteRetur, GetFysiskPostReturMottaker());
        }

        internal static Forsendelse GetForsendelseSimple()
        {
            return new Forsendelse(GetAvsender(), GetDigitalPostInfoWithVarsel(), GetDokumentpakkeWithoutAttachments(), Prioritet.Normal, Guid.NewGuid().ToString());
        }

        internal static Forsendelse GetForsendelseWithEHF()
        {
            return new Forsendelse(GetAvsender(), GetDigitalPostInfoWithVarsel(), GetDokumentpakkeMedEHFDokument(), Prioritet.Normal, Guid.NewGuid().ToString());
        }
        
        internal static Forsendelse GetForsendelseForDataType()
        {
            return new Forsendelse(GetAvsender(), GetDigitalPostInfoWithVarsel(), GetDokumentpakkeWithoutAttachments(), Prioritet.Normal, Guid.NewGuid().ToString());
        }

        internal static Forsendelse GetFysiskPostSimple()
        {
            return new Forsendelse(GetAvsender(), GetFysiskPostInfoSimple(), GetDokumentpakkeWithoutAttachments(), Prioritet.Normal, Guid.NewGuid().ToString());
        }

        internal static Forsendelse GetDigitalDigitalPostWithNotificationMultipleDocumentsAndHigherSecurity(int antallVedlegg = 5)
        {
            return new Forsendelse(GetAvsender(), GetDigitalPostInfoWithVarsel(), GetDokumentpakkeWithMultipleVedlegg(antallVedlegg), Prioritet.Normal, Guid.NewGuid().ToString());
        }

        internal static SikkerDigitalPostKlient GetSikkerDigitalPostKlientIPLocalHost()
        {
            var serviceProvider = LoggingUtility.CreateServiceProviderAndSetUpLogging();
            return new SikkerDigitalPostKlient(GetDatabehandler(), new Klientkonfigurasjon(SikkerDigitalPostKlientTests.IntegrasjonsPunktLocalHostMiljø), serviceProvider.GetService<ILoggerFactory>());
        }

        internal static Leveringskvittering GetLeveringskvittering()
        {
            var meldingsId = "MeldingsId";
            var konversasjonsId = Guid.NewGuid();
            var bodyReferenceUri = "bodyReferenceUri";
            var digestValue = "digestValue";

            return new Leveringskvittering(meldingsId, konversasjonsId, bodyReferenceUri, digestValue);
        }

        internal static Mottakskvittering GetMottakskvittering()
        {
            var meldingsId = "MeldingsId";
            var konversasjonsId = Guid.NewGuid();
            var bodyReferenceUri = "bodyReferenceUri";
            var digestValue = "digestValue";

            return new Mottakskvittering(meldingsId, konversasjonsId, bodyReferenceUri, digestValue);
        }

        public static Returpostkvittering GetReturpostkvittering()
        {
            var meldingsId = "MeldingsId";
            var konversasjonsId = Guid.NewGuid();
            var bodyReferenceUri = "bodyReferenceUri";
            var digestValue = "digestValue";

            return new Returpostkvittering(meldingsId, konversasjonsId, bodyReferenceUri, digestValue);
        }

        public static VarslingFeiletKvittering GetVarslingFeiletKvittering()
        {
            var meldingsId = "MeldingsId";
            var konversasjonsId = Guid.NewGuid();
            var bodyReferenceUri = "bodyReferenceUri";
            var digestValue = "digestValue";

            return new VarslingFeiletKvittering(meldingsId, konversasjonsId, bodyReferenceUri, digestValue);
        }

        public static Åpningskvittering GetÅpningskvittering()
        {
            var meldingsId = "MeldingsId";
            var konversasjonsId = Guid.NewGuid();
            var bodyReferenceUri = "bodyReferenceUri";
            var digestValue = "digestValue";

            return new Åpningskvittering(meldingsId, konversasjonsId, bodyReferenceUri, digestValue);
        }

        public static Feilmelding GetFeilmelding()
        {
            var meldingsId = "MeldingsId";
            var konversasjonsId = Guid.NewGuid();
            var bodyReferenceUri = "bodyReferenceUri";
            var digestValue = "digestValue";

            return new Feilmelding(meldingsId, konversasjonsId, bodyReferenceUri, digestValue);
        }

        public static Klientkonfigurasjon GetKlientkonfigurasjon()
        {
            return new Klientkonfigurasjon(SikkerDigitalPostKlientTests.IntegrasjonsPunktLocalHostMiljø);
        }
    }
}
