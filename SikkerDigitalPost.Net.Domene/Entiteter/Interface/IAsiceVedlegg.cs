namespace SikkerDigitalPost.Net.Domene.Entiteter.Interface
{
    public interface IAsiceVedlegg
    {
        string Filsti { get; }
        byte[] Bytes { get; }
        string Innholdstype { get; }
    }
}
