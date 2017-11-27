using System.Text;
using System.Xml;
using Difi.Felles.Utility.Resources.Xml;
using Digipost.Api.Client.Shared.Resources.Resource;

namespace Difi.SikkerDigitalPost.Klient.Resources.Xml
{
    internal class XmlResource
    {
        private static readonly ResourceUtility ResourceUtility = new ResourceUtility("Difi.SikkerDigitalPost.Klient.Resources.Xml.Data");

        private static XmlDocument GetResource(params string[] path)
        {
            var bytes = ResourceUtility.ReadAllBytes(true, path);
            return XmlUtility.ToXmlDocument(Encoding.UTF8.GetString(bytes));
        }

        internal class Request
        {
        }

        internal class Response
        {
            public static XmlDocument GetTransportError()
            {
                return GetResource("Response", "CreateTransportError.xml");
            }

            public static XmlDocument GetTransportOk()
            {
                return GetResource("Response", "CreateTransportOk.xml");
            }
        }
    }
}