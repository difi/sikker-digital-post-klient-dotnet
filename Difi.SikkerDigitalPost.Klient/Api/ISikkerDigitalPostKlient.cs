using System.Threading.Tasks;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer.Forretning;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer.Transport;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Post;

namespace Difi.SikkerDigitalPost.Klient.Api
{
    public interface ISikkerDigitalPostKlient
    {
        Task<Transportkvittering> SendAsync(Forsendelse forsendelse, bool lagreDokumentpakke = false);

        Task<Kvittering> HentKvitteringAsync(Kvitteringsforespørsel kvitteringsforespørsel);
        
        Task<Kvittering> HentKvitteringOgBekreftForrigeAsync(Kvitteringsforespørsel kvitteringsforespørsel,
            Forretningskvittering forrigeKvittering);

        Task BekreftAsync(Forretningskvittering forrigeKvittering);

        Transportkvittering Send(Forsendelse forsendelse, bool lagreDokumentpakke = false);

        Kvittering HentKvittering(Kvitteringsforespørsel kvitteringsforespørsel);

        Kvittering HentKvitteringOgBekreftForrige(Kvitteringsforespørsel kvitteringsforespørsel,
            Forretningskvittering forrigeKvittering);

        void Bekreft(Forretningskvittering forrigeKvittering);

    }
}
