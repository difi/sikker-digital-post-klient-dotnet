using System;
using System.Configuration;
using SikkerDigitalPost.Domene.Entiteter;

namespace SikkerDigitalPost.Klientbibliotek
{
    public class Klientkonfigurasjon
    {
        public Uri MeldingsformidlerRoot { get; set; }

        public string ProxyHost { get; set; }

        public int ProxyPort { get; set; }

        public string ProxyScheme { get; set; }

        public double SocketTimeoutInMillis { get; set; }

        public double ConnectTimeoutInMillis { get; set; }

        public double ConnectionRequestTimeoutInMillis { get; set; }

        public bool UseProxy
        {
            get
            {
                return !string.IsNullOrWhiteSpace(ProxyHost) && ProxyPort > 0;
            }
        }

        public Organisasjonsnummer MeldingsformidlerOrganisasjon { get; set; }

        /// <summary>
        /// Klientkonfigurasjon som brukes ved oppsett av <see cref="SikkerDigitalPostKlient"/>.  Brukes for å sette parametere
        /// som proxy, timeout og URI til meldingsformidler.
        /// </summary>
        public Klientkonfigurasjon()
        {
            MeldingsformidlerRoot = SetFromAppConfig<Uri>("SDP:MeldingsformidlerRoot", new Uri("https://meldingsformidler.digipost.no/api/ebms"));
            MeldingsformidlerOrganisasjon = SetFromAppConfig<Organisasjonsnummer>("SDP:MeldingsformidlerOrganisasjon", new Organisasjonsnummer("984661185")); // Posten Norge AS
            ProxyHost = SetFromAppConfig<string>("SDP:ProxyHost", null);
            ProxyScheme = SetFromAppConfig<string>("SDP:ProxyScheme", "https");

            SocketTimeoutInMillis = SetFromAppConfig<double>("SDP:SocketTimeoutInMillis", TimeSpan.FromSeconds(30).TotalMilliseconds);
            ConnectTimeoutInMillis = SetFromAppConfig<double>("SDP:ConnectTimeoutInMillis", TimeSpan.FromSeconds(10).TotalMilliseconds);
            ConnectionRequestTimeoutInMillis = SetFromAppConfig<double>("SDP:ConnectionRequestTimeoutInMillis", TimeSpan.FromSeconds(10).TotalMilliseconds);
        }

        private T SetFromAppConfig<T>(string key, T @default)
        {
            var appSettings = ConfigurationManager.AppSettings;

            string value = appSettings[key];
            if (value == null)
                return @default;

            if (typeof(IConvertible).IsAssignableFrom(typeof(T)))
            {
                return (T)Convert.ChangeType(value, typeof(T));
            }
            else
            {
                return (T)Activator.CreateInstance(typeof(T), new object[] { value });
            }
        }
    }
}
