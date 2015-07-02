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

using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using ApiClientShared;
using Difi.SikkerDigitalPost.Klient.AsicE;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Aktører;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Post;
using Difi.SikkerDigitalPost.Klient.Envelope.Forretningsmelding;
using Difi.SikkerDigitalPost.Klient.Tester.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Difi.SikkerDigitalPost.Klient.Tester
{
    [TestClass]
    public class TestBase
    {
        protected static Dokument Hoveddokument = DomeneUtility.GetHoveddokumentEnkel();
        protected static IEnumerable<Dokument> Vedlegg = DomeneUtility.GetVedleggEnkel();
        protected static Dokumentpakke Dokumentpakke = DomeneUtility.GetDokumentpakkeEnkel();

        protected static X509Certificate2 AvsenderSertifikat = DomeneUtility.GetAvsenderSertifikat();
        protected static X509Certificate2 MottakerSertifikat = DomeneUtility.GetMottakerSertifikat();

        protected static Avsender Avsender = DomeneUtility.GetAvsender();
        protected static Databehandler Databehandler = DomeneUtility.GetDatabehandler();

        protected static DigitalPostMottaker DigitalPostMottaker = DomeneUtility.GetDigitalPostMottaker();

        protected static DigitalPostInfo DigitalPostInfo = DomeneUtility.GetDigitalPostInfoMedVarsel();
        protected static Forsendelse Forsendelse = DomeneUtility.GetForsendelseEnkel();

        internal static AsicEArkiv Arkiv = DomeneUtility.GetAsicEArkivEnkel();
        internal static ForretningsmeldingEnvelope Envelope = DomeneUtility.GetForretningsmeldingEnvelope();
    }
}
