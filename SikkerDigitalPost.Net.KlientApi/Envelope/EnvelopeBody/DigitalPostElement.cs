using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Xml;
using SikkerDigitalPost.Net.Domene.Entiteter;
using SikkerDigitalPost.Net.KlientApi.Xml;

namespace SikkerDigitalPost.Net.KlientApi.Envelope.EnvelopeBody
{
    public class DigitalPostElement : XmlPart
    {

        private readonly SHA256Managed _managedSha256;

        public DigitalPostElement(XmlDocument dokument, Forsendelse forsendelse, AsicEArkiv asicEArkiv, Databehandler databehandler) : base(dokument, forsendelse, asicEArkiv, databehandler)
        {
            _managedSha256 = new SHA256Managed();
        }

        public override XmlElement Xml()
        {
            XmlElement digitalPostElement = XmlDocument.CreateElement("ns9", "digitalPost", Navnerom.Ns9);
            {
                digitalPostElement.AppendChild(AvsenderElement());
                digitalPostElement.AppendChild(MottakerElement());
                digitalPostElement.AppendChild(DigitalPostInfoElement());
                digitalPostElement.AppendChild(DokumentfingerpakkeavtrykkElement());
                digitalPostElement.PrependChild(XmlDocument.ImportNode(Signature().GetXml(), true));
            }
            return digitalPostElement;
        }

        private SignedXml Signature()
        {
            SignedXml signedXml = new SignedXmlWithAgnosticId(XmlDocument, Databehandler.Sertifikat);
            
            var reference = new Sha256Reference("");
            reference.AddTransform(new XmlDsigEnvelopedSignatureTransform());
            reference.AddTransform(new XmlDsigExcC14NTransform("difi"));
            signedXml.AddReference(reference);

            //signedXml.SignedInfo.CanonicalizationMethod = "http://www.w3.org/TR/2001/REC-xml-c14n-20010315";
            //signedXml.Signature.Id = "Signature";

            var keyInfoX509Data = new KeyInfoX509Data(Databehandler.Sertifikat, X509IncludeOption.WholeChain);
            signedXml.KeyInfo.AddClause(keyInfoX509Data);

            signedXml.ComputeSignature();
          
            return signedXml;
        }

        private XmlElement AvsenderElement()
        {
            XmlElement avsender = XmlDocument.CreateElement("ns9", "avsender", Navnerom.Ns9);
            {
                XmlElement organisasjon = XmlDocument.CreateElement("ns9", "organisasjon", Navnerom.Ns9);
                organisasjon.SetAttribute("authority", "iso6523-actorid-upis");
                organisasjon.InnerText = Forsendelse.Behandlingsansvarlig.Organisasjonsnummer.Iso6523();
                avsender.AppendChild(organisasjon);
            }
            return avsender;
        }

        private XmlElement MottakerElement()
        {
            XmlElement mottaker = XmlDocument.CreateElement("ns9", "mottaker", Navnerom.Ns9);
            {
                XmlElement person = XmlDocument.CreateElement("ns9", "person", Navnerom.Ns9);
                {
                    XmlElement personidentifikator = XmlDocument.CreateElement("ns9", "personidentifikator", Navnerom.Ns9);
                    personidentifikator.InnerText = Forsendelse.DigitalPost.Mottaker.Personidentifikator;
                    person.AppendChild(personidentifikator);

                    XmlElement postkasseadresse = XmlDocument.CreateElement("ns9", "postkasseadresse", Navnerom.Ns9);
                    postkasseadresse.InnerText = Forsendelse.DigitalPost.Mottaker.Postkasseadresse;
                    person.AppendChild(postkasseadresse);
                }
                mottaker.AppendChild(person);
            }
            return mottaker;
        }

        private XmlElement DigitalPostInfoElement()
        {
            XmlElement digitalPostInfo = XmlDocument.CreateElement("ns9", "digitalPostInfo", Navnerom.Ns9);
            {
                XmlElement aapningskvittering = XmlDocument.CreateElement("ns9", "aapningskvittering", Navnerom.Ns9);
                aapningskvittering.InnerText = Forsendelse.DigitalPost.Åpningskvittering.ToString();
                digitalPostInfo.AppendChild(aapningskvittering);

                XmlElement sikkerhetsnivaa = XmlDocument.CreateElement("ns9", "sikkerhetsnivaa", Navnerom.Ns9);
                sikkerhetsnivaa.InnerText = Forsendelse.DigitalPost.Sikkerhetsnivå.ToString();
                digitalPostInfo.AppendChild(sikkerhetsnivaa);

                XmlElement ikkeSensitivTittel = XmlDocument.CreateElement("ns9", "ikkeSensitivTittel", Navnerom.Ns9);
                ikkeSensitivTittel.InnerText = Forsendelse.DigitalPost.IkkeSensitivTittel;
                digitalPostInfo.AppendChild(ikkeSensitivTittel);

                XmlElement varsler = XmlDocument.CreateElement("ns9", "varsler", Navnerom.Ns9);
                {
                    if (Forsendelse.DigitalPost.EpostVarsel != null)
                    {
                        XmlElement epostVarsel = XmlDocument.CreateElement("ns9", "epostVarsel", Navnerom.Ns9);
                        {
                            XmlElement epostadresse = XmlDocument.CreateElement("ns9", "epostadresse", Navnerom.Ns9);
                            epostadresse.InnerText = Forsendelse.DigitalPost.EpostVarsel.Epostadresse;
                            epostVarsel.AppendChild(epostadresse);

                            XmlElement varseltekst = XmlDocument.CreateElement("ns9", "varseltekst", Navnerom.Ns9);
                            varseltekst.InnerText = Forsendelse.DigitalPost.EpostVarsel.Varslingstekst;
                            epostVarsel.AppendChild(varseltekst);

                            XmlElement repetisjoner = XmlDocument.CreateElement("ns9", "repetisjoner", Navnerom.Ns9);
                            //
                            epostVarsel.AppendChild(repetisjoner);
                        }
                        varsler.AppendChild(epostVarsel);
                    }
                    if (Forsendelse.DigitalPost.SmsVarsel != null)
                    {
                        XmlElement smsVarsel = XmlDocument.CreateElement("ns9", "smsVarsel", Navnerom.Ns9);
                        {
                            XmlElement mobiltelefonnummer = XmlDocument.CreateElement("ns9", "mobiltelefonnummer", Navnerom.Ns9);
                            mobiltelefonnummer.InnerText = Forsendelse.DigitalPost.SmsVarsel.Mobilnummer;
                            smsVarsel.AppendChild(mobiltelefonnummer);

                            XmlElement varseltekst = XmlDocument.CreateElement("ns9", "varseltekst", Navnerom.Ns9);
                            varseltekst.InnerText = Forsendelse.DigitalPost.SmsVarsel.Varslingstekst;
                            smsVarsel.AppendChild(varseltekst);

                            XmlElement repetisjoner = XmlDocument.CreateElement("ns9", "repetisjoner", Navnerom.Ns9);
                            //
                            smsVarsel.AppendChild(repetisjoner);
                        }
                        varsler.AppendChild(smsVarsel);
                    }
                }
                digitalPostInfo.AppendChild(varsler);
            }
            return digitalPostInfo;
        }

        private XmlElement DokumentfingerpakkeavtrykkElement()
        {
            XmlElement dokumentpakkefingeravtrykk = XmlDocument.CreateElement("ns9", "dokumentfingerpakkeavtrykk", Navnerom.Ns9);
            {
                XmlElement digestMethod = XmlDocument.CreateElement("ns5", "DigestMethod", Navnerom.Ns5);
                digestMethod.SetAttribute("Algorithm", "http://www.w3.org/2001/04/xmlenc#sha256");
                dokumentpakkefingeravtrykk.AppendChild(digestMethod);

                XmlElement digestValue = XmlDocument.CreateElement("ns5", "DigestValue", Navnerom.Ns5);
                digestValue.InnerText = Convert.ToBase64String(_managedSha256.ComputeHash(AsicEArkiv.Krypter(Forsendelse.DigitalPost.Mottaker.Sertifikat)));
                dokumentpakkefingeravtrykk.AppendChild(digestValue);
            }
            return dokumentpakkefingeravtrykk;
        }
    }
}
