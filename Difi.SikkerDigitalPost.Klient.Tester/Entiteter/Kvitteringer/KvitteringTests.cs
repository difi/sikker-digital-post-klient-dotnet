using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer;
using Xunit;

namespace Difi.SikkerDigitalPost.Klient.Tester.Entiteter.Kvitteringer
{
    public class KvitteringTests
    {
        public class RådataMethod
        {
            [Fact]
            public void SameRådataAsXml()
            {
                var kvittering = new KvitteringMock("kvitteringid");
                var kvitteringdata = Resources.Xml.XmlResource.Response.GetTransportOk();

                kvittering.Xml = kvitteringdata;

                Assert.Equal(kvitteringdata.OuterXml, kvittering.Rådata);
            }
        }

    }

    class KvitteringMock : Kvittering
    {
        public KvitteringMock(string meldingsId)
            : base(meldingsId)
        {
        }
    }
}
