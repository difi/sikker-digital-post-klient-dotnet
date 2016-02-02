using System.Text;
using System.Xml;
using ApiClientShared;
using Difi.Felles.Utility.Utilities;

namespace Difi.SikkerDigitalPost.Klient.Tester.Utilities
{
    public static class KvitteringsUtility
    {

        public static class Forretningskvittering
        {
            static readonly ResourceUtility ResourceUtility = new ResourceUtility("Difi.SikkerDigitalPost.Klient.Tester.Skjema.Eksempler.Kvitteringer.Forretning");

            public static XmlDocument FeilmeldingXml()
            {
                return TilXmlDokument("Feilmelding.xml");
            }

            public static XmlDocument LeveringskvitteringXml()
            {
                return TilXmlDokument("Leveringskvittering.xml");
            }

            public static XmlDocument MottakskvitteringXml()
            {
                return TilXmlDokument("Mottakskvittering.xml");
            }

            public static XmlDocument ReturpostkvitteringXml()
            {
                return TilXmlDokument("Returpostkvittering.xml");
            }

            public static XmlDocument VarslingFeiletKvitteringXml()
            {
                return TilXmlDokument("VarslingFeiletKvittering.xml");
            }

            public static XmlDocument ÅpningskvitteringXml()
            {
                return TilXmlDokument("Åpningskvittering.xml");
            }

            public static XmlDocument TilXmlDokument(string kvittering)
            {
                return XmlUtility.TilXmlDokument(Encoding.UTF8.GetString(ResourceUtility.ReadAllBytes(true, kvittering)));
            }
        }

        public static class Transportkvittering
        {
            static readonly ResourceUtility ResourceUtility = new ResourceUtility("Difi.SikkerDigitalPost.Klient.Tester.Skjema.Eksempler.Kvitteringer.Transport");

            public static XmlDocument TomKøKvitteringXml()
            {
                return TilXmlDokument("TomKøKvittering.xml");
            }

            public static XmlDocument TransportFeiletKvitteringXml()
            {
                return TilXmlDokument("TransportFeiletKvittering.xml");
            }

            public static XmlDocument TransportOkKvitteringXml()
            {
                return TilXmlDokument("TransportOkKvittering.xml");
            }

            public static XmlDocument TilXmlDokument(string kvittering)
            {
                return XmlUtility.TilXmlDokument(Encoding.UTF8.GetString(ResourceUtility.ReadAllBytes(true, kvittering)));
            }
        }
    }
}
