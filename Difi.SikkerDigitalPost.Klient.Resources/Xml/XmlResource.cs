using System.Reflection;
using System.Text;
using System.Xml;
using Digipost.Api.Client.Shared.Resources.Resource;
using Digipost.Api.Client.Shared.Resources.Xml;

namespace Difi.SikkerDigitalPost.Klient.Resources.Xml
{
    internal class XmlResource
    {
        private static readonly ResourceUtility ResourceUtility = new ResourceUtility(Assembly.GetExecutingAssembly(), "Xml.Data.Response");

        private static XmlDocument GetResource(params string[] path)
        {
            var bytes = ResourceUtility.ReadAllBytes(path);
            return XmlUtility.ToXmlDocument(Encoding.UTF8.GetString(bytes));
        }

        internal class Request
        {
        }

        internal class Response
        {
            public static XmlDocument GetTransportError()
            {
                return GetResource("CreateTransportError.xml");
            }

            public static XmlDocument GetTransportOk()
            {
                return GetResource("CreateTransportOk.xml");
            }
        }
    }
}