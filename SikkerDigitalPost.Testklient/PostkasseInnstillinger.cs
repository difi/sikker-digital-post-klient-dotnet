using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace SikkerDigitalPost.Testklient
{
    public class PostkasseInnstillinger
    {
        /*
        * Følgende sertifikater må brukes for å kunne sende digital post
        * 
        * - Mottagersertifikat brukes for å kryptere og signere dokumentpakke som skal til mottagerens postkasse.
        * - TekniskAvsenderSertifikat brukes for sikker kommunikasjon med meldingsformidler.
        */
        public X509Certificate2 Avsendersertifikat { get; set; }
        public X509Certificate2 Mottakersertifikat { get; set; }

        public string OrgNummerBehandlingsansvarlig { get; set; }
        public string OrgNummerDatabehandler { get; set; }
        public string OrgnummerPostkasse { get; set; }
        public string Personnummer { get; set; }
        public string Postkasseadresse { get; set; }

        private PostkasseInnstillinger(X509Certificate2 avsendersertifikat, X509Certificate2 mottakersertifikat, string orgNummerBehandlingsansvarlig, string orgNummerDatabehandler, string orgnummerPostkasse,
            string personnummer, string postkasseadresse)
        {
            Avsendersertifikat = avsendersertifikat;
            Mottakersertifikat = mottakersertifikat;
            OrgNummerBehandlingsansvarlig = orgNummerBehandlingsansvarlig;
            OrgNummerDatabehandler = orgNummerDatabehandler;
            OrgnummerPostkasse = orgnummerPostkasse;
            Personnummer = personnummer;
            Postkasseadresse = postkasseadresse;
        }

        public static PostkasseInnstillinger GetEboks()
        {
            X509Certificate2 tekniskAvsenderSertifikat;
            {
                X509Store store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
                store.Open(OpenFlags.ReadOnly);
                tekniskAvsenderSertifikat = store.Certificates.Find(
                    X509FindType.FindByThumbprint, "8702F5E55217EC88CF2CCBADAC290BB4312594AC", true)[0];
                store.Close();
            }

            X509Certificate2 mottakerSertifikat;
            {
                X509Store store2 = new X509Store(StoreName.TrustedPeople, StoreLocation.CurrentUser);
                store2.Open(OpenFlags.ReadOnly);
                mottakerSertifikat =
                    store2.Certificates.Find(X509FindType.FindByThumbprint, "7166484C1116D8A32AACD49FD2FB79F06751661D", true)[0];
                store2.Close();
            }

            var orgnummerPosten = "984661185";
            var orgnummerDatabehandler = orgnummerPosten;
            var orgnummerBehandlingsansvarlig = orgnummerPosten;
            var orgnummerPostkasse = "996460320";
            var mottakerPersonnummer = "01043100358";
            var mottakerPostkasse = "0000485509";

            return new PostkasseInnstillinger(tekniskAvsenderSertifikat, mottakerSertifikat, orgnummerBehandlingsansvarlig, orgnummerDatabehandler, orgnummerPostkasse, mottakerPersonnummer, mottakerPostkasse);
        }

        public static PostkasseInnstillinger GetPosten()
        {
            X509Certificate2 tekniskAvsenderSertifikat;
            {
                X509Store store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
                store.Open(OpenFlags.ReadOnly);
                tekniskAvsenderSertifikat = store.Certificates.Find(
                    X509FindType.FindByThumbprint, "8702F5E55217EC88CF2CCBADAC290BB4312594AC", true)[0];
                store.Close();
            }

            X509Certificate2 mottakerSertifikat;
            {
                X509Store store2 = new X509Store(StoreName.TrustedPeople, StoreLocation.CurrentUser);
                store2.Open(OpenFlags.ReadOnly);
                mottakerSertifikat =
                    store2.Certificates.Find(X509FindType.FindByThumbprint, "B43CAAA0FBEE6C8DA85B47D1E5B7BCAB42AB9ADD", true)[0];
                store2.Close();
            }

            var orgnummerPosten = "984661185";
            var orgnummerDatabehandler = orgnummerPosten;
            var orgnummerBehandlingsansvarlig = orgnummerPosten;
            var orgnummerPostkasse = orgnummerPosten;
            var mottakerPersonnummer = "04036125433";
            var mottakerPostkasse = "ove.jonsen#6K5A";

            return new PostkasseInnstillinger(tekniskAvsenderSertifikat, mottakerSertifikat, orgnummerBehandlingsansvarlig, orgnummerDatabehandler, orgnummerPostkasse, mottakerPersonnummer, mottakerPostkasse);
        }
    }
}
