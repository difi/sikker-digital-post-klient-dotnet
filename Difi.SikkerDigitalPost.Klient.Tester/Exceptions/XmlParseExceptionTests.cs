using Difi.SikkerDigitalPost.Klient.Domene.Exceptions;
using Xunit;

namespace Difi.SikkerDigitalPost.Klient.Tester.Exceptions
{
    public class XmlParseExceptionTests
    {
        public class Rådata
        {
            [Fact]
            public void SameRådataAsXml()
            {
                var errorXml = Resources.Xml.XmlResource.Response.GetTransportError();
                var exception = new XmlParseException {Xml = errorXml};

                Assert.Equal(errorXml.OuterXml, exception.Rådata);
            }
        }

    }
}
