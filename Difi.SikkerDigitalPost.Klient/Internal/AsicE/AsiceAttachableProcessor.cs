using System.IO;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Post;

namespace Difi.SikkerDigitalPost.Klient.Internal.AsicE
{
    internal class AsiceAttachableProcessor
    {
        public AsiceAttachableProcessor(Forsendelse forsendelse, IDokumentpakkeProsessor dokumentpakkeProsessor)
        {
            Forsendelse = forsendelse;
            DokumentpakkeProsessor = dokumentpakkeProsessor;
        }

        private IDokumentpakkeProsessor DokumentpakkeProsessor { get; }

        private Forsendelse Forsendelse { get; }

        public void Process(Stream stream)
        {
            DokumentpakkeProsessor.Prosesser(Forsendelse, stream);
        }
    }
}