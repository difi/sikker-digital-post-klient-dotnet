using System;
using System.Text;
using System.Xml;
using SikkerDigitalPost.Net.Domene.Entiteter.Aktører;
using SikkerDigitalPost.Net.Domene.Entiteter.Interface;

namespace SikkerDigitalPost.Net.Domene.Entiteter.AsicE.Manifest
{
    public class Manifest : IAsiceVedlegg
    {
        public Manifest(Mottaker mottaker, Behandlingsansvarlig avsender, Forsendelse forsendelse)
        {
            Avsender = avsender;
            Forsendelse = forsendelse;
            Mottaker = mottaker;
        }

        public Behandlingsansvarlig Avsender { get; private set; }

        public Forsendelse Forsendelse { get; set; }

        public Mottaker Mottaker { get; private set; }

        public byte[] Bytes { get; set; }

        public XmlDocument Xml()
        {
            var doc = new XmlDocument();
            var xml = Encoding.UTF8.GetString(Bytes);
            doc.LoadXml(xml);
            return doc;
        }

        public string Filnavn {
            get { return "manifest.xml"; }
        }

        public string Innholdstype {
            get { return "application/xml"; }
        }


    }
}
