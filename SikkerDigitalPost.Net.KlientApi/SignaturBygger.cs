using System.Security.Cryptography.Xml;
using System.Text;
using System.Xml;
using SikkerDigitalPost.Net.Domene.Entiteter.AsicE.Signatur;
using SikkerDigitalPost.Net.KlientApi.Xml;

namespace SikkerDigitalPost.Net.KlientApi
{
    
    public class SignaturBygger
    {
        private const string NsXmlns = "http://uri.etsi.org/2918/v1.2.1#";
        
        private readonly Signatur _signatur;
        private XmlDocument _signaturXml;

        public SignaturBygger(Signatur signatur)
        {
            _signatur = signatur;
        }

        public void Bygg()
        {
            _signaturXml = new XmlDocument {PreserveWhitespace = true};
            var xmlDeclaration = _signaturXml.CreateXmlDeclaration("1.0", "UTF-8", null);
            _signaturXml.AppendChild(_signaturXml.CreateElement("XAdESSignatures", NsXmlns));
            _signaturXml.InsertBefore(xmlDeclaration, _signaturXml.DocumentElement);

            SignedXml signedXml = new SignedXml(_signaturXml);
            var sertifikat = _signatur.Sertifikat;
            signedXml.SigningKey = sertifikat.PrivateKey;
            var reference = new Sha256Reference();
            reference.Uri = "";
            signedXml.AddReference(reference);
            signedXml.ComputeSignature();
            XmlElement xmlDigitalSignature = signedXml.GetXml();

            _signaturXml.DocumentElement.AppendChild(_signaturXml.ImportNode(xmlDigitalSignature, true));
            _signaturXml.Save(@"Z:\Development\Digipost\SikkerDigitalPost.Net\Signatur.xml");

            _signatur.Bytes = Encoding.UTF8.GetBytes(_signaturXml.OuterXml);
        }
    }
}
