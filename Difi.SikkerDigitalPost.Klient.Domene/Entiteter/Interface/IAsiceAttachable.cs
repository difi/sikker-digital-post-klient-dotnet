namespace Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Interface
{
    internal interface IAsiceAttachable
    {
        string Filnavn { get; }

        byte[] Bytes { get; }

        string MimeType { get; }

        string Id { get; }
    }
}