namespace Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Interface
{
    internal interface IAsiceVedlegg
    {
        string Filnavn { get; }
        byte[] Bytes { get; }
        string MimeType { get; }
        string Id { get; }
    }
}
