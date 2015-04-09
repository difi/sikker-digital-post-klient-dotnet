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

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Instrumentation;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using DigipostApiClientShared;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SikkerDigitalPost.Domene.Entiteter;
using SikkerDigitalPost.Domene.Entiteter.Aktører;
using SikkerDigitalPost.Domene.Entiteter.Post;
using SikkerDigitalPost.Domene.Entiteter.Varsel;
using SikkerDigitalPost.Klient;
using SikkerDigitalPost.Klient.AsicE;
using SikkerDigitalPost.Klient.Envelope;
using SikkerDigitalPost.Klient.Envelope.Forretningsmelding;

namespace SikkerDigitalPost.Tester
{
    [TestClass]
    public class TestBase
    {
        static readonly ResourceUtility _resourceUtility = new ResourceUtility("SikkerDigitalPost.Tester.testdata");

        private static string _hoveddokument;

        protected static string[] Vedleggsstier;
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
        protected static DigitalPostMottaker DigitalPostMottaker;

        protected static DigitalPostInfo DigitalPostInfo;
        protected static Forsendelse Forsendelse;

        internal static AsicEArkiv Arkiv;
        internal static ForretningsmeldingEnvelope Envelope;
        internal static GuidHandler GuidHandler;


        public static void Initialiser()
        {

            //Dokumentpakke
            Vedleggsstier =  _resourceUtility.GetFiles(VedleggsMappe).ToArray();
            _hoveddokument = _resourceUtility.GetFiles(HoveddokumentMappe).ElementAt(0);
            
            Dokumentpakke = GenererDokumentpakke();
            HentSertifikater(out AvsenderSertifikat, out MottakerSertifikat);

            //Avsender og mottaker
            OrgNrAvsender = new Organisasjonsnummer("984661185");
            Behandlingsansvarlig = new Behandlingsansvarlig(OrgNrAvsender);
            Databehandler = new Databehandler(OrgNrAvsender, AvsenderSertifikat);

            OrgNrMottaker = new Organisasjonsnummer("984661185");
            DigitalPostMottaker = new DigitalPostMottaker("04036125433", "ove.jonsen#6K5A", MottakerSertifikat, OrgNrMottaker.Iso6523());
            
            //DigitalPost og forsendelse
            DigitalPostInfo = new DigitalPostInfo(DigitalPostMottaker, "Ikke-sensitiv tittel")
            {
                EpostVarsel = new EpostVarsel("tull@ball.no", "Dette er et epostvarsel. En trojansk ... hest.", 0, 7),
                SmsVarsel = new SmsVarsel("45215454", "Dette er et smsvarsel. En trojansk ... telefon..", 3, 14)
            };

            Forsendelse = new Forsendelse(Behandlingsansvarlig, DigitalPostInfo, Dokumentpakke);
            
            //Guids, AsicEArkiv og Envelope
            GuidHandler = new GuidHandler();
            Arkiv = new AsicEArkiv(Forsendelse, GuidHandler, Databehandler.Sertifikat);
            Envelope = new ForretningsmeldingEnvelope(new EnvelopeSettings(Forsendelse, Arkiv, Databehandler, GuidHandler, new Klientkonfigurasjon()));
        }

        private static void HentSertifikater(out X509Certificate2 avsenderSertifikat, out X509Certificate2 mottakerSertifkat)
        {
            var storeMy = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            storeMy.Open(OpenFlags.ReadOnly);

            try
            {
                avsenderSertifikat = storeMy.Certificates
                    .Find(X509FindType.FindByThumbprint, "8702F5E55217EC88CF2CCBADAC290BB4312594AC", true)[0];
            }
            catch (Exception e)
            {
                throw new InstanceNotFoundException("Kunne ikke finne avsendersertifikat for testing. Har du lagt det til slik guiden tilsier? (https://github.com/difi/sikker-digital-post-net-klient#legg-inn-avsendersertifikat-i-certificate-store) ", e);
            }
            storeMy.Close();


            var storeTrusted = new X509Store(StoreName.TrustedPeople, StoreLocation.CurrentUser);
            storeTrusted.Open(OpenFlags.ReadOnly);
            try
            {
                mottakerSertifkat =
                    storeTrusted.Certificates
                    .Find(X509FindType.FindByThumbprint, "B43CAAA0FBEE6C8DA85B47D1E5B7BCAB42AB9ADD", true)[0];
            }
            catch (Exception e)
            {
                throw new InstanceNotFoundException("Kunne ikke finne mottakersertifikat for testing. Har du lagt det til slik guiden tilsier? (https://github.com/difi/sikker-digital-post-net-klient#legg-inn-mottakersertifikat-i-certificate-store) ", e);
            }
            storeTrusted.Close();
            
        }

        private static Dokumentpakke GenererDokumentpakke()
        {
            var dokumentpakke = new Dokumentpakke(GenererHoveddokument());
            dokumentpakke.LeggTilVedlegg(GenererVedlegg());
            return dokumentpakke;
        }

        private static Dokument GenererHoveddokument()
        {
            var bytes = _resourceUtility.ReadAllBytes(false,_hoveddokument);
            var fileName = _resourceUtility.GetFileName(_hoveddokument);
            return Hoveddokument = new Dokument("Hoveddokument", bytes,"text/xml","NO", fileName);
        }

        private static IEnumerable<Dokument> GenererVedlegg()
        {
            int count = 0;
            return Vedlegg = new List<Dokument>(
                    Vedleggsstier.Select(
                        v => new Dokument("Vedlegg" + count++, _resourceUtility.ReadAllBytes(false,v), "text/" + Path.GetExtension(_hoveddokument), "NO", _resourceUtility.GetFileName(v))));
        }
    }
}
