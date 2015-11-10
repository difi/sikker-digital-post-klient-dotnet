using System;
using System.Diagnostics;
using System.Text;
using System.Xml;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Aktører;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Interface;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Post;
using Difi.SikkerDigitalPost.Klient.Domene.Enums;
using Difi.SikkerDigitalPost.Klient.Domene.Extensions;
using Difi.SikkerDigitalPost.Klient.Utilities;

namespace Difi.SikkerDigitalPost.Klient.AsicE
{
    internal class Manifest : IAsiceVedlegg
    {
        private XmlDocument _manifestXml;

        public Avsender Avsender { get; private set; }
        public Forsendelse Forsendelse { get; private set; }


        public Manifest(Forsendelse forsendelse)
        {
            Forsendelse = forsendelse;
            Avsender = forsendelse.Avsender;
        }

        public string Filnavn
        {
            get { return "manifest.xml"; }
        }

        public string MimeType
        {
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

            Logging.Log(TraceEventType.Verbose, Forsendelse.KonversasjonsId, "Generert manifest for dokumentpakke" + Environment.NewLine + _manifestXml.OuterXml);

            return _manifestXml;
        }

        private XmlElement MottakerNode()
        {
           var digitalMottaker = (DigitalPostMottaker) Forsendelse.PostInfo.Mottaker;

            var mottaker = _manifestXml.CreateElement("mottaker", NavneromUtility.DifiSdpSchema10);

            XmlElement person = _manifestXml.CreateElement("person", NavneromUtility.DifiSdpSchema10);
            {
                XmlElement personidentifikator = person.AppendChildElement("personidentifikator", NavneromUtility.DifiSdpSchema10, _manifestXml);
                personidentifikator.InnerText = digitalMottaker.Personidentifikator;

                XmlElement postkasseadresse = person.AppendChildElement("postkasseadresse", NavneromUtility.DifiSdpSchema10, _manifestXml);
                postkasseadresse.InnerText = digitalMottaker.Postkasseadresse;
            }

            mottaker.AppendChild(person);
            return mottaker;
        }

        private XmlElement AvsenderNode()
        {
            XmlElement avsender = _manifestXml.CreateElement("avsender", NavneromUtility.DifiSdpSchema10);
            {
                XmlElement organisasjon = avsender.AppendChildElement("organisasjon", NavneromUtility.DifiSdpSchema10, _manifestXml);
                organisasjon.SetAttribute("authority", "iso6523-actorid-upis");
                organisasjon.InnerText = Avsender.Organisasjonsnummer.Iso6523();

                var avsenderId = Avsender.Avsenderidentifikator;
                if (!String.IsNullOrWhiteSpace(avsenderId))
                {
                    XmlElement avsenderidentifikator = avsender.AppendChildElement("avsenderidentifikator", NavneromUtility.DifiSdpSchema10, _manifestXml);
                    avsenderidentifikator.InnerText = Avsender.Avsenderidentifikator;
                }

                XmlElement fakturaReferanse = avsender.AppendChildElement("fakturaReferanse", NavneromUtility.DifiSdpSchema10, _manifestXml);
                fakturaReferanse.InnerText = Avsender.Fakturareferanse;
            }

            return avsender;
        }

        private XmlElement DokumentNode(Dokument dokument, string elementnavn, string innholdstekst)
        {
            XmlElement dokumentXml = _manifestXml.CreateElement(elementnavn, NavneromUtility.DifiSdpSchema10);
            dokumentXml.SetAttribute("href", dokument.FilnavnRådata);
            dokumentXml.SetAttribute("mime", dokument.MimeType);
            {
                XmlElement tittel = dokumentXml.AppendChildElement("tittel", NavneromUtility.DifiSdpSchema10, _manifestXml);
                tittel.SetAttribute("lang", dokument.Språkkode ?? Forsendelse.Språkkode);
                tittel.InnerText = innholdstekst;
            }
            return dokumentXml;
        }

    }
}
