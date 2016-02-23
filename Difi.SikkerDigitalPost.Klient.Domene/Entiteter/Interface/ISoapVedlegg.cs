namespace Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Interface
{
    internal interface ISoapVedlegg
    {
        string Filnavn { get; }

        byte[] Bytes { get; }

        string Innholdstype { get; }

        string ContentId { get; }

        string TransferEncoding { get; }
    }
}