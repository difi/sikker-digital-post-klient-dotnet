namespace SikkerDigitalPost.Net.Domene.Entiteter
{
    public interface IAsiceVedlegg
    {
        string Filnavn { get; }
        byte[] Bytes { get; }
        string MimeType { get; }
    }
}
