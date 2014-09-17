using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SikkerDigitalPost.Net.Klient
{
    public class KlientKonfigurasjon
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

        public KlientKonfigurasjon()
        {
            this.MeldingsformidlerRoot = SetFromAppConfig<Uri>("SDP:MeldingsformidlerRoot", new Uri("https://meldingsformidler.digipost.no/api/ebms"));
            this.MeldingsformidlerOrganisasjon = SetFromAppConfig<Organisasjonsnummer>("SDP:MeldingsformidlerOrganisasjon", new Organisasjonsnummer("984661185")); // Posten Norge AS
            this.ProxyHost = SetFromAppConfig<string>("SDP:ProxyHost", null);
            this.ProxyScheme = SetFromAppConfig<string>("SDP:ProxyScheme", "https");

            this.SocketTimeoutInMillis = SetFromAppConfig<double>("SDP:SocketTimeoutInMillis", TimeSpan.FromSeconds(30).TotalMilliseconds);
            this.ConnectTimeoutInMillis = SetFromAppConfig<double>("SDP:ConnectTimeoutInMillis", TimeSpan.FromSeconds(10).TotalMilliseconds);
            this.ConnectionRequestTimeoutInMillis = SetFromAppConfig<double>("SDP:ConnectionRequestTimeoutInMillis", TimeSpan.FromSeconds(10).TotalMilliseconds);
        }

        private T SetFromAppConfig<T>(string key, T @default)
        {
            var appSettings = ConfigurationManager.AppSettings;

            string value = appSettings[key];
            if (value == null)
                return @default;

            if (typeof(T) is IConvertible)
                return (T)Convert.ChangeType(value, typeof(T));
            else
            {
                return (T)Activator.CreateInstance(typeof(T), new object[] { value });
            }
        }
    }
}
