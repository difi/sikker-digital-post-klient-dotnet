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
using System.Security.Cryptography.X509Certificates;
using ApiClientShared;
using Difi.SikkerDigitalPost.Klient.AsicE;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Aktører;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Post;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Varsel;
using Difi.SikkerDigitalPost.Klient.Envelope;
using Difi.SikkerDigitalPost.Klient.Envelope.Forretningsmelding;
using Difi.SikkerDigitalPost.Klient.Tester.Utilities;
using Difi.SikkerDigitalPost.Klient.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Difi.SikkerDigitalPost.Klient.Tester
{
    [TestClass]
    public class TestBase
    {
        static readonly ResourceUtility _resourceUtility = new ResourceUtility("Difi.SikkerDigitalPost.Klient.Tester.testdata");

        //private static string _hoveddokument;

        //protected static string[] Vedleggsstier;
        //protected static string VedleggsMappe = "vedlegg";
        //protected static string HoveddokumentMappe = "hoveddokument";

        protected static Dokument Hoveddokument = DomeneUtility.GetHoveddokumentEnkel();
        protected static IEnumerable<Dokument> Vedlegg = DomeneUtility.GetVedleggEnkel();
        protected static Dokumentpakke Dokumentpakke = DomeneUtility.GetDokumentpakkeEnkel();

        protected static X509Certificate2 AvsenderSertifikat = DomeneUtility.GetAvsenderSertifikat();
        protected static X509Certificate2 MottakerSertifikat = DomeneUtility.GetMottakerSertifikat();

        //protected static Organisasjonsnummer OrgNrAvsender;
        protected static Avsender Avsender = DomeneUtility.GetAvsender();
        protected static Databehandler Databehandler;

        protected static Organisasjonsnummer OrgNrMottaker;
        protected static DigitalPostMottaker DigitalPostMottaker;

        protected static DigitalPostInfo DigitalPostInfo;
        protected static Forsendelse Forsendelse;

        internal static AsicEArkiv Arkiv;
        internal static ForretningsmeldingEnvelope Envelope;
        internal static GuidUtility GuidHandler;


        public static void Initialiser()
        {

            //Dokumentpakke
            //Vedleggsstier =  _resourceUtility.GetFiles(VedleggsMappe).ToArray();
            //_hoveddokument = _resourceUtility.GetFiles(HoveddokumentMappe).ElementAt(0);
            
            //Dokumentpakke = GenererDokumentpakke();
            //HentSertifikater(out AvsenderSertifikat, out MottakerSertifikat);

            //Avsender og mottakers
            //OrgNrAvsender = new Organisasjonsnummer("984661185");
            //Avsender = new Avsender(OrgNrAvsender);
            Databehandler = new Databehandler(Avsender.Organisasjonsnummer, AvsenderSertifikat);

            OrgNrMottaker = new Organisasjonsnummer("984661185");
            DigitalPostMottaker = new DigitalPostMottaker("04036125433", "ove.jonsen#6K5A", MottakerSertifikat, OrgNrMottaker.Iso6523());
            
            //DigitalPost og forsendelse
            DigitalPostInfo = new DigitalPostInfo(DigitalPostMottaker, "Ikke-sensitiv tittel")
            {
                EpostVarsel = new EpostVarsel("tull@ball.no", "Dette er et epostvarsel. En trojansk ... hest.", 0, 7),
                SmsVarsel = new SmsVarsel("45215454", "Dette er et smsvarsel. En trojansk ... telefon..", 3, 14)
            };

            Forsendelse = new Forsendelse(Avsender, DigitalPostInfo, Dokumentpakke);
            
            //Guids, AsicEArkiv og Envelope
            GuidHandler = new GuidUtility();
            Arkiv = new AsicEArkiv(Forsendelse, GuidHandler, Databehandler.Sertifikat);
            Envelope = new ForretningsmeldingEnvelope(new EnvelopeSettings(Forsendelse, Arkiv, Databehandler, GuidHandler, new Klientkonfigurasjon()));
        }
    }
}
