using Difi.SikkerDigitalPost.Klient.Domene.Entiteter;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Post;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Difi.SikkerDigitalPost.Klient.Api
{
    public interface IForsendelseLager
    {

        Forsendelse hentForsendelse(string meldingsId);
        void lagreForsendelse(Forsendelse forsendelse);

    }

}
