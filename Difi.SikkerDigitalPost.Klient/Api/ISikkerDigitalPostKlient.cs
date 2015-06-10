using System.Threading.Tasks;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer.Forretning;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer.Transport;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Post;

namespace Difi.SikkerDigitalPost.Klient.Api
{
    public interface ISikkerDigitalPostKlient
    {
        Task<Transportkvittering> Send(Forsendelse forsendelse, bool lagreDokumentpakke = false);

        Task<Kvittering> HentKvittering(Kvitteringsforespørsel kvitteringsforespørsel);
        
        Task<Kvittering> HentKvitteringOgBekreftForrige(Kvitteringsforespørsel kvitteringsforespørsel,
            Forretningskvittering forrigeKvittering);

        Task Bekreft(Forretningskvittering forrigeKvittering);

    }
}
