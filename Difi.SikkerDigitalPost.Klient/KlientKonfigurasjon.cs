using System;
using System.Collections.Generic;
using System.IO;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Post;
using Difi.SikkerDigitalPost.Klient.XmlValidering;

namespace Difi.SikkerDigitalPost.Klient
{
    public class Klientkonfigurasjon
    {
        public Klientkonfigurasjon(Miljø miljø)
        {
            Miljø = miljø;
        }

        [Obsolete]
        public Organisasjonsnummer MeldingsformidlerOrganisasjon { get; set; } = new Organisasjonsnummer("984661185");

        public Miljø Miljø { get; set; }

        /// <summary>
        ///     Angir host som skal benyttes i forbindelse med bruk av proxy. Både ProxyHost og ProxyPort må spesifiseres for at en
        ///     proxy skal benyttes.
        public string ProxyHost { get; set; } = null;

        /// <summary>
        ///     Angir schema ved bruk av proxy. Standardverdien er 'https'.
        /// </summary>
        public string ProxyScheme { get; set; } = "https";

        /// <summary>
        ///     Angir portnummeret som skal benyttes i forbindelse med bruk av proxy. Både ProxyHost og ProxyPort må spesifiseres
        ///     for at en proxy skal benyttes.
        /// </summary>
        public int ProxyPort { get; set; }

        /// <summary>
        ///     Angir timeout for komunikasjonen fra og til meldingsformindleren. Default tid er 30 sekunder.
        /// </summary>
        public int TimeoutIMillisekunder { get; set; } = (int) TimeSpan.FromSeconds(30).TotalMilliseconds;

        public bool BrukProxy => !string.IsNullOrWhiteSpace(ProxyHost) && ProxyPort > 0;

        /// <summary>
        ///     Hvis satt til true, så vil alle forespørsler og responser logges med nivå DEBUG.
        /// </summary>
        public bool LoggForespørselOgRespons { get; set; } = false;
    }
}
