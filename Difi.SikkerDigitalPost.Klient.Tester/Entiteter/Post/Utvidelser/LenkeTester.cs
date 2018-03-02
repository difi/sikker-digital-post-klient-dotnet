using System;
using System.Text;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Post.Utvidelser;
using Difi.SikkerDigitalPost.Klient.Domene.Exceptions;
using Difi.SikkerDigitalPost.Klient.Domene.Extensions;
using Xunit;

namespace Difi.SikkerDigitalPost.Klient.Tester.Entiteter.Post.Utvidelser
{
    public class LenkeTester
    {
        [Fact]
        public void GenerererRiktigXmlMedAlleEgenskaper()
        {
            var nå = DateTime.Now;
            var xml = new Lenke("lenke.xml", "https://www.avsender.no", "Bekreft vedtak", "Bekreft", "NO", nå).AsXml();

            Assert.Contains("<lenke xmlns=\"http://begrep.difi.no/sdp/utvidelser/lenke\">", xml.OuterXml);
            Assert.Contains("<url>https://www.avsender.no</url>", xml.OuterXml);
            Assert.Contains("<beskrivelse lang=\"no\">Bekreft vedtak</beskrivelse>", xml.OuterXml);
            Assert.Contains("<knappTekst lang=\"no\">Bekreft</knappTekst>", xml.OuterXml);
            Assert.Contains($"<frist>{nå.ToStringWithUtcOffset()}</frist>", xml.OuterXml);
        }

        [Fact]
        public void GenerererXmlSomBytes()
        {
            var xmlSomBytes = new Lenke("lenke.xml", "https://www.avsender.no").Bytes;

            var xmlSomString = Encoding.UTF8.GetString(xmlSomBytes);

            Assert.Contains("<lenke xmlns=\"http://begrep.difi.no/sdp/utvidelser/lenke\">", xmlSomString);
            Assert.Contains("<url>https://www.avsender.no</url>", xmlSomString);
        }

        [Fact]
        public void GenererRiktigXmlMedBareUrl()
        {
            var xml = new Lenke("lenke.xml", "https://www.avsender.no").AsXml();

            Assert.Contains("<lenke xmlns=\"http://begrep.difi.no/sdp/utvidelser/lenke\">", xml.OuterXml);
            Assert.Contains("<url>https://www.avsender.no</url>", xml.OuterXml);
        }

        [Fact]
        public void GirValideringsfeilVedUgyldigXml()
        {
            var ugyldigLenke = new Lenke("lenke.xml", "ugyldig-lenke-uten-protokoll.no");

            Assert.Throws<XmlValidationException>(() => ugyldigLenke.Bytes);
        }
    }
}