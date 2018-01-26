using System;
using System.IO;
using System.Xml;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Interface;
using Difi.SikkerDigitalPost.Klient.Domene.XmlValidering;

namespace Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Post.Utvidelser
{
    public abstract class DataDokument : IAsiceAttachable
    {
        private readonly Lazy<byte[]> _bytes;

        protected DataDokument(string filnavn, string mimeType)
        {
            Filnavn = filnavn;
            MimeType = mimeType;
            _bytes = new Lazy<byte[]>(() =>
            {
                var xml = AsXml();
                SdpXmlValidator.Validate(xml, filnavn);
                var ms = new MemoryStream();
                xml.Save(ms);
                return ms.ToArray();
            });
        }

        public string Filnavn { get; }

        public byte[] Bytes => _bytes.Value;

        public string MimeType { get; }

        public string Id { get; set; }

        internal abstract XmlDocument AsXml();
    }
}