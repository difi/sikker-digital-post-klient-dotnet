using System.Threading.Tasks;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer.Forretning;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer.Transport;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Post;

namespace Difi.SikkerDigitalPost.Klient.Api
{
    public interface ISikkerDigitalPostKlient
    {
        Transportkvittering Send(Forsendelse forsendelse, bool lagreDokumentpakke = false);

        Task<Transportkvittering> SendAsync(Forsendelse forsendelse, bool lagreDokumentpakke = false);
        
        Kvittering HentKvittering(Kvitteringsforespørsel kvitteringsforespørsel);

        Task<Kvittering> HentKvitteringAsync(Kvitteringsforespørsel kvitteringsforespørsel);

        void Bekreft(Forretningskvittering forrigeKvittering);
        
        Task BekreftAsync(Forretningskvittering forrigeKvittering);

        Kvittering HentKvitteringOgBekreftForrige(Kvitteringsforespørsel kvitteringsforespørsel,
            Forretningskvittering forrigeKvittering);

        Task<Kvittering> HentKvitteringOgBekreftForrigeAsync(Kvitteringsforespørsel kvitteringsforespørsel,
            Forretningskvittering forrigeKvittering);
    }
}
