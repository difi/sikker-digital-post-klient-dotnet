namespace Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Interface
{
    public interface IAsiceAttachable
    {
        string Filnavn { get; }

        byte[] Bytes { get; }

        string MimeType { get; }

        string Id { get; }
    }
}