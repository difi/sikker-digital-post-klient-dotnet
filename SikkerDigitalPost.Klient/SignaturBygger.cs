using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Xml;
using SikkerDigitalPost.Domene;
using SikkerDigitalPost.Domene.Entiteter.Interface;
using SikkerDigitalPost.Domene.Entiteter.Post;
using SikkerDigitalPost.Klient.AsicE;
using SikkerDigitalPost.Klient.Envelope;
using SikkerDigitalPost.Klient.Xml;
using Sha256Reference = SikkerDigitalPost.Klient.Xml.Sha256Reference;

namespace SikkerDigitalPost.Klient
{
    internal class SignaturBygger
    {
        private const string XsiSchemaLocation = "http://begrep.difi.no/sdp/schema_v10 ../xsd/ts_102918v010201.xsd";

        private readonly Signatur _signatur;
        private readonly Forsendelse _forsendelse;
        private readonly Manifest _manifest;

        private XmlDocument _signaturDokumentXml;

        internal SignaturBygger(Signatur signatur, Forsendelse forsendelse, Manifest manifest)
        {
            _signatur = signatur;
            _forsendelse = forsendelse;
            _manifest = manifest;
        }

       
    }
}
