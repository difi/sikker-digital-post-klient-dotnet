using System.IO;
using System.Xml;
using ApiClientShared;
using Difi.Felles.Utility;
using Difi.SikkerDigitalPost.Klient.Utilities;

namespace Difi.SikkerDigitalPost.Klient.Internal.AsicE
{
    internal class ManifestValidator : XmlValidator
    {
        private static readonly ResourceUtility ResourceUtility = new ResourceUtility("Difi.SikkerDigitalPost.Klient.XmlValidering.xsd");

        public ManifestValidator()
        {
            LeggTilXsdRessurs(NavneromUtility.DifiSdpSchema10, GetResource("sdp-manifest.xsd"));
            LeggTilXsdRessurs(NavneromUtility.DifiSdpSchema10, GetResource("sdp-felles.xsd"));
            LeggTilXsdRessurs(NavneromUtility.XmlDsig, GetResource("w3.xmldsig-core-schema.xsd"));
        }

        private static XmlReader GetResource(string path)
        {
            var bytes = ResourceUtility.ReadAllBytes(true, path);
            return XmlReader.Create(new MemoryStream(bytes));
        }
    }
}