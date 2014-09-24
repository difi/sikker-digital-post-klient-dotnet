using System.ComponentModel.Design;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using SikkerDigitalPost.Net.Domene.Extensions;

namespace SikkerDigitalPost.Net.Domene.Entiteter.AsicE.Manifest
{
    public class ManifestBygger
    {
        private const string NsXmlns = "http://begrep.difi.no/sdp/schema_v10";
        private const string NsXmlnsxsi = "http://www.w3.org/2001/XMLSchema-instance";
        private const string NsXsiSchemaLocation = "http://begrep.difi.no/sdp/schema_v10 ../xsd/sdp-manifest.xsd ";

        private readonly Manifest _manifest;
        private XmlDocument _manifestXml;

        public ManifestBygger(Manifest manifest)
        {
            _manifest = manifest;
        }

        public byte[] Bygg()
        {
            // Opprett rotnode med navnerom.
            _manifestXml = new XmlDocument {PreserveWhitespace = true};
            _manifestXml.AppendChild(_manifestXml.CreateElement("manifest", NsXmlns));
            _manifestXml.DocumentElement.SetAttribute("xmlns:xsi", NsXmlnsxsi);
            _manifestXml.DocumentElement.SetAttribute("schemaLocation", NsXmlnsxsi, NsXsiSchemaLocation);

            _manifestXml.DocumentElement.AppendChild(Mottaker());
            _manifestXml.DocumentElement.AppendChild(Avsender());
            _manifestXml.DocumentElement.AppendChild(Dokument(_manifest.Forsendelse.Dokumentpakke.Hoveddokument, "hoveddokument"));

            foreach (var vedlegg in _manifest.Forsendelse.Dokumentpakke.Vedlegg)
            {
                _manifestXml.DocumentElement.AppendChild(Dokument(vedlegg, "vedlegg"));
            }
            
            return Encoding.Default.GetBytes(_manifestXml.OuterXml);
        }
        
        private XmlElement Mottaker()
        {
            var mottaker = _manifestXml.CreateElement("mottaker", NsXmlns);

            XmlElement person = _manifestXml.CreateElement("person", NsXmlns);
            {
                XmlElement personidentifikator = person.AppendChildElement("personidentifikator", NsXmlns, _manifestXml);
                personidentifikator.InnerText = _manifest.Mottaker.Personidentifikator;

                XmlElement postkasseadresse = person.AppendChildElement("postkasseadresse", NsXmlns, _manifestXml);
                postkasseadresse.InnerText = _manifest.Mottaker.Postkasseadresse;
            }
            
            mottaker.AppendChild(person);
            return mottaker;
        }

        private XmlElement Avsender()
        {
            XmlElement avsender = _manifestXml.CreateElement("avsender", NsXmlns);
            {
                XmlElement organisasjon = avsender.AppendChildElement("organisasjon", NsXmlns, _manifestXml);
                organisasjon.SetAttribute("authority", "iso6523-actorid-upis");
                organisasjon.InnerText = _manifest.Avsender.Organisasjonsnummer.Iso6523();

                XmlElement avsenderidentifikator = avsender.AppendChildElement("avsenderidentifikator", NsXmlns, _manifestXml);
                avsenderidentifikator.InnerText = _manifest.Avsender.Avsenderidentifikator;

                XmlElement fakturaReferanse = avsender.AppendChildElement("fakturaReferanse", NsXmlns, _manifestXml);
                fakturaReferanse.InnerText = _manifest.Avsender.Fakturareferanse;
            }
            
            return avsender;
        }

        private XmlElement Dokument(Dokument dokument, string elementnavn)
        {
            XmlElement dokumentXml = _manifestXml.CreateElement(elementnavn, NsXmlns);
            dokumentXml.SetAttribute("href", dokument.Filnavn);
            dokumentXml.SetAttribute("mime", dokument.Innholdstype);
            {
                XmlElement tittel = dokumentXml.AppendChildElement("tittel", NsXmlns, _manifestXml);
                tittel.SetAttribute("lang", HentSpråkkode(dokument));
                tittel.InnerText = dokument.Tittel;
            }
            return dokumentXml;
        }

        private string HentSpråkkode(Dokument dokument)
        {
            return dokument.HarStandardSpråk
                ? _manifest.Forsendelse.Språkkode
                : dokument.Språkkode;
        }
    }
}