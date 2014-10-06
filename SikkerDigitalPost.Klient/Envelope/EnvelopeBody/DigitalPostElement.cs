using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Xml;
using SikkerDigitalPost.Domene.Entiteter.Aktører;
using SikkerDigitalPost.Domene.Entiteter.Post;
using SikkerDigitalPost.Domene.Entiteter.Varsel;
using SikkerDigitalPost.Klient.Xml;
using SikkerDigitalPost.Domene.Extensions;

namespace SikkerDigitalPost.Klient.Envelope.EnvelopeBody
{
    internal class DigitalPostElement : XmlPart
    {
        private readonly SHA256Managed _managedSha256;
        
        public DigitalPostElement(Envelope rot) : base(rot)
        {
            _managedSha256 = new SHA256Managed();
        }

        public override XmlElement Xml()
        {
            XmlElement digitalPostElement = Rot.EnvelopeXml.CreateElement("ns9", "digitalPost", Navnerom.Ns9);
            {
                digitalPostElement.AppendChild(AvsenderElement());
                digitalPostElement.AppendChild(MottakerElement());
                digitalPostElement.AppendChild(DigitalPostInfoElement());
                digitalPostElement.AppendChild(DokumentfingerpakkeavtrykkElement());
                digitalPostElement.PrependChild(Rot.EnvelopeXml.ImportNode(SignatureElement().GetXml(), true));
            }
            return digitalPostElement;
        }

        private SignedXml SignatureElement()
        {
            SignedXml signedXml = new SignedXmlWithAgnosticId(Rot.EnvelopeXml, Rot.Databehandler.Sertifikat);
            
            var reference = new Sha256Reference("");
            reference.AddTransform(new XmlDsigEnvelopedSignatureTransform());
            reference.AddTransform(new XmlDsigExcC14NTransform("ns3"));
            signedXml.AddReference(reference);

            var keyInfoX509Data = new KeyInfoX509Data(Rot.Databehandler.Sertifikat);
            signedXml.KeyInfo.AddClause(keyInfoX509Data);

            signedXml.ComputeSignature();
          
            return signedXml;
        }

        private XmlElement AvsenderElement()
        {
            XmlElement avsender = Rot.EnvelopeXml.CreateElement("ns9", "avsender", Navnerom.Ns9);
            {
                XmlElement organisasjon = avsender.AppendChildElement("organisasjon", "ns9", Navnerom.Ns9, Rot.EnvelopeXml);
                organisasjon.SetAttribute("authority", "iso6523-actorid-upis");
                organisasjon.InnerText = Rot.Forsendelse.Behandlingsansvarlig.Organisasjonsnummer.Iso6523();
            }
            return avsender;
        }

        private XmlElement MottakerElement()
        {
            XmlElement mottaker = Rot.EnvelopeXml.CreateElement("ns9", "mottaker", Navnerom.Ns9);
            {
                XmlElement person = mottaker.AppendChildElement("person", "ns9", Navnerom.Ns9, Rot.EnvelopeXml);
                {
                    XmlElement personidentifikator = person.AppendChildElement("personidentifikator", "ns9", Navnerom.Ns9, Rot.EnvelopeXml);
                    personidentifikator.InnerText = Rot.Forsendelse.DigitalPost.Mottaker.Personidentifikator;

                    XmlElement postkasseadresse = person.AppendChildElement("postkasseadresse", "ns9", Navnerom.Ns9, Rot.EnvelopeXml);
                    postkasseadresse.InnerText = Rot.Forsendelse.DigitalPost.Mottaker.Postkasseadresse;
                }
            }
            return mottaker;
        }

        private XmlElement DigitalPostInfoElement()
        {
            XmlElement digitalPostInfo = Rot.EnvelopeXml.CreateElement("ns9", "digitalPostInfo", Navnerom.Ns9);
            {
                XmlElement aapningskvittering = digitalPostInfo.AppendChildElement("aapningskvittering", "ns9", Navnerom.Ns9, Rot.EnvelopeXml);
                aapningskvittering.InnerText = Rot.Forsendelse.DigitalPost.Åpningskvittering.ToString().ToLower();

                XmlElement sikkerhetsnivaa = digitalPostInfo.AppendChildElement("sikkerhetsnivaa", "ns9", Navnerom.Ns9, Rot.EnvelopeXml);
                sikkerhetsnivaa.InnerText = ((int)Rot.Forsendelse.DigitalPost.Sikkerhetsnivå).ToString();

                XmlElement ikkeSensitivTittel = digitalPostInfo.AppendChildElement("ikkeSensitivTittel", "ns9", Navnerom.Ns9, Rot.EnvelopeXml);
                ikkeSensitivTittel.SetAttribute("lang", Rot.Forsendelse.Språkkode.ToLower());
                ikkeSensitivTittel.InnerText = Rot.Forsendelse.DigitalPost.IkkeSensitivTittel;

                if (Rot.Forsendelse.DigitalPost.SmsVarsel != null || Rot.Forsendelse.DigitalPost.EpostVarsel != null)
                {
                    XmlElement varsler = digitalPostInfo.AppendChildElement("varsler", "ns9", Navnerom.Ns9, Rot.EnvelopeXml);
                    {
                        if (Rot.Forsendelse.DigitalPost.EpostVarsel != null)
                        {
                            varsler.AppendChild(EpostVarselElement());
                        }
                        if (Rot.Forsendelse.DigitalPost.SmsVarsel != null)
                        {
                            varsler.AppendChild(SmsVarselElement());
                        }
                    }
                }
            }
            return digitalPostInfo;
        }

        private XmlElement EpostVarselElement()
        {
            var epostVarsel = Rot.Forsendelse.DigitalPost.EpostVarsel;
            return VarselElement(epostVarsel, "epostVarsel", "epostadresse", epostVarsel.Epostadresse, epostVarsel.Varslingstekst);
        }

        private XmlElement SmsVarselElement()
        {
            var smsVarsel = Rot.Forsendelse.DigitalPost.SmsVarsel;
            return VarselElement(smsVarsel, "smsVarsel", "mobiltelefonnummer", smsVarsel.Mobilnummer, smsVarsel.Varslingstekst);
        }

        private XmlElement VarselElement(Varsel varselObjekt, string varselType, string kontakttype, string kontaktinfo, string varslingstekst)
        {
            XmlElement varsel = Rot.EnvelopeXml.CreateElement("ns9", varselType, Navnerom.Ns9);
            {
                XmlElement kontakt = varsel.AppendChildElement(kontakttype, "ns9", Navnerom.Ns9, Rot.EnvelopeXml);
                kontakt.InnerText = kontaktinfo;
                
                XmlElement varseltekst = varsel.AppendChildElement("varslingsTekst", "ns9", Navnerom.Ns9, Rot.EnvelopeXml);
                varseltekst.SetAttribute("lang", Rot.Forsendelse.Språkkode.ToLower());
                varseltekst.InnerText = varslingstekst;

                XmlElement repetisjoner = varsel.AppendChildElement("repetisjoner", "ns9", Navnerom.Ns9, Rot.EnvelopeXml);
                {
                    foreach (var v in varselObjekt.VarselEtterDager)
                    {
                        var dagerEtter = repetisjoner.AppendChildElement("dagerEtter", "ns9", Navnerom.Ns9, Rot.EnvelopeXml);
                        dagerEtter.InnerText = v.ToString();
                    }
                }
            }
            return varsel;
        }

        private XmlElement DokumentfingerpakkeavtrykkElement()
        {
            XmlElement dokumentpakkefingeravtrykk = Rot.EnvelopeXml.CreateElement("ns9", "dokumentpakkefingeravtrykk", Navnerom.Ns9);
            {
                XmlElement digestMethod = dokumentpakkefingeravtrykk.AppendChildElement("DigestMethod", "ns5", Navnerom.Ns5, Rot.EnvelopeXml);
                digestMethod.SetAttribute("Algorithm", "http://www.w3.org/2001/04/xmlenc#sha256");

                XmlElement digestValue = dokumentpakkefingeravtrykk.AppendChildElement("DigestValue", "ns5", Navnerom.Ns5, Rot.EnvelopeXml);
                digestValue.InnerText = Convert.ToBase64String(_managedSha256.ComputeHash(
                    Rot.AsicEArkiv.KrypterteBytes(Rot.Forsendelse.DigitalPost.Mottaker.Sertifikat)));
            }
            return dokumentpakkefingeravtrykk;
        }
    }
}
