using System;
using System.Text;
using System.Xml;
using SikkerDigitalPost.Domene.Entiteter.Aktører;
using SikkerDigitalPost.Domene.Entiteter.Interface;
using SikkerDigitalPost.Domene.Entiteter.Post;
using SikkerDigitalPost.Domene.Extensions;

namespace SikkerDigitalPost.Klient.AsicE
{
    internal class Manifest : IAsiceVedlegg
    {
        private XmlDocument _manifestXml;

        public Behandlingsansvarlig Avsender { get; private set; }
        public Forsendelse Forsendelse { get; private set; }

        
        public Manifest(Forsendelse forsendelse)
        {
            Forsendelse = forsendelse;
            Avsender = forsendelse.Behandlingsansvarlig;
        }
        
        public string Filnavn {
            get { return "manifest.xml"; }
        }
        
        public string Innholdstype {
            get { return "application/xml"; }
        }

        public string Id
        {
            get
            {
                return "Id_1";
            }
        }

        public byte[] Bytes
        {
            get
            {
                return Encoding.UTF8.GetBytes(Xml().OuterXml);
            }
        }

        public XmlDocument Xml()
        {
            if (_manifestXml != null)
                return _manifestXml;

            _manifestXml = new XmlDocument { PreserveWhitespace = true };
            var xmlDeclaration = _manifestXml.CreateXmlDeclaration("1.0", "UTF-8", null);
            _manifestXml.AppendChild(_manifestXml.CreateElement("manifest", Navnerom.Ns9));
            _manifestXml.InsertBefore(xmlDeclaration, _manifestXml.DocumentElement);

            _manifestXml.DocumentElement.AppendChild(MottakerNode());
            _manifestXml.DocumentElement.AppendChild(AvsenderNode());

            var hoveddokument = Forsendelse.Dokumentpakke.Hoveddokument;
            _manifestXml.DocumentElement.AppendChild(DokumentNode(hoveddokument, "hoveddokument", hoveddokument.Tittel));

            foreach (var vedlegg in Forsendelse.Dokumentpakke.Vedlegg)
            {
                _manifestXml.DocumentElement.AppendChild(DokumentNode(vedlegg, "vedlegg", vedlegg.Filnavn));
            }
            
            return _manifestXml;
        }

        private XmlElement MottakerNode()
        {
            var mottaker = _manifestXml.CreateElement("mottaker", Navnerom.Ns9);

            XmlElement person = _manifestXml.CreateElement("person", Navnerom.Ns9);
            {
                XmlElement personidentifikator = person.AppendChildElement("personidentifikator", Navnerom.Ns9, _manifestXml);
                personidentifikator.InnerText = Forsendelse.DigitalPost.Mottaker.Personidentifikator;

                XmlElement postkasseadresse = person.AppendChildElement("postkasseadresse", Navnerom.Ns9, _manifestXml);
                postkasseadresse.InnerText = Forsendelse.DigitalPost.Mottaker.Postkasseadresse;
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
                organisasjon.InnerText = Avsender.Organisasjonsnummer.Iso6523();

                var avsenderId = Avsender.Avsenderidentifikator;
                if (!String.IsNullOrWhiteSpace(avsenderId))
                {
                    XmlElement avsenderidentifikator = avsender.AppendChildElement("avsenderidentifikator", Navnerom.Ns9, _manifestXml);
                    avsenderidentifikator.InnerText = Avsender.Avsenderidentifikator;
                }

                XmlElement fakturaReferanse = avsender.AppendChildElement("fakturaReferanse", Navnerom.Ns9, _manifestXml);
                fakturaReferanse.InnerText = Avsender.Fakturareferanse;
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
                tittel.SetAttribute("lang", dokument.Språkkode ?? Forsendelse.Språkkode);
                tittel.InnerText = innholdstekst;
            }
            return dokumentXml;
        }

    }
}
