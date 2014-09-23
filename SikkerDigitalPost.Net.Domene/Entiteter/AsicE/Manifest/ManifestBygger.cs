using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace SikkerDigitalPost.Net.Domene.Entiteter.AsicE.Manifest
{
    public class ManifestBygger
    {
        private readonly Manifest _manifest;
        private XmlDocument _manifestdokument;
        private static readonly XNamespace NS_xmlns = "http://begrep.difi.no/sdp/schema_v10";
        private static readonly XNamespace NS_xmlnsxsi = "http://www.w3.org/2001/XMLSchema-instance";
        private static readonly XNamespace NS_xsiSchemaLocation = "http://begrep.difi.no/sdp/schema_v10 ../xsd/sdp-manifest.xsd ";

        public ManifestBygger(Manifest manifest)
        {
            _manifest = manifest;
        }

        public byte[] Bygg()
        {
            _manifestdokument = new XmlDocument();
            _manifestdokument.PreserveWhitespace = true;

            var mgr = Namespace();

            var rotelement = RotElement();
            _manifestdokument.AppendChild(rotelement);

            var mottaker = Mottaker();
            rotelement.AppendChild(mottaker);
            
            SkrivTilFilTest();

            return null;
        }

        private void SkrivTilFilTest()
        {
            XmlWriter writer = new XmlTextWriter(new StreamWriter(@"Z:\Development\Digipost\XmlManifest.xml"));
            _manifestdokument.WriteTo(writer);
        }


        private XmlNamespaceManager Namespace()
        {
            var mgr = new XmlNamespaceManager(_manifestdokument.NameTable);
            mgr.AddNamespace("xmlns", NS_xmlns.NamespaceName);
            mgr.AddNamespace("xmlns:xsi", NS_xmlnsxsi.NamespaceName);
            mgr.AddNamespace("xsi:schemasocation", NS_xsiSchemaLocation.NamespaceName);
            return mgr;
        }

        private XmlElement RotElement()
        {
            var root = _manifestdokument.CreateElement("manifest", NS_xmlns.NamespaceName);
            root.SetAttribute("xmlns:xsi", NS_xmlns.NamespaceName);
            root.SetAttribute("xsi:schemalocation", NS_xmlns.NamespaceName);
            return root;
        }
        
        private XmlElement Mottaker()
        {
            var mottaker = _manifestdokument.CreateElement("mottaker");
            mottaker.AppendChild(Person());
            return mottaker;
        }

        private XmlElement Person()
        {
            var person = _manifestdokument.CreateElement("person");
            //person.AppendChild(Personidentifikator());
            return person;
        }

        private XmlNode Personidentifikator()
        {
            return null; // throw new System.NotImplementedException();
        }
    }
}
