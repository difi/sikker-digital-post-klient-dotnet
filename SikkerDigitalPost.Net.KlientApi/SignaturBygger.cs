using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Xml;
using SikkerDigitalPost.Net.Domene.Entiteter;
using SikkerDigitalPost.Net.Domene.Entiteter.AsicE.Signatur;
using SikkerDigitalPost.Net.KlientApi.Xml;

namespace SikkerDigitalPost.Net.KlientApi
{
    
    public class SignaturBygger
    {
        private const string NsXmlns = "http://uri.etsi.org/2918/v1.2.1#";
        private readonly IEnumerable<Dokument> _vedlegg;
        
        private readonly Signatur _signatur;
        private XmlDocument _signaturXml;

        public SignaturBygger(Signatur signatur, Forsendelse forsendelse)
        {
            _signatur = signatur;
            _vedlegg = forsendelse.Dokumentpakke.Vedlegg;
        }

        public void Bygg()
        {
            //Opprett dokument
            _signaturXml = new XmlDocument {PreserveWhitespace = true};
            var xmlDeclaration = _signaturXml.CreateXmlDeclaration("1.0", "UTF-8", null);
            _signaturXml.AppendChild(_signaturXml.CreateElement("XAdESSignatures", NsXmlns));
            _signaturXml.InsertBefore(xmlDeclaration, _signaturXml.DocumentElement);

            //Opprett SignedXml
            var sertifikat = _signatur.Sertifikat;
            SignedXml signedXml = new SignedXmlWithAgnosticId(_signaturXml, sertifikat);
            signedXml.SignedInfo.CanonicalizationMethod = "http://www.w3.org/TR/2001/REC-xml-c14n-20010315";
            signedXml.Signature.Id = "Signature";
            
            //Legg til referanser
            foreach (var item in _vedlegg)
            {
                var referanse = new Sha256Reference(item.Bytes)
                {
                    Uri = item.Filnavn,
                    Id = item.Filnavn
                };
                signedXml.AddReference(referanse);
            }

            signedXml.ComputeSignature();
            XmlElement xmlDigitalSignature = signedXml.GetXml();

            _signaturXml.DocumentElement.AppendChild(_signaturXml.ImportNode(xmlDigitalSignature, true));
            _signaturXml.Save(@"Z:\Development\Digipost\SikkerDigitalPost.Net\Signatur.xml");

            _signatur.Bytes = Encoding.UTF8.GetBytes(_signaturXml.OuterXml);
        }
    }
}
