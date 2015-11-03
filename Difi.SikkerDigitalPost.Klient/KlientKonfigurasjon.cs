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
using System.Diagnostics;
using System.IO;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter;
using Difi.SikkerDigitalPost.Klient.Utilities;
using Difi.SikkerDigitalPost.Klient.XmlValidering;

namespace Difi.SikkerDigitalPost.Klient
{
    /// <summary>
    /// Inneholder konfigurasjon for sending av digital post.
    /// </summary> 
    public class Klientkonfigurasjon
    {
        public Miljø Miljø { get; set; }

        /// <summary>
        /// Angir host som skal benyttes i forbindelse med bruk av proxy. Både ProxyHost og ProxyPort må spesifiseres for at en proxy skal benyttes. Denne verdien kan også overstyres i 
        /// applikasjonens konfigurasjonsfil gjennom med appSettings verdi med nøkkelen 'SDP:ProxyHost'.
        /// </summary>
        public string ProxyHost { get; set; }

        /// <summary>
        /// Angir schema ved bruk av proxy. Standardverdien er 'https'. Denne verdien kan også overstyres i 
        /// applikasjonens konfigurasjonsfil gjennom med appSettings verdi med nøkkelen 'SDP:ProxyScheme'.
        /// </summary>
        public string ProxyScheme { get; set; }

        /// <summary>
        /// Angir portnummeret som skal benyttes i forbindelse med bruk av proxy. Både ProxyHost og ProxyPort må spesifiseres for at en proxy skal benyttes. Denne verdien kan også overstyres i 
        /// applikasjonens konfigurasjonsfil gjennom med appSettings verdi med nøkkelen 'SDP:ProxyPort'.
        /// </summary>
        public int ProxyPort { get; set; }

        /// <summary>
        /// Angir timeout for komunikasjonen fra og til meldingsformindleren. Default tid er 30 sekunder. Denne verdien kan også overstyres i 
        /// applikasjonens konfigurasjonsfil gjennom med appSettings verdi med nøkkelen 'SDP:TimeoutIMillisekunder'.
        /// </summary>
        public int TimeoutIMillisekunder { get; set; }

        /// <summary>
        /// Eksponerer et grensesnitt for logging hvor brukere kan integrere sin egen loggefunksjonalitet eller en tredjepartsløsning som f.eks log4net. For bruk, angi en annonym funksjon med 
        /// følgende parametre: severity, konversasjonsid, metode, melding. Som default benyttes trace logging med navn 'SikkerDigitalPost.Klient' som kan aktiveres i applikasjonens konfigurasjonsfil. 
        /// </summary>
        public Action<TraceEventType, Guid?, string, string> Logger { get; set; }

        /// <summary>
        /// Indikerer om proxy skal benyttes for oppkoblingen mot meldingsformidleren.
        /// </summary>
        public bool BrukProxy
        {
            get
            {
                return !string.IsNullOrWhiteSpace(ProxyHost) && ProxyPort > 0;
            }
        }

        /// <summary>
        /// Angir organisasjonsnummeret til meldingsformidleren. Standardverdi er '984661185' som er organisasjonsnummeret til Posten Norge AS. Denne verdien kan også overstyres i 
        /// applikasjonens konfigurasjonsfil gjennom med appSettings verdi med nøkkelen 'SDP:MeldingsformidlerOrganisasjon'.
        /// </summary>
        internal Organisasjonsnummer MeldingsformidlerOrganisasjon { get; set; }

        /// <summary>
        /// Setter om logging skal gjøres til fil for alle meldinger som går mellom Klientbibliotek og Meldingsformidler
        /// </summary>
        public bool LoggXmlTilFil { get; set; }

        public string StandardLoggSti { get; set; }

        /// <summary>
        /// Klientkonfigurasjon som brukes ved oppsett av <see cref="SikkerDigitalPostKlient"/>.  Brukes for å sette parametere
        /// som proxy, timeout og URI til meldingsformidler.
        /// </summary>
        public Klientkonfigurasjon(Miljø miljø)
        {
            Miljø = miljø;
            MeldingsformidlerOrganisasjon = new Organisasjonsnummer("984661185");
            ProxyHost = null;
            ProxyScheme = "https";
            TimeoutIMillisekunder = (int) TimeSpan.FromSeconds(30).TotalMilliseconds;            
            Logger = Logging.TraceLogger();
            LoggXmlTilFil = false;
            StandardLoggSti = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Digipost");
        }
    }
}
