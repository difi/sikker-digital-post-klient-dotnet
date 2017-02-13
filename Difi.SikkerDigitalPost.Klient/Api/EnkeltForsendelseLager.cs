using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter;
using System.Collections.Concurrent;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Post;

namespace Difi.SikkerDigitalPost.Klient.Api
{
    public class InMemoryForsendelseLager : IForsendelseLager
    {

        private ConcurrentDictionary<string, Forsendelse> forsendelser = new ConcurrentDictionary<string, Forsendelse>();

        public void lagreForsendelse(Forsendelse forsendelse)
        {
            forsendelser.TryAdd(forsendelse.KonversasjonsId.ToString(), forsendelse);
        }

        public Forsendelse hentForsendelse(string konversasjonsId)
        {
            Forsendelse forsendelse;
            forsendelser.TryGetValue(konversasjonsId, out forsendelse);
            return forsendelse;
        }

        public void slettForsendelse(string konversasjonsId)
        {
            Forsendelse forsendelse;
            forsendelser.TryRemove(konversasjonsId, out forsendelse);
        }

    }

}
