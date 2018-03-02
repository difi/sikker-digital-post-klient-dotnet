using System;
using System.Xml;
using Difi.SikkerDigitalPost.Klient.Domene.Extensions;
using Difi.SikkerDigitalPost.Klient.Domene.XmlValidering;

namespace Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Post.Utvidelser
{
    public class Lenke : DataDokument
    {
        private const string LenkeMimeType = "application/vnd.difi.dpi.lenke+xml";

        public Lenke(string filnavn, string url, string beskrivelse = null, string knappetekst = null, string spraakkode = "NO", DateTime? frist = null)
            : base(filnavn, LenkeMimeType)
        {
            Url = url;
            Beskrivelse = beskrivelse;
            Knappetekst = knappetekst;
            Spraakkode = spraakkode;
            Frist = frist;
        }

        public string Url { get; }

        public string Beskrivelse { get; }

        public string Knappetekst { get; }

        public string Spraakkode { get; }

        public DateTime? Frist { get; }

        internal override XmlDocument AsXml()
        {
            var xml = new XmlDocument {PreserveWhitespace = true};
            var xmlDeclaration = xml.CreateXmlDeclaration("1.0", "UTF-8", null);
            xml.AppendChild(xml.CreateElement("lenke", NavneromUtility.Lenke));
            xml.InsertBefore(xmlDeclaration, xml.DocumentElement);

            var url = xml.DocumentElement.AppendChildElement("url", NavneromUtility.Lenke, xml);
            url.InnerText = Url;

            if (Beskrivelse != null)
            {
                var beskrivelse = xml.DocumentElement.AppendChildElement("beskrivelse", NavneromUtility.Lenke, xml);
                beskrivelse.SetAttribute("lang", Spraakkode.ToLower());
                beskrivelse.InnerText = Beskrivelse;
            }

            if (Knappetekst != null)
            {
                var knappTekst = xml.DocumentElement.AppendChildElement("knappTekst", NavneromUtility.Lenke, xml);
                knappTekst.SetAttribute("lang", Spraakkode.ToLower());
                knappTekst.InnerText = Knappetekst;
            }

            if (Frist.HasValue)
            {
                var frist = xml.DocumentElement.AppendChildElement("frist", NavneromUtility.Lenke, xml);
                frist.InnerText = Frist.Value.ToStringWithUtcOffset();
            }

            return xml;
        }
    }
}