/** 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *         http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System.Security.Cryptography.X509Certificates;
using SikkerDigitalPost.Tester;

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
            var tekniskAvsenderSertifikat = SertifikatUtility.AvsenderSertifkat("8702F5E55217EC88CF2CCBADAC290BB4312594AC");
            var mottakerSertifikat = SertifikatUtility.MottakerSertifikat("7166484C1116D8A32AACD49FD2FB79F06751661D");

            var orgnummerPosten = "984661185";
            var orgnummerDatabehandler = orgnummerPosten;
            var orgnummerBehandlingsansvarlig = orgnummerPosten;
            var orgnummerPostkasse = "996460320";
            var mottakerPersonnummer = "02018090301";
            var mottakerPostkasse = "0000485509";

            return new PostkasseInnstillinger(tekniskAvsenderSertifikat, mottakerSertifikat, orgnummerBehandlingsansvarlig, orgnummerDatabehandler, orgnummerPostkasse, mottakerPersonnummer, mottakerPostkasse);
        }

        public static PostkasseInnstillinger GetPosten()
        {
            var tekniskAvsenderSertifikat = SertifikatUtility.AvsenderSertifkat("8702F5E55217EC88CF2CCBADAC290BB4312594AC");
            var mottakerSertifikat = SertifikatUtility.MottakerSertifikat("B43CAAA0FBEE6C8DA85B47D1E5B7BCAB42AB9ADD");

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
