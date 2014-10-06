using System.Text;
using System.Xml;
using SikkerDigitalPost.Domene.Entiteter.AsicE.Manifest;
using SikkerDigitalPost.Domene.Entiteter.Post;
using SikkerDigitalPost.Domene.Extensions;
using SikkerDigitalPost.Klient.Envelope;

namespace SikkerDigitalPost.Klient
{
    internal class ManifestBygger
    {
        private const string XsiSchemaLocation = "http://begrep.difi.no/sdp/schema_v10 ../xsd/sdp-manifest.xsd ";

        private readonly Manifest _manifest;
        private XmlDocument _manifestXml;

        internal ManifestBygger(Manifest manifest)
        {
            _manifest = manifest;
        }

        public void Bygg()
        {
            // Opprett rotnode med navnerom.
            _manifestXml = new XmlDocument {PreserveWhitespace = true};
            var xmlDeclaration = _manifestXml.CreateXmlDeclaration("1.0", "UTF-8", null);
            _manifestXml.AppendChild(_manifestXml.CreateElement("manifest", Navnerom.Ns9));
            _manifestXml.DocumentElement.SetAttribute("xmlns:xsi", Navnerom.xsi);
            _manifestXml.DocumentElement.SetAttribute("schemaLocation", Navnerom.xsi, XsiSchemaLocation);
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
            var mottaker = _manifestXml.CreateElement("mottaker", Navnerom.Ns9);

            XmlElement person = _manifestXml.CreateElement("person", Navnerom.Ns9);
            {
                XmlElement personidentifikator = person.AppendChildElement("personidentifikator", Navnerom.Ns9, _manifestXml);
                personidentifikator.InnerText = _manifest.Mottaker.Personidentifikator;

                XmlElement postkasseadresse = person.AppendChildElement("postkasseadresse", Navnerom.Ns9, _manifestXml);
                postkasseadresse.InnerText = _manifest.Mottaker.Postkasseadresse;
            }
            
            mottaker.AppendChild(person);
            return mottaker;
        }

        private XmlElement AvsenderNode()
        {
            XmlElement avsender = _manifestXml.CreateElement("avsender", Navnerom.Ns9);
            {
                XmlElement organisasjon = avsender.AppendChildElement("organisasjon", Navnerom.Ns9, _manifestXml);
                organisasjon.SetAttribute("authority", "iso6523-actorid-upis");
                organisasjon.InnerText = _manifest.Avsender.Organisasjonsnummer.Iso6523();

                XmlElement avsenderidentifikator = avsender.AppendChildElement("avsenderidentifikator", Navnerom.Ns9, _manifestXml);
                avsenderidentifikator.InnerText = _manifest.Avsender.Avsenderidentifikator;

                XmlElement fakturaReferanse = avsender.AppendChildElement("fakturaReferanse", Navnerom.Ns9, _manifestXml);
                fakturaReferanse.InnerText = _manifest.Avsender.Fakturareferanse;
            }
            
            return avsender;
        }

        private XmlElement DokumentNode(Dokument dokument, string elementnavn, string innholdstekst)
        {
            XmlElement dokumentXml = _manifestXml.CreateElement(elementnavn, Navnerom.Ns9);
            dokumentXml.SetAttribute("href", dokument.Filnavn);
            dokumentXml.SetAttribute("mime", dokument.Innholdstype);
            {
                XmlElement tittel = dokumentXml.AppendChildElement("tittel", Navnerom.Ns9, _manifestXml);
                tittel.SetAttribute("lang", dokument.Språkkode ?? _manifest.Forsendelse.Språkkode);
                tittel.InnerText = innholdstekst;
            }
            return dokumentXml;
        }
    }
}