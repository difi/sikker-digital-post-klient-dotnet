using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using ApiClientShared;
using ApiClientShared.Enums;
using Difi.SikkerDigitalPost.Klient.Api;
using Difi.SikkerDigitalPost.Klient.AsicE;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Aktører;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.FysiskPost;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Post;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Varsel;
using Difi.SikkerDigitalPost.Klient.Domene.Enums;
using Difi.SikkerDigitalPost.Klient.Envelope;
using Difi.SikkerDigitalPost.Klient.Envelope.Forretningsmelding;
using Difi.SikkerDigitalPost.Klient.Tester.Properties;
using Difi.SikkerDigitalPost.Klient.Utilities; 

namespace Difi.SikkerDigitalPost.Klient.Tester.Utilities
{
    /// <summary>
    /// Hjelpeklasse for instansiering av domeneobjekter. Klassen kan virke tilstandsløs, og vil for alle praktiske formål være det,
    /// da man vil få det samme tilbake hver gang / deterministisk. Likevel er det viktig å vite at filobjekter vil leses fra disk kun èn
    /// gang for økt ytelse.
    /// </summary>
    internal static class DomeneUtility
    {
        internal static readonly ResourceUtility ResourceUtility = new ResourceUtility("Difi.SikkerDigitalPost.Klient.Tester.testdata");
        
        private static readonly GuidUtility GuidUtility = new GuidUtility();
        
        private static readonly string fileExtension;

        private static Dokument _hoveddokument;
        
        private static IEnumerable<Dokument> _vedlegg;
        
        private static Avsender _avsender;
        
        private static DigitalPostMottaker _digitalPostMottaker; 
        
        private static FysiskPostMottaker _fysiskPostMottaker;
        private static FysiskPostReturmottaker _fysiskPostReturmottaker;

        private static X509Certificate2 _avsenderSertifikat;
        
        private static X509Certificate2 _mottakerSertifikat;
        

        internal static Dokumentpakke GetDokumentpakkeUtenVedlegg()
        {
            var dokumentpakke = new Dokumentpakke(GetHoveddokumentEnkel());
            return dokumentpakke;
        }

        internal static Dokumentpakke GetDokumentpakkeMedFlereVedlegg(int antall = 3)
        {
            var dokumentpakke = new Dokumentpakke(GetHoveddokumentEnkel());
            dokumentpakke.LeggTilVedlegg(GetVedlegg(antall));
            return dokumentpakke;
        }

        internal static Dokument GetHoveddokumentEnkel()
        {
            if (_hoveddokument != null)
            {
                return _hoveddokument;
            }

            var hoveddokumentMappe = "hoveddokument";
            var hoveddokument = ResourceUtility.GetFiles(hoveddokumentMappe).ElementAt(0);

            var bytes = ResourceUtility.ReadAllBytes(false, hoveddokument);
            var fileName = ResourceUtility.GetFileName(hoveddokument);
            
            return _hoveddokument = new Dokument("Hoveddokument", bytes, "text/xml", "NO", fileName);
        }

        internal static string[] GetVedleggsFilerStier()
        {
            const string VedleggsMappe = "vedlegg";
            
            return ResourceUtility.GetFiles(VedleggsMappe).ToArray();
        }

        internal static IEnumerable<Dokument> GetVedlegg(int maksAntall)
        {
            if (_vedlegg != null)
            {
                return _vedlegg;
            }

            var count = 0;
           
            _vedlegg = new List<Dokument>(
                    GetVedleggsFilerStier().Select(
                        v => new Dokument("Vedlegg" + count++,
                            ResourceUtility.ReadAllBytes(false, v), 
                            "text/" + fileExtension, 
                            "NO", ResourceUtility.GetFileName(v))));

            return _vedlegg.Take(maksAntall);

        }

        internal static Avsender GetAvsender()
        {
            if (_avsender != null)
            {
                return _avsender;
            }

            var orgNrAvsender = new Organisasjonsnummer(Settings.Default.OrganisasjonsnummerAvsender);
            
            return _avsender = new Avsender(orgNrAvsender);
        }
        
        internal static DigitalPostMottaker GetDigitalPostMottaker()
        {
            if (_digitalPostMottaker != null)
            {
                return _digitalPostMottaker;
            }

        return _digitalPostMottaker = new DigitalPostMottaker(Settings.Default.PersonnummerMottaker, Settings.Default.DigitalPostkasseAdresseMottaker, GetMottakerSertifikat(), Settings.Default.OrganisasjonsnummerPostkasse);
        }

        internal static FysiskPostMottaker GetFysiskPostMottaker()
        {
            if (_fysiskPostMottaker != null)
            {
                return _fysiskPostMottaker;
            }

            return
                _fysiskPostMottaker =
                    new FysiskPostMottaker("Testbruker i Tester .NET", new NorskAdresse("0001", "Testekommunen"), GetMottakerSertifikat(), Settings.Default.OrganisasjonsnummerPostkasse);
        }

        internal static FysiskPostReturmottaker GetFysiskPostReturMottaker()
        {
            if (_fysiskPostReturmottaker != null)
            {
                return _fysiskPostReturmottaker;
            }
            return _fysiskPostReturmottaker = 
                new FysiskPostReturmottaker("Testbruker i Tester .NET", new NorskAdresse("0001", "Testekommunen"));
        }

        internal static Databehandler GetDatabehandler()
        {
            return new Databehandler(GetAvsender().Organisasjonsnummer, GetMottakerSertifikat());
        }

        internal static DigitalPostInfo GetDigitalPostInfoMedVarsel()
        {
            return new DigitalPostInfo(GetDigitalPostMottaker(), "Ikke-sensitiv tittel")
            {
                EpostVarsel = new EpostVarsel("tull@ball.no", "Dette er et epostvarsel fra Enhentstester .NET", 0, 7),
                SmsVarsel = new SmsVarsel("45215454", "Dette er et smsvarsel fra Enhetstester .NET", 3, 14)
            };
        }

        internal static DigitalPostInfo GetDigitalPostInfoEnkel()
        {
            return new DigitalPostInfo(GetDigitalPostMottaker(), "Ikke-sensitiv tittel");
        }

        internal static FysiskPostInfo GetFysiskPostInfoEnkel()
        {
            return new FysiskPostInfo(GetFysiskPostMottaker(), Posttype.A, Utskriftsfarge.Farge,
                Posthåndtering.DirekteRetur, GetFysiskPostMottaker());
        } 

        internal static Forsendelse GetDigitalForsendelseEnkel()
        {
            return new Forsendelse(GetAvsender(), GetDigitalPostInfoEnkel(), GetDokumentpakkeUtenVedlegg(), Prioritet.Normal,Guid.NewGuid().ToString());
        }

        internal static Forsendelse GetFysiskForsendelseEnkel()
        {
            return new Forsendelse(GetAvsender(), GetFysiskPostInfoEnkel(), GetDokumentpakkeUtenVedlegg(), Prioritet.Normal, Guid.NewGuid().ToString());
        }

        internal static Forsendelse GetDigitalForsendelseVarselFlereDokumenterHøyereSikkerhet()
        {
            return new Forsendelse(GetAvsender(), GetDigitalPostInfoMedVarsel(), GetDokumentpakkeMedFlereVedlegg(), Prioritet.Normal, Guid.NewGuid().ToString());
        }

        internal static AsicEArkiv GetAsicEArkivEnkel()
        {
            
            return new AsicEArkiv(GetDigitalForsendelseEnkel(), GuidUtility, GetAvsenderSertifikat());
        }

        internal static ForretningsmeldingEnvelope GetForretningsmeldingEnvelope()
        {
            var envelopeSettings = new EnvelopeSettings(
                GetDigitalForsendelseEnkel(), 
                GetAsicEArkivEnkel(), 
                GetDatabehandler(),
                GuidUtility, 
                new Klientkonfigurasjon());
           return new ForretningsmeldingEnvelope(envelopeSettings);
        }
        
        internal static SikkerDigitalPostKlient GetSikkerDigitalPostKlientQaOffentlig()
        {
            return new SikkerDigitalPostKlient(GetDatabehandler(), new Klientkonfigurasjon()
            {
                MeldingsformidlerUrl = new Uri(Settings.Default.UrlMeldingsformidler)
            });
        }

        internal static X509Certificate2 GetAvsenderSertifikat()
        {
            if (_avsenderSertifikat != null)
            {
                return _avsenderSertifikat;
            }

            return _avsenderSertifikat = CertificateUtility.SenderCertificate("8702F5E55217EC88CF2CCBADAC290BB4312594AC", Language.Norwegian);
        }

        internal static X509Certificate2 GetMottakerSertifikat()
        {
            if (_mottakerSertifikat != null)
            {
                return _mottakerSertifikat;
            }

            return _mottakerSertifikat = CertificateUtility.SenderCertificate("8702F5E55217EC88CF2CCBADAC290BB4312594AC", Language.Norwegian);
        }

    }
}
