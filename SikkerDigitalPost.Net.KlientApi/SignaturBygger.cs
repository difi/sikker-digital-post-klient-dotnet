using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Xml;
using SikkerDigitalPost.Net.Domene.Entiteter;
using SikkerDigitalPost.Net.Domene.Entiteter.AsicE.Signatur;
using SikkerDigitalPost.Net.Domene.Entiteter.Interface;
using SikkerDigitalPost.Net.KlientApi.Xml;

namespace SikkerDigitalPost.Net.KlientApi
{

    public class SignaturBygger
    {
        private const string NsXmlns = "http://uri.etsi.org/2918/v1.2.1#";

        private readonly Signatur _signatur;
        private readonly Forsendelse _forsendelse;
        private XmlDocument _signaturXml;

        public SignaturBygger(Signatur signatur, Forsendelse forsendelse)
        {
            _signatur = signatur;
            _forsendelse = forsendelse;
        }

        public void Bygg()
        {
            _signaturXml = OpprettXmlDocument();

            var signedXml = OpprettSignedXml(_signaturXml, _signatur);

            var referanser = Referanser(_forsendelse.Dokumentpakke.Hoveddokument, _forsendelse.Dokumentpakke.Vedlegg);
            OpprettReferanser(signedXml, referanser);
            
            signedXml.AddObject(new QualifyingPropertiesObject(_signatur.Sertifikat, "#Signature", 
                referanser
                    .Select(r => new QualifyingPropertiesReference { Filename = r.Filnavn, Mimetype = r.Innholdstype })
                    .ToArray(), _signaturXml.DocumentElement)
                );

            var signedPropertiesReference = new Sha256Reference("#SignedProperties");
            signedPropertiesReference.Type = "http://uri.etsi.org/01903#SignedProperties";
            signedPropertiesReference.AddTransform(new XmlDsigC14NTransform(false));
            signedXml.AddReference(signedPropertiesReference);

            KeyInfoX509Data keyInfoX509Data = new KeyInfoX509Data(_signatur.Sertifikat, X509IncludeOption.WholeChain);
            signedXml.KeyInfo.AddClause(keyInfoX509Data);

            signedXml.ComputeSignature();

            _signaturXml.DocumentElement.AppendChild(_signaturXml.ImportNode(signedXml.GetXml(), true));

            _signaturXml.Save(@"Z:\Development\Digipost\SikkerDigitalPost.Net\Signatur.xml");
            _signatur.Bytes = Encoding.UTF8.GetBytes(_signaturXml.OuterXml);
        }

        private void OpprettReferanser(SignedXml signedXml, IEnumerable<Dokument> referanser)
        {
            foreach (var item in referanser)
            {
                signedXml.AddReference(HentReferanse(item));
            }
        }

        private static IEnumerable<Dokument> Referanser(Dokument hoveddokument, IEnumerable<Dokument> vedlegg)
        {
            var referanser = new List<Dokument>();
            referanser.Add(hoveddokument);
            referanser.AddRange(vedlegg);
            return referanser;
        }

        private SignedXml OpprettSignedXml(XmlDocument signaturXml, Signatur signatur)
        {
            SignedXml signedXml = new SignedXmlWithAgnosticId(signaturXml, signatur.Sertifikat);
            signedXml.SignedInfo.CanonicalizationMethod = "http://www.w3.org/TR/2001/REC-xml-c14n-20010315";
            signedXml.Signature.Id = "Signature";
            return signedXml;
        }

        private XmlDocument OpprettXmlDocument()
        {
            var signaturXml = new XmlDocument { PreserveWhitespace = true };
            var xmlDeclaration = signaturXml.CreateXmlDeclaration("1.0", "UTF-8", null);
            signaturXml.AppendChild(signaturXml.CreateElement("XAdESSignatures", NsXmlns));
            signaturXml.InsertBefore(xmlDeclaration, signaturXml.DocumentElement);
            return signaturXml;
        }

        private Sha256Reference HentReferanse(IAsiceVedlegg dokument)
        {
            return new Sha256Reference(dokument.Bytes)
            {
                Uri = dokument.Filnavn,
                Id = dokument.Filnavn
            };
        }
    }
}
