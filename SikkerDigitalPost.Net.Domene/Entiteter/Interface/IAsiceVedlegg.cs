namespace SikkerDigitalPost.Net.Domene.Entiteter.Interface
{
    public interface IAsiceVedlegg
    {
        string Filnavn { get; }
        byte[] Bytes { get; }
        string MimeType { get; }
    }
}
