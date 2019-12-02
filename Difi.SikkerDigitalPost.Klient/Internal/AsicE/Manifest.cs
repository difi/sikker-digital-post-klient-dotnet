using System.Text;
using System.Xml;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Aktører;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Interface;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Post;
using Difi.SikkerDigitalPost.Klient.Domene.Enums;
using Difi.SikkerDigitalPost.Klient.Domene.Extensions;
using Difi.SikkerDigitalPost.Klient.Utilities;

namespace Difi.SikkerDigitalPost.Klient.Internal.AsicE
{
    internal class Manifest : IAsiceAttachable
    {
        private XmlDocument _manifestXml;

        public Manifest(Forsendelse forsendelse)
        {
            Forsendelse = forsendelse;
            Avsender = forsendelse.Avsender;
        }

        public Avsender Avsender { get; }

        public Forsendelse Forsendelse { get; }

        public string Filnavn => "manifest.xml";

        public string MimeType => "application/xml";

        public string Id => "Id_1";

        public byte[] Bytes => Encoding.UTF8.GetBytes(Xml().OuterXml);

        public XmlDocument Xml()
        {
            if (_manifestXml != null)
                return _manifestXml;

            _manifestXml = new XmlDocument {PreserveWhitespace = true};
            var xmlDeclaration = _manifestXml.CreateXmlDeclaration("1.0", "UTF-8", null);
            _manifestXml.AppendChild(_manifestXml.CreateElement("manifest", NavneromUtility.DifiSdpSchema10));
            _manifestXml.InsertBefore(xmlDeclaration, _manifestXml.DocumentElement);

            if (Forsendelse.Sendes(Postmetode.Digital))
            {
                _manifestXml.DocumentElement.AppendChild(MottakerNode());
            }

            _manifestXml.DocumentElement.AppendChild(AvsenderNode());

            var hoveddokument = Forsendelse.Dokumentpakke.Hoveddokument;
            _manifestXml.DocumentElement.AppendChild(DokumentNode(hoveddokument, "hoveddokument", hoveddokument.Tittel));

            foreach (var vedlegg in Forsendelse.Dokumentpakke.Vedlegg)
            {
                _manifestXml.DocumentElement.AppendChild(DokumentNode(vedlegg, "vedlegg", vedlegg.Tittel));
            }

            return _manifestXml;
        }

        private XmlElement MottakerNode()
        {
            var digitalMottaker = (DigitalPostMottaker) Forsendelse.PostInfo.Mottaker;

            var mottaker = _manifestXml.CreateElement("mottaker", NavneromUtility.DifiSdpSchema10);

            var person = _manifestXml.CreateElement("person", NavneromUtility.DifiSdpSchema10);
            {
                var personidentifikator = person.AppendChildElement("personidentifikator", NavneromUtility.DifiSdpSchema10, _manifestXml);
                personidentifikator.InnerText = digitalMottaker.Personidentifikator;

                var postkasseadresse = person.AppendChildElement("postkasseadresse", NavneromUtility.DifiSdpSchema10, _manifestXml);
                postkasseadresse.InnerText = digitalMottaker.Postkasseadresse;
            }

            mottaker.AppendChild(person);
            return mottaker;
        }

        private XmlElement AvsenderNode()
        {
            var avsender = _manifestXml.CreateElement("avsender", NavneromUtility.DifiSdpSchema10);
            {
                var organisasjon = avsender.AppendChildElement("organisasjon", NavneromUtility.DifiSdpSchema10, _manifestXml);
                organisasjon.SetAttribute("authority", "iso6523-actorid-upis");
                organisasjon.InnerText = Avsender.Organisasjonsnummer.WithCountryCode;

                var avsenderId = Avsender.Avsenderidentifikator;
                if (!string.IsNullOrWhiteSpace(avsenderId))
                {
                    var avsenderidentifikator = avsender.AppendChildElement("avsenderidentifikator", NavneromUtility.DifiSdpSchema10, _manifestXml);
                    avsenderidentifikator.InnerText = Avsender.Avsenderidentifikator;
                }

                var fakturaReferanse = avsender.AppendChildElement("fakturaReferanse", NavneromUtility.DifiSdpSchema10, _manifestXml);
                fakturaReferanse.InnerText = Avsender.Fakturareferanse;
            }

            return avsender;
        }

        private XmlElement DokumentNode(Dokument dokument, string elementnavn, string innholdstekst)
        {
            var dokumentXml = _manifestXml.CreateElement(elementnavn, NavneromUtility.DifiSdpSchema10);
            dokumentXml.SetAttribute("href", dokument.FilnavnRådata);
            dokumentXml.SetAttribute("mime", dokument.MimeType);
            {
                var tittel = dokumentXml.AppendChildElement("tittel", NavneromUtility.DifiSdpSchema10, _manifestXml);
                tittel.SetAttribute("lang", dokument.Språkkode ?? Forsendelse.Språkkode);
                tittel.InnerText = innholdstekst;
            }
            
            if (Forsendelse.MetadataDocument != null)
            {
                var data = dokumentXml.AppendChildElement("data", NavneromUtility.DifiSdpSchema10, _manifestXml);
                data.SetAttribute("href", Forsendelse.MetadataDocument.Filnavn);
                data.SetAttribute("mime", Forsendelse.MetadataDocument.MimeType);
            }
            
            return dokumentXml;
        }
    }
}