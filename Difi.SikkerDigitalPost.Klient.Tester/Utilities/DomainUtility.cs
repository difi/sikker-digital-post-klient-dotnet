using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using Difi.SikkerDigitalPost.Klient.Api;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Aktører;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.FysiskPost;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Interface;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer.Forretning;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Post;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Varsel;
using Difi.SikkerDigitalPost.Klient.Domene.Enums;
using Difi.SikkerDigitalPost.Klient.Envelope;
using Difi.SikkerDigitalPost.Klient.Envelope.Forretningsmelding;
using Difi.SikkerDigitalPost.Klient.Internal.AsicE;
using Difi.SikkerDigitalPost.Klient.Utilities;
using Difi.SikkerDigitalPost.Klient.XmlValidering;
using Digipost.Api.Client.Shared.Resources.Resource;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Difi.SikkerDigitalPost.Klient.Tester.Utilities
{
    internal static class DomainUtility
    {
        internal static readonly ResourceUtility ResourceUtility = new ResourceUtility(Assembly.GetExecutingAssembly(), "testdata");

        private static readonly GuidUtility GuidUtility = new GuidUtility();

        internal static Dokumentpakke GetDokumentpakkeWithoutAttachments()
        {
            return new Dokumentpakke(GetHoveddokumentSimple());
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
            return new Avsender(Organisasjonsnummer());
        }

        internal static Organisasjonsnummer Organisasjonsnummer()
        {
            return new Organisasjonsnummer("988015814");
        }

        internal static string GetPersonnummerMottaker()
        {
            return "01043100358";
        }

        internal static string GetDigipostadresseMottaker()
        {
            return "dangfart.utnes#1BK5";
        }

        public static Organisasjonsnummer OrganisasjonsnummerMeldingsformidler()
        {
            return GetOrganisasjonsnummerPostkasse();
        }

        internal static Organisasjonsnummer GetOrganisasjonsnummerPostkasse()
        {
            return new Organisasjonsnummer("984661185");
        }

        internal static DigitalPostMottaker GetDigitalPostMottaker()
        {
            return new DigitalPostMottaker(GetPersonnummerMottaker(), GetDigipostadresseMottaker(), GetMottakerCertificate(), GetOrganisasjonsnummerPostkasse());
        }

        internal static FysiskPostMottaker GetFysiskPostMottaker()
        {
            return new FysiskPostMottaker("Testbruker i Tester .NET", new NorskAdresse("0001", "Testekommunen"), GetMottakerCertificate(), GetOrganisasjonsnummerPostkasse());
        }

        internal static DigitalPostMottaker GetDigitalPostMottakerWithTestCertificate()
        {
            return new DigitalPostMottaker(GetPersonnummerMottaker(), GetDigipostadresseMottaker(), GetReceiverUnitTestsCertificate(), GetOrganisasjonsnummerPostkasse());
        }

        internal static FysiskPostMottaker GetFysiskPostMottakerWithTestCertificate()
        {
            return new FysiskPostMottaker("Testbruker i Tester .NET med testsertifikat", new NorskAdresse("0001", "Testekommunen"), GetReceiverUnitTestsCertificate(), GetOrganisasjonsnummerPostkasse());
        }

        internal static FysiskPostReturmottaker GetFysiskPostReturMottaker()
        {
            return new FysiskPostReturmottaker("Testbruker i Tester .NET", new NorskAdresse("0001", "Testekommunen"));
        }

        internal static Databehandler GetDatabehandler()
        {
            return new Databehandler(GetAvsender().Organisasjonsnummer, GetAvsenderCertificate());
        }

        internal static Databehandler GetDatabehandlerWithTestCertificate()
        {
            return new Databehandler(GetAvsender().Organisasjonsnummer, GetAvsenderEnhetstesterSertifikat());
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

        internal static DigitalPostInfo GetDigitalPostInfoWithTestCertificate()
        {
            return new DigitalPostInfo(GetDigitalPostMottakerWithTestCertificate(), "Ikke-sensitiv tittel");
        }

        internal static FysiskPostInfo GetFysiskPostInfoSimple()
        {
            return new FysiskPostInfo(GetFysiskPostMottaker(), Posttype.A, Utskriftsfarge.Farge,
                Posthåndtering.DirekteRetur, GetFysiskPostReturMottaker());
        }

        internal static Forsendelse GetForsendelseSimple()
        {
            return new Forsendelse(GetAvsender(), GetDigitalPostInfoSimple(), GetDokumentpakkeWithoutAttachments(), Prioritet.Normal, Guid.NewGuid().ToString());
        }
        
        internal static Forsendelse GetForsendelseWithDataType()
        {
            return new Forsendelse(GetAvsender(), GetDigitalPostInfoSimple(), GetDokumentpakkeWithoutAttachments(), Prioritet.Normal, Guid.NewGuid().ToString());
        }

        internal static Forsendelse GetForsendelseWithTestCertificate()
        {
            return new Forsendelse(GetAvsender(), GetDigitalPostInfoWithTestCertificate(), GetDokumentpakkeWithoutAttachments(), Prioritet.Normal, Guid.NewGuid().ToString());
        }

        internal static Forsendelse GetFysiskPostSimple()
        {
            return new Forsendelse(GetAvsender(), GetFysiskPostInfoSimple(), GetDokumentpakkeWithoutAttachments(), Prioritet.Normal, Guid.NewGuid().ToString());
        }

        internal static Forsendelse GetDigitalDigitalPostWithNotificationMultipleDocumentsAndHigherSecurity(int antallVedlegg = 5)
        {
            return new Forsendelse(GetAvsender(), GetDigitalPostInfoWithVarsel(), GetDokumentpakkeWithMultipleVedlegg(antallVedlegg), Prioritet.Normal, Guid.NewGuid().ToString());
        }

        internal static DocumentBundle GetAsiceArchiveSimple()
        {
            return AsiceGenerator.Create(GetForsendelseSimple(), new GuidUtility(), GetAvsenderCertificate(), GetKlientkonfigurasjon());
        }

        internal static AsiceArchive GetAsiceArchive(Forsendelse message)
        {
            var manifest = new Manifest(message);
            var signature = new Signature(message, manifest, GetAvsenderEnhetstesterSertifikat());
            var cryptographicCertificate = message.PostInfo.Mottaker.Sertifikat;

            var asiceAttachables = new List<IAsiceAttachable>();
            asiceAttachables.AddRange(message.Dokumentpakke.Vedlegg);
            asiceAttachables.Add(message.Dokumentpakke.Hoveddokument);
            asiceAttachables.Add(manifest);
            asiceAttachables.Add(signature);

            return new AsiceArchive(cryptographicCertificate, new GuidUtility(), new List<AsiceAttachableProcessor>(), asiceAttachables.ToArray());
        }

        internal static ForretningsmeldingEnvelope GetForretningsmeldingEnvelopeWithTestTestCertificate()
        {
            var envelopeSettings = new EnvelopeSettings(
                GetForsendelseWithTestCertificate(),
                GetAsiceArchiveSimple(),
                GetDatabehandlerWithTestCertificate(),
                GuidUtility,
                new Klientkonfigurasjon(Miljø.FunksjoneltTestmiljø));
            return new ForretningsmeldingEnvelope(envelopeSettings);
        }

        internal static ForretningsmeldingEnvelope GetForretningsmeldingEnvelope()
        {
            var envelopeSettings = new EnvelopeSettings(
                GetForsendelseSimple(),
                GetAsiceArchiveSimple(),
                GetDatabehandler(),
                GuidUtility,
                new Klientkonfigurasjon(Miljø.FunksjoneltTestmiljø));
            return new ForretningsmeldingEnvelope(envelopeSettings);
        }

        internal static SikkerDigitalPostKlient GetSikkerDigitalPostKlientQaOffentlig()
        {
            var serviceProvider = LoggingUtility.CreateServiceProviderAndSetUpLogging();
            return new SikkerDigitalPostKlient(GetDatabehandler(), new Klientkonfigurasjon(Miljø.FunksjoneltTestmiljø), serviceProvider.GetService<ILoggerFactory>());
        }
        
        internal static X509Certificate2 GetAvsenderEnhetstesterSertifikat()
        {
            return GetEternalTestCertificateMedPrivateKey();
        }

        internal static X509Certificate2 GetReceiverUnitTestsCertificate()
        {
            return GetEternalTestCertificateWithoutPrivateKey();
        }

        private static X509Certificate2 GetEternalTestCertificateWithoutPrivateKey()
        {
            return new X509Certificate2(ResourceUtility.ReadAllBytes("sertifikater", "enhetstester", "difi-enhetstester.cer"), "", X509KeyStorageFlags.Exportable);
        }

        private static X509Certificate2 GetEternalTestCertificateMedPrivateKey()
        {
            return new X509Certificate2(ResourceUtility.ReadAllBytes("sertifikater", "enhetstester", "difi-enhetstester.p12"), "Qwer1234", X509KeyStorageFlags.Exportable);
        }

        internal static X509Certificate2 GetAvsenderCertificate()
        {
            return CertificateReader.ReadCertificate();
        }

        internal static X509Certificate2 GetMottakerCertificate()
        {
            return new X509Certificate2(ResourceUtility.ReadAllBytes("sertifikater", "test", "posten-test.pem"));
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
            return new Klientkonfigurasjon(Miljø.FunksjoneltTestmiljø);
        }
    }
}