using Difi.Felles.Utility;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter;
using Difi.SikkerDigitalPost.Klient.Utilities;
using Difi.SikkerDigitalPost.Klient.XmlValidering;

namespace Difi.SikkerDigitalPost.Klient
{
    public class Klientkonfigurasjon : GeneriskKlientkonfigurasjon
    {
        public Klientkonfigurasjon(Miljø miljø)
            : base(miljø)
        {
            MeldingsformidlerOrganisasjon = new Organisasjonsnummer("984661185");
            Logger = Logging.TraceLogger();
        }

        public Organisasjonsnummer MeldingsformidlerOrganisasjon { get; set; }
    }
}