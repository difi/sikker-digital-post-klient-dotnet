using System.Text;

namespace Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Interface
{
    public class MetadataDocument : IWithDocumentProperties
    {
        public string Filnavn { get; }
        public byte[] Bytes { get; }
        public string MimeType { get; }
        public string Id { get; }
        public string Tittel => null;

        public MetadataDocument(string filnavn, string mimeType, string xml)
        {
            Filnavn = filnavn;
            MimeType = mimeType;
            Bytes = Encoding.UTF8.GetBytes(xml);
            Id = "Id_3";
        }
    }
}
