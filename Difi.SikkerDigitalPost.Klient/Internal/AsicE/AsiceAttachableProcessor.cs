using System.IO;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Post;

namespace Difi.SikkerDigitalPost.Klient.Internal.AsicE
{
    internal class AsiceAttachableProcessor
    {
        public AsiceAttachableProcessor(Forsendelse message, IDokumentpakkeProsessor dokumentpakkeProsessor)
        {
            Message = message;
            DokumentpakkeProsessor = dokumentpakkeProsessor;
        }

        private IDokumentpakkeProsessor DokumentpakkeProsessor { get; }

        private Forsendelse Message { get; }

        public void Process(Stream stream)
        {
            DokumentpakkeProsessor.Prosesser(Message, stream);
        }
    }
}