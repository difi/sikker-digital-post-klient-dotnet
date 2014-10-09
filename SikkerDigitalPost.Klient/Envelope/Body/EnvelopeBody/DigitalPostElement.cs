using System;
using System.Security.Cryptography;
using System.Xml;
using SikkerDigitalPost.Domene.Entiteter.Varsel;
using SikkerDigitalPost.Domene.Extensions;

namespace SikkerDigitalPost.Klient.Envelope.Body.EnvelopeBody
{
    internal class DigitalPostElement : XmlPart
    {
        private readonly SHA256Managed _managedSha256;

        public DigitalPostElement(EnvelopeSettings settings, XmlDocument context) : base(settings, context)
        {
            _managedSha256 = new SHA256Managed();
        }

        public override XmlNode Xml()
        {
            XmlElement digitalPostElement = Context.CreateElement("ns9", "digitalPost", Navnerom.Ns9);
            {
                digitalPostElement.AppendChild(AvsenderElement());
                digitalPostElement.AppendChild(MottakerElement());
                digitalPostElement.AppendChild(DigitalPostInfoElement());
                digitalPostElement.AppendChild(DokumentfingerpakkeavtrykkElement());
            }
            return digitalPostElement;
        }
        
        private XmlElement AvsenderElement()
        {
            XmlElement avsender = Context.CreateElement("ns9", "avsender", Navnerom.Ns9);
            {
                XmlElement organisasjon = avsender.AppendChildElement("organisasjon", "ns9", Navnerom.Ns9, Context);
                organisasjon.SetAttribute("authority", "iso6523-actorid-upis");
                organisasjon.InnerText = Settings.Forsendelse.Behandlingsansvarlig.Organisasjonsnummer.Iso6523();

                XmlElement fakturaReferanse = avsender.AppendChildElement("fakturaReferanse", "ns9", Navnerom.Ns9, Context);
                fakturaReferanse.InnerText = Settings.Forsendelse.Behandlingsansvarlig.Fakturareferanse;
            }
            return avsender;
        }

        private XmlElement MottakerElement()
        {
            XmlElement mottaker = Context.CreateElement("ns9", "mottaker", Navnerom.Ns9);
            {
                XmlElement person = mottaker.AppendChildElement("person", "ns9", Navnerom.Ns9, Context);
                {
                    XmlElement personidentifikator = person.AppendChildElement("personidentifikator", "ns9", Navnerom.Ns9, Context);
                    personidentifikator.InnerText = Settings.Forsendelse.DigitalPost.Mottaker.Personidentifikator;

                    XmlElement postkasseadresse = person.AppendChildElement("postkasseadresse", "ns9", Navnerom.Ns9, Context);
                    postkasseadresse.InnerText = Settings.Forsendelse.DigitalPost.Mottaker.Postkasseadresse;
                }
            }
            return mottaker;
        }

        private XmlElement DigitalPostInfoElement()
        {
            XmlElement digitalPostInfo = Context.CreateElement("ns9", "digitalPostInfo", Navnerom.Ns9);
            {
                XmlElement aapningskvittering = digitalPostInfo.AppendChildElement("aapningskvittering", "ns9", Navnerom.Ns9, Context);
                aapningskvittering.InnerText = Settings.Forsendelse.DigitalPost.Åpningskvittering.ToString().ToLower();

                //XmlElement virkningsdato = digitalPostInfo.AppendChildElement("virkningsdato", "ns9", Navnerom.Ns9, Context);
                //virkningsdato.InnerText = Settings.Forsendelse.DigitalPost.Virkningsdato.ToString("yyyy-MM-dd");

                XmlElement sikkerhetsnivaa = digitalPostInfo.AppendChildElement("sikkerhetsnivaa", "ns9", Navnerom.Ns9, Context);
                sikkerhetsnivaa.InnerText = ((int)Settings.Forsendelse.DigitalPost.Sikkerhetsnivå).ToString();

                XmlElement ikkeSensitivTittel = digitalPostInfo.AppendChildElement("ikkeSensitivTittel", "ns9", Navnerom.Ns9, Context);
                ikkeSensitivTittel.SetAttribute("lang", Settings.Forsendelse.Språkkode.ToLower());
                ikkeSensitivTittel.InnerText = Settings.Forsendelse.DigitalPost.IkkeSensitivTittel;

                XmlElement varsler = digitalPostInfo.AppendChildElement("varsler", "ns9", Navnerom.Ns9, Context);
                {
                    if (Settings.Forsendelse.DigitalPost.SmsVarsel != null || Settings.Forsendelse.DigitalPost.EpostVarsel != null)
                    {
                        if (Settings.Forsendelse.DigitalPost.EpostVarsel != null)
                        {
                            varsler.AppendChild(EpostVarselElement());
                        }
                        if (Settings.Forsendelse.DigitalPost.SmsVarsel != null)
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
            var epostVarsel = Settings.Forsendelse.DigitalPost.EpostVarsel;
            return VarselElement(epostVarsel, "epostVarsel", "epostadresse", epostVarsel.Epostadresse, epostVarsel.Varslingstekst);
        }

        private XmlElement SmsVarselElement()
        {
            var smsVarsel = Settings.Forsendelse.DigitalPost.SmsVarsel;
            return VarselElement(smsVarsel, "smsVarsel", "mobiltelefonnummer", smsVarsel.Mobilnummer, smsVarsel.Varslingstekst);
        }

        private XmlElement VarselElement(Varsel varselObjekt, string varselType, string kontakttype, string kontaktinfo, string varslingstekst)
        {
            XmlElement varsel = Context.CreateElement("ns9", varselType, Navnerom.Ns9);
            {
                XmlElement kontakt = varsel.AppendChildElement(kontakttype, "ns9", Navnerom.Ns9, Context);
                kontakt.InnerText = kontaktinfo;
                
                XmlElement varseltekst = varsel.AppendChildElement("varslingsTekst", "ns9", Navnerom.Ns9, Context);
                varseltekst.SetAttribute("lang", Settings.Forsendelse.Språkkode.ToUpper());
                varseltekst.InnerText = varslingstekst;

                XmlElement repetisjoner = varsel.AppendChildElement("repetisjoner", "ns9", Navnerom.Ns9, Context);
                {
                    foreach (var v in varselObjekt.VarselEtterDager)
                    {
                        var dagerEtter = repetisjoner.AppendChildElement("dagerEtter", "ns9", Navnerom.Ns9, Context);
                        dagerEtter.InnerText = v.ToString();
                    }
                }
            }
            return varsel;
        }

        private XmlElement DokumentfingerpakkeavtrykkElement()
        {
            XmlElement dokumentpakkefingeravtrykk = Context.CreateElement("ns9", "dokumentpakkefingeravtrykk", Navnerom.Ns9);
            {
                XmlElement digestMethod = dokumentpakkefingeravtrykk.AppendChildElement("DigestMethod", "ns5", Navnerom.Ns5, Context);
                digestMethod.SetAttribute("Algorithm", "http://www.w3.org/2001/04/xmlenc#sha256");

                XmlElement digestValue = dokumentpakkefingeravtrykk.AppendChildElement("DigestValue", "ns5", Navnerom.Ns5, Context);
                digestValue.InnerText = Convert.ToBase64String(_managedSha256.ComputeHash(
                    Settings.AsicEArkiv.Bytes));
            }
            return dokumentpakkefingeravtrykk;
        }
    }
}
