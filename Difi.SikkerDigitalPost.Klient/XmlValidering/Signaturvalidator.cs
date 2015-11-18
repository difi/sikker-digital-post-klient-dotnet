using System.IO;
using System.Xml;
using ApiClientShared;
using Difi.Felles.Utility;
using Difi.SikkerDigitalPost.Klient.Utilities;

namespace Difi.SikkerDigitalPost.Klient.XmlValidering
{
    internal class Signaturvalidator : XmlValidator
    {
        private static readonly ResourceUtility ResourceUtility = new ResourceUtility("Difi.SikkerDigitalPost.Klient.XmlValidering.xsd");

        public Signaturvalidator()
        {
            LeggTilXsdRessurs(NavneromUtility.UriEtsi121, HentRessurs("w3.ts_102918v010201.xsd"));
            LeggTilXsdRessurs(NavneromUtility.UriEtsi132, HentRessurs("w3.XAdES.xsd"));
            LeggTilXsdRessurs(NavneromUtility.XmlDsig, HentRessurs("w3.xmldsig-core-schema.xsd"));
        }

        private XmlReader HentRessurs(string path)
        {
            var bytes = ResourceUtility.ReadAllBytes(true, path);
            return XmlReader.Create(new MemoryStream(bytes));
        }
    }
}
