using System.Reflection;
using System.Text;
using System.Xml;
using Difi.SikkerDigitalPost.Klient.Utilities;

namespace Difi.SikkerDigitalPost.Klient.Tester.Utilities
{
    public static class KvitteringsUtility
    {
        public static class Forretningskvittering
        {
            private static readonly ResourceUtility ResourceUtility = new ResourceUtility(Assembly.GetExecutingAssembly(), "Skjema.Eksempler.Kvitteringer.Forretning");

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
                return XmlUtility.TilXmlDokument(Encoding.UTF8.GetString(ResourceUtility.ReadAllBytes(kvittering)));
            }
        }
    }
}
