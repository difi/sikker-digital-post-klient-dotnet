using System.Text;
using System.Xml;
using SikkerDigitalPost.Domene.Entiteter.Aktører;
using SikkerDigitalPost.Domene.Entiteter.Interface;
using SikkerDigitalPost.Domene.Entiteter.Post;

namespace SikkerDigitalPost.Domene.Entiteter.AsicE.Manifest
{
    internal class Manifest : IAsiceVedlegg
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
