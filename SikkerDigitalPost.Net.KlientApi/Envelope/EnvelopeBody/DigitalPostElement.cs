using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Xml;
using SikkerDigitalPost.Net.Domene.Entiteter;
using SikkerDigitalPost.Net.Domene.Entiteter.Varsel;
using SikkerDigitalPost.Net.Domene.Extensions;
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
            XmlElement digitalPostElement = XmlEnvelope.CreateElement("ns9", "digitalPost", Navnerom.Ns9);
            {
                digitalPostElement.AppendChild(AvsenderElement());
                digitalPostElement.AppendChild(MottakerElement());
                digitalPostElement.AppendChild(DigitalPostInfoElement());
                digitalPostElement.AppendChild(DokumentfingerpakkeavtrykkElement());
                digitalPostElement.PrependChild(XmlEnvelope.ImportNode(SignatureElement().GetXml(), true));
            }
            return digitalPostElement;
        }

        private SignedXml SignatureElement()
        {
            SignedXml signedXml = new SignedXmlWithAgnosticId(XmlEnvelope, Databehandler.Sertifikat);
            
            var reference = new Sha256Reference("");
            reference.AddTransform(new XmlDsigEnvelopedSignatureTransform());
            reference.AddTransform(new XmlDsigExcC14NTransform("ns3"));
            signedXml.AddReference(reference);

            var keyInfoX509Data = new KeyInfoX509Data(Databehandler.Sertifikat, X509IncludeOption.WholeChain);
            signedXml.KeyInfo.AddClause(keyInfoX509Data);

            signedXml.ComputeSignature();
          
            return signedXml;
        }

        private XmlElement AvsenderElement()
        {
            XmlElement avsender = XmlEnvelope.CreateElement("ns9", "avsender", Navnerom.Ns9);
            {
                XmlElement organisasjon = avsender.AppendChildElement("organisasjon", "ns9", Navnerom.Ns9, XmlEnvelope);
                organisasjon.SetAttribute("authority", "iso6523-actorid-upis");
                organisasjon.InnerText = Forsendelse.Behandlingsansvarlig.Organisasjonsnummer.Iso6523();
            }
            return avsender;
        }

        private XmlElement MottakerElement()
        {
            XmlElement mottaker = XmlEnvelope.CreateElement("ns9", "mottaker", Navnerom.Ns9);
            {
                XmlElement person = mottaker.AppendChildElement("person", "ns9", Navnerom.Ns9, XmlEnvelope);
                {
                    XmlElement personidentifikator = person.AppendChildElement("personidentifikator", "ns9", Navnerom.Ns9, XmlEnvelope);
                    personidentifikator.InnerText = Forsendelse.DigitalPost.Mottaker.Personidentifikator;

                    XmlElement postkasseadresse = person.AppendChildElement("postkasseadresse", "ns9", Navnerom.Ns9, XmlEnvelope);
                    postkasseadresse.InnerText = Forsendelse.DigitalPost.Mottaker.Postkasseadresse;
                }
            }
            return mottaker;
        }

        private XmlElement DigitalPostInfoElement()
        {
            XmlElement digitalPostInfo = XmlEnvelope.CreateElement("ns9", "digitalPostInfo", Navnerom.Ns9);
            {
                XmlElement aapningskvittering = digitalPostInfo.AppendChildElement("aapningskvittering", "ns9", Navnerom.Ns9, XmlEnvelope);
                aapningskvittering.InnerText = Forsendelse.DigitalPost.Åpningskvittering.ToString();

                XmlElement sikkerhetsnivaa = digitalPostInfo.AppendChildElement("sikkerhetsnivaa", "ns9", Navnerom.Ns9, XmlEnvelope);
                sikkerhetsnivaa.InnerText = Forsendelse.DigitalPost.Sikkerhetsnivå.ToString();

                XmlElement ikkeSensitivTittel = digitalPostInfo.AppendChildElement("ikkeSensitivTittel", "ns9", Navnerom.Ns9, XmlEnvelope);
                ikkeSensitivTittel.InnerText = Forsendelse.DigitalPost.IkkeSensitivTittel;

                XmlElement varsler = digitalPostInfo.AppendChildElement("varsler", "ns9", Navnerom.Ns9, XmlEnvelope);
                {
                    if (Forsendelse.DigitalPost.EpostVarsel != null)
                    {
                        varsler.AppendChild(EpostVarselElement());
                    }
                    if (Forsendelse.DigitalPost.SmsVarsel != null)
                    {
                        varsler.AppendChild(SmsVarselElement());
                    }
                }
            }
            return digitalPostInfo;
        }

        private XmlElement EpostVarselElement()
        {
            return VarselElement("epostVarsel", "epostadresse", Forsendelse.DigitalPost.EpostVarsel.Varslingstekst);
        }

        private XmlElement SmsVarselElement()
        {
            return VarselElement("smsVarsel", "mobiltelefonnummer", Forsendelse.DigitalPost.SmsVarsel.Varslingstekst);
        }

        private XmlElement VarselElement(string varselType, string kontakttype, string varslingstekst)
        {
            XmlElement varsel = XmlEnvelope.CreateElement("ns9", varselType, Navnerom.Ns9);
            {
                XmlElement kontakt = varsel.AppendChildElement(kontakttype, "ns9", Navnerom.Ns9, XmlEnvelope);
                kontakt.InnerText = Forsendelse.DigitalPost.SmsVarsel.Mobilnummer;

                XmlElement varseltekst = varsel.AppendChildElement("varseltekst", "ns9", Navnerom.Ns9, XmlEnvelope);
                varseltekst.InnerText = varslingstekst;
            }
            return varsel;
        }

        private XmlElement DokumentfingerpakkeavtrykkElement()
        {
            XmlElement dokumentpakkefingeravtrykk = XmlEnvelope.CreateElement("ns9", "dokumentfingerpakkeavtrykk", Navnerom.Ns9);
            {
                XmlElement digestMethod = dokumentpakkefingeravtrykk.AppendChildElement("DigestMethod", "ns5", Navnerom.Ns5, XmlEnvelope);
                digestMethod.SetAttribute("Algorithm", "http://www.w3.org/2001/04/xmlenc#sha256");

                XmlElement digestValue = dokumentpakkefingeravtrykk.AppendChildElement("DigestValue", "ns5", Navnerom.Ns5, XmlEnvelope);
                digestValue.InnerText = Convert.ToBase64String(_managedSha256.ComputeHash(
                    AsicEArkiv.Krypter(Forsendelse.DigitalPost.Mottaker.Sertifikat)));
            }
            return dokumentpakkefingeravtrykk;
        }
    }
}
