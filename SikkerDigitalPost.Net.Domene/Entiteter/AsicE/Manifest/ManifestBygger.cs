using System.ComponentModel.Design;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using SikkerDigitalPost.Net.Domene.Extensions;

namespace SikkerDigitalPost.Net.Domene.Entiteter.AsicE.Manifest
{
    public class ManifestBygger
    {
        private readonly Manifest _manifest;
        private XmlDocument doc;
        private static readonly string NS_xmlns = "http://begrep.difi.no/sdp/schema_v10";
        private static readonly string NS_xmlnsxsi = "http://www.w3.org/2001/XMLSchema-instance";
        private static readonly string NS_xsiSchemaLocation = "http://begrep.difi.no/sdp/schema_v10 ../xsd/sdp-manifest.xsd ";

        public ManifestBygger(Manifest manifest)
        {
            _manifest = manifest;
        }

        public byte[] Bygg()
        {
            doc = new XmlDocument {PreserveWhitespace = true};
            doc.AppendChild(doc.CreateElement("manifest", NS_xmlns));
            doc.DocumentElement.SetAttribute("xmlns:xsi", NS_xmlnsxsi);
            doc.DocumentElement.SetAttribute("schemaLocation", NS_xmlnsxsi, NS_xsiSchemaLocation);

            doc.DocumentElement.AppendChild(Mottaker());
            doc.DocumentElement.AppendChild(Avsender());
            doc.DocumentElement.AppendChild(Dokument(_manifest.Forsendelse.Dokumentpakke.Hoveddokument, "hoveddokument"));

            foreach (var vedlegg in _manifest.Forsendelse.Dokumentpakke.Vedlegg)
            {
                doc.DocumentElement.AppendChild(Dokument(vedlegg, "vedlegg"));
            }
            SkrivTilFilTest();
            
            return null;
        }

        private void SkrivTilFilTest()
        {
            doc.Save(@"Z:\Development\Digipost\XmlManifest.xml");
        }
        
        private XmlElement Mottaker()
        {
            var mottaker = doc.CreateElement("mottaker", NS_xmlns);

            XmlElement person = doc.CreateElement("person", NS_xmlns);
            {
                var personidentifikator = person.AppendChildElement("personidentifikator", NS_xmlns, doc);
                personidentifikator.InnerText = _manifest.Mottaker.Personidentifikator;

                var postkasseadresse = person.AppendChildElement("postkasseadresse", NS_xmlns, doc);
                postkasseadresse.InnerText = _manifest.Mottaker.Postkasseadresse;
            }
            
            mottaker.AppendChild(person);
            return mottaker;
        }

        private XmlElement Avsender()
        {
            var avsender = doc.CreateElement("avsender", NS_xmlns);
            {
                var organisasjon = avsender.AppendChildElement("organisasjon", NS_xmlns, doc);
                organisasjon.SetAttribute("authority", "iso6523-actorid-upis");
                organisasjon.InnerText = _manifest.Avsender.Organisasjonsnummer.Iso6523();

                var avsenderidentifikator = avsender.AppendChildElement("avsenderidentifikator", NS_xmlns, doc);
                avsenderidentifikator.InnerText = _manifest.Avsender.Avsenderidentifikator;

                var fakturaReferanse = avsender.AppendChildElement("fakturaReferanse", NS_xmlns, doc);
                fakturaReferanse.InnerText = _manifest.Avsender.Fakturareferanse;
            }
            
            return avsender;
        }

        private XmlElement Dokument(Dokument dokument, string elementnavn)
        {
            var dokumentXml = doc.CreateElement(elementnavn, NS_xmlns);
            dokumentXml.SetAttribute("href", dokument.Filnavn);
            dokumentXml.SetAttribute("mime", dokument.Innholdstype);
            {
                var tittel = dokumentXml.AppendChildElement("tittel", NS_xmlns, doc);
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
