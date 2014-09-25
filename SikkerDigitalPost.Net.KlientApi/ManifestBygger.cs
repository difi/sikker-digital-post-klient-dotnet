using System.Text;
using System.Xml;
using SikkerDigitalPost.Net.Domene.Entiteter;
using SikkerDigitalPost.Net.Domene.Entiteter.AsicE.Manifest;
using SikkerDigitalPost.Net.Domene.Extensions;

namespace SikkerDigitalPost.Net.KlientApi
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

        public void Bygg()
        {
            // Opprett rotnode med navnerom.
            _manifestXml = new XmlDocument {PreserveWhitespace = true};
            var xmlDeclaration = _manifestXml.CreateXmlDeclaration("1.0", "UTF-8", null);
            _manifestXml.AppendChild(_manifestXml.CreateElement("manifest", NsXmlns));
            _manifestXml.DocumentElement.SetAttribute("xmlns:xsi", NsXmlnsxsi);
            _manifestXml.DocumentElement.SetAttribute("schemaLocation", NsXmlnsxsi, NsXsiSchemaLocation);
            _manifestXml.InsertBefore(xmlDeclaration, _manifestXml.DocumentElement);
            

            _manifestXml.DocumentElement.AppendChild(MottakerNode());
            _manifestXml.DocumentElement.AppendChild(AvsenderNode());

            var hoveddokument = _manifest.Forsendelse.Dokumentpakke.Hoveddokument;
            _manifestXml.DocumentElement.AppendChild(DokumentNode(hoveddokument, "hoveddokument", hoveddokument.Tittel));

            foreach (var vedlegg in _manifest.Forsendelse.Dokumentpakke.Vedlegg)
            {
                _manifestXml.DocumentElement.AppendChild(DokumentNode(vedlegg, "vedlegg", vedlegg.Filnavn));
            }
            _manifest.Bytes = Encoding.UTF8.GetBytes(_manifestXml.OuterXml);
        }

        public void SkrivXmlTilFil(string filsti)
        {
            _manifestXml.Save(filsti);
        }
        
        private XmlElement MottakerNode()
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

        private XmlElement AvsenderNode()
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

        private XmlElement DokumentNode(Dokument dokument, string elementnavn, string innholdstekst)
        {
            XmlElement dokumentXml = _manifestXml.CreateElement(elementnavn, NsXmlns);
            dokumentXml.SetAttribute("href", dokument.Filnavn);
            dokumentXml.SetAttribute("mime", dokument.Innholdstype);
            {
                XmlElement tittel = dokumentXml.AppendChildElement("tittel", NsXmlns, _manifestXml);
                tittel.SetAttribute("lang", dokument.Språkkode ?? _manifest.Forsendelse.Språkkode);
                tittel.InnerText = innholdstekst;
            }
            return dokumentXml;
        }
    }
}