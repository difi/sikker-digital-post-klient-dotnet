using System.IO;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Post;

namespace Difi.SikkerDigitalPost.Klient.Internal.AsicE
{
    internal class AsiceAttachableProcessor
    {
        private IDokumentpakkeProsessor DokumentpakkeProsessor { get; }
        private Forsendelse Message { get; }

        public AsiceAttachableProcessor(Forsendelse message, IDokumentpakkeProsessor dokumentpakkeProsessor)
        {
            Message = message;
            DokumentpakkeProsessor = dokumentpakkeProsessor;
        }

        public void Process(Stream stream)
        {
            DokumentpakkeProsessor.Prosesser(Message, stream);
        }
    }
}
