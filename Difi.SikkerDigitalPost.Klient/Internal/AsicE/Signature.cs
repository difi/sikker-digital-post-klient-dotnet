using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Xml;
using Difi.Felles.Utility.Security;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Interface;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Post;
using Difi.SikkerDigitalPost.Klient.Domene.Exceptions;
using Difi.SikkerDigitalPost.Klient.Security;
using Difi.SikkerDigitalPost.Klient.Utilities;
using Sha256Reference = Difi.SikkerDigitalPost.Klient.Domene.Sha256Reference;

namespace Difi.SikkerDigitalPost.Klient.Internal.AsicE
{
    internal class Signature : IAsiceAttachable
    {
        private readonly Forsendelse _forsendelse;
        private readonly Manifest _manifest;
        private readonly MetadataDocument _metadataDocument;
        private readonly X509Certificate2 _sertifikat;
        private XmlDocument _xml;

        public Signature(Forsendelse forsendelse, Manifest manifest, X509Certificate2 sertifikat, MetadataDocument metadataDocument = null)
        {
            _forsendelse = forsendelse;
            _manifest = manifest;
            _sertifikat = sertifikat;
            _metadataDocument = metadataDocument;
        }

        public string Filnavn => "META-INF/signatures.xml";

        public byte[] Bytes => Encoding.UTF8.GetBytes(Xml().OuterXml);

        public string MimeType => "application/xml";

        public string Id => "Id_0";

        public XmlDocument Xml()
        {
            if (_xml != null)
            {
                return _xml;
            }

            try
            {
                _xml = OpprettXmlDokument();

                var signaturnode = Signaturnode();

                var referanser = Referanser(_forsendelse.Dokumentpakke.Hoveddokument,
                    _forsendelse.Dokumentpakke.Vedlegg, _manifest, _metadataDocument);
                OpprettReferanser(signaturnode, referanser);

                var keyInfoX509Data = new KeyInfoX509Data(_sertifikat, X509IncludeOption.EndCertOnly);
                
                signaturnode.KeyInfo.AddClause(keyInfoX509Data);
                signaturnode.ComputeSignature();

                _xml.DocumentElement.AppendChild(_xml.ImportNode(signaturnode.GetXml(), true));
            }
            catch (Exception e)
            {
                throw new XmlParseException("Kunne ikke bygge Xml for signatur.", e);
            }

            return _xml;
        }

        private static Sha256Reference SignedPropertiesReferanse()
        {
            var signedPropertiesReference = new Sha256Reference("#SignedProperties")
            {
                Type = "http://uri.etsi.org/01903#SignedProperties"
            };
            signedPropertiesReference.AddTransform(new XmlDsigC14NTransform(false));
            return signedPropertiesReference;
        }

        private void OpprettReferanser(SignedXml signaturnode, IEnumerable<IAsiceAttachable> referanser)
        {
            foreach (var item in referanser)
            {
                signaturnode.AddReference(Sha256Referanse(item));
            }

            signaturnode.AddObject(
                new QualifyingPropertiesObject(
                    _sertifikat, "#Signature", referanser.ToArray(), _xml.DocumentElement)
                );

            signaturnode.AddReference(SignedPropertiesReferanse());
        }

        private static IEnumerable<IAsiceAttachable> Referanser(Dokument hoveddokument, IEnumerable<IAsiceAttachable> vedlegg, Manifest manifest, MetadataDocument metadataDocument = null)
        {
            var referanser = new List<IAsiceAttachable> {hoveddokument};
            referanser.AddRange(vedlegg);
            referanser.Add(manifest);

            if (metadataDocument != null)
            {
                referanser.Add(metadataDocument);
            }
            
            return referanser;
        }

        private SignedXml Signaturnode()
        {
            var signedXml = new SignedXmlWithAgnosticId(_xml, _sertifikat);
            signedXml.SignedInfo.CanonicalizationMethod = "http://www.w3.org/TR/2001/REC-xml-c14n-20010315";
            signedXml.Signature.Id = "Signature";
            return signedXml;
        }

        private XmlDocument OpprettXmlDokument()
        {
            var signaturXml = new XmlDocument {PreserveWhitespace = true};
            var xmlDeclaration = signaturXml.CreateXmlDeclaration("1.0", "UTF-8", null);
            signaturXml.AppendChild(signaturXml.CreateElement("xades", "XAdESSignatures", NavneromUtility.UriEtsi121));
            signaturXml.DocumentElement.SetAttribute("xmlns:ns11", NavneromUtility.UriEtsi132);

            signaturXml.InsertBefore(xmlDeclaration, signaturXml.DocumentElement);
            return signaturXml;
        }

        private Sha256Reference Sha256Referanse(IAsiceAttachable dokument)
        {
            return new Sha256Reference(dokument.Bytes)
            {
                Uri = dokument.Filnavn,
                Id = dokument.Id
            };
        }
    }
}