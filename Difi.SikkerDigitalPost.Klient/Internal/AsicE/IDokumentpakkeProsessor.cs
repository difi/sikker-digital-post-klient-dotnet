using System.IO;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Post;

namespace Difi.SikkerDigitalPost.Klient.Internal.AsicE
{
    public interface IDokumentpakkeProsessor
    {
        void Prosesser(Forsendelse signatureJob, Stream bundleStream);
    }
}