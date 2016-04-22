using System;
using System.Diagnostics;
using System.IO;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter;
using Difi.SikkerDigitalPost.Klient.Utilities;
using Difi.SikkerDigitalPost.Klient.XmlValidering;

namespace Difi.SikkerDigitalPost.Klient
{
    public class Klientkonfigurasjon
    {
        public Klientkonfigurasjon(Miljø miljø)
        {
            MeldingsformidlerOrganisasjon = new Organisasjonsnummer("984661185");
            Miljø = miljø;
            ProxyHost = null;
            ProxyScheme = "https";
            TimeoutIMillisekunder = (int) TimeSpan.FromSeconds(30).TotalMilliseconds;
            LoggXmlTilFil = false;
            StandardLoggSti = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Digipost");
        }

        public Organisasjonsnummer MeldingsformidlerOrganisasjon { get; set; }

        public Miljø Miljø { get; set; }

        /// <summary>
        ///     Angir host som skal benyttes i forbindelse med bruk av proxy. Både ProxyHost og ProxyPort må spesifiseres for at en
        ///     proxy skal benyttes.
        public string ProxyHost { get; set; }

        /// <summary>
        ///     Angir schema ved bruk av proxy. Standardverdien er 'https'.
        /// </summary>
        public string ProxyScheme { get; set; }

        /// <summary>
        ///     Angir portnummeret som skal benyttes i forbindelse med bruk av proxy. Både ProxyHost og ProxyPort må spesifiseres
        ///     for at en proxy skal benyttes.
        /// </summary>
        public int ProxyPort { get; set; }

        /// <summary>
        ///     Angir timeout for komunikasjonen fra og til meldingsformindleren. Default tid er 30 sekunder.
        /// </summary>
        public int TimeoutIMillisekunder { get; set; }

        public bool BrukProxy => !string.IsNullOrWhiteSpace(ProxyHost) && ProxyPort > 0;

        public bool LoggXmlTilFil { get; set; }

        public string StandardLoggSti { get; set; }
    }
}