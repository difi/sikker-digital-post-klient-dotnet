using System.IO;
using System.Xml;
using ApiClientShared;
using Difi.Felles.Utility;
using Difi.SikkerDigitalPost.Klient.Utilities;

namespace Difi.SikkerDigitalPost.Klient.XmlValidering
{
    internal class ManifestValidator : XmlValidator
    {
        private static readonly ResourceUtility ResourceUtility = new ResourceUtility("Difi.SikkerDigitalPost.Klient.XmlValidering.xsd");

        public ManifestValidator()
        {
            LeggTilXsdRessurs(NavneromUtility.DifiSdpSchema10, HentRessurs("sdp-manifest.xsd"));
            LeggTilXsdRessurs(NavneromUtility.DifiSdpSchema10, HentRessurs("sdp-felles.xsd"));
            LeggTilXsdRessurs(NavneromUtility.XmlDsig, HentRessurs("w3.xmldsig-core-schema.xsd"));
        }

        private XmlReader HentRessurs(string path)
        {
            var bytes = ResourceUtility.ReadAllBytes(true, path);
            return XmlReader.Create(new MemoryStream(bytes));
        }
    }
}
