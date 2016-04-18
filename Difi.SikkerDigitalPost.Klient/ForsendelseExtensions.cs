using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Difi.SikkerDigitalPost.Klient.AsicE;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Post;

namespace Difi.SikkerDigitalPost.Klient
{
    /*
     * Basert på:
     * http://begrep.difi.no/SikkerDigitalPost/1.2.0/forretningslag/meldingsstorrelse
     * http://begrep.difi.no/SikkerDigitalPost/1.2.0/forretningslag/Dokumentpakke/langtidslagring
     */
    public static class ForsendelseExtensions
    {
        public static long BeregnForsendelseStorrelse(this Forsendelse forsendelse, X509Certificate2 avsenderSertifikat)
        {
            var manifest = new Manifest(forsendelse);
            var signatur = new Signatur(forsendelse, manifest, avsenderSertifikat);

            var retur = 0L;

            retur += manifest.Bytes.Length;
            retur += signatur.Bytes.Length;
            retur += forsendelse.Dokumentpakke.Hoveddokument.Bytes.Length;

            retur += forsendelse.Dokumentpakke.Vedlegg.Sum(v => v.Bytes.Length);

            return retur;
        }
    }
}