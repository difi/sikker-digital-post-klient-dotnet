using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Interface;
using Difi.SikkerDigitalPost.Klient.Domene.Exceptions;
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
                ValiderXml(filnavn, xml);
                var ms = new MemoryStream();
                xml.Save(ms);
                return ms.ToArray();
            });
        }

        public string Filnavn { get; }

        public byte[] Bytes => _bytes.Value;

        public string MimeType { get; }

        public string Id { get; set; }

        private static void ValiderXml(string filnavn, XmlDocument xml)
        {
            List<string> validationMessages;
            var valid = SdpXmlValidator.Instance.Validate(xml.OuterXml, out validationMessages);
            if (!valid)
            {
                throw new XmlValidationException($"{filnavn} er ikke gyldig ihht. XSD", validationMessages);
            }
        }

        internal abstract XmlDocument AsXml();
    }
}