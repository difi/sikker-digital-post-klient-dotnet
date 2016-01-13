using Difi.SikkerDigitalPost.Klient.Domene.Entiteter;
using Difi.SikkerDigitalPost.Klient.Utilities;
using Difi.SikkerDigitalPost.Klient.XmlValidering;

namespace Difi.SikkerDigitalPost.Klient
{
    public class Klientkonfigurasjon : GeneriskKlientkonfigurasjon
    {
        public Organisasjonsnummer MeldingsformidlerOrganisasjon { get; set; }

        public Klientkonfigurasjon(Miljø miljø) : base(miljø)
        {
            MeldingsformidlerOrganisasjon = new Organisasjonsnummer("984661185");
            Logger = Logging.TraceLogger();

        }
    }
}
