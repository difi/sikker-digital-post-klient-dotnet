using System;
using System.Xml;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Aktører;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Post;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Varsel;
using Difi.SikkerDigitalPost.Klient.Domene.Enums;
using Difi.SikkerDigitalPost.Klient.Domene.Extensions;
using Difi.SikkerDigitalPost.Klient.Envelope.Abstract;
using Difi.SikkerDigitalPost.Klient.Utilities;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Digests;

namespace Difi.SikkerDigitalPost.Klient.Envelope.Forretningsmelding
{
    internal class DigitalPostElement : EnvelopeXmlPart
    {
        private readonly IDigest _managedSha256;

        public DigitalPostElement(EnvelopeSettings settings, XmlDocument context)
            : base(settings, context)
        {
            _managedSha256 = new Sha256Digest();
        }

        public override XmlNode Xml()
        {
            var digitalPostElement = Context.CreateElement("ns9", "digitalPost", NavneromUtility.DifiSdpSchema10);
            {
                digitalPostElement.AppendChild(AvsenderElement());

                if (Settings.Forsendelse.Sendes(Postmetode.Digital))
                {
                    digitalPostElement.AppendChild(MottakerElement());
                    digitalPostElement.AppendChild(DigitalPostInfoElement());
                }
                else
                {
                    digitalPostElement.AppendChild(FysiskPostInfoElement());
                }
                
                digitalPostElement.AppendChild(DokumentpakkeFingeravtrykkElement());
            }
            return digitalPostElement;
        }

        private XmlElement AvsenderElement()
        {
            var avsender = Context.CreateElement("ns9", "avsender", NavneromUtility.DifiSdpSchema10);
            {
                var organisasjon = avsender.AppendChildElement("organisasjon", "ns9", NavneromUtility.DifiSdpSchema10, Context);
                organisasjon.SetAttribute("authority", "iso6523-actorid-upis");
                organisasjon.InnerText = Settings.Forsendelse.Avsender.Organisasjonsnummer.WithCountryCode;

                var avsenderIdentifikator = Settings.Forsendelse.Avsender.Avsenderidentifikator;
                if (avsenderIdentifikator != string.Empty)
                {
                    var avsenderidentifikator =
                        avsender.AppendChildElement("avsenderidentifikator", "ns9", NavneromUtility.DifiSdpSchema10, Context);
                    avsenderidentifikator.InnerText = avsenderIdentifikator;
                }

                var fakturaReferanse = avsender.AppendChildElement("fakturaReferanse", "ns9", NavneromUtility.DifiSdpSchema10, Context);
                fakturaReferanse.InnerText = Settings.Forsendelse.Avsender.Fakturareferanse;
            }
            return avsender;
        }

        private XmlElement MottakerElement()
        {
            var digitalPostMottaker = (DigitalPostMottaker) Settings.Forsendelse.PostInfo.Mottaker;
            var mottaker = Context.CreateElement("ns9", "mottaker", NavneromUtility.DifiSdpSchema10);
            {
                var person = mottaker.AppendChildElement("person", "ns9", NavneromUtility.DifiSdpSchema10, Context);
                {
                    var personidentifikator = person.AppendChildElement("personidentifikator", "ns9", NavneromUtility.DifiSdpSchema10, Context);
                    personidentifikator.InnerText = digitalPostMottaker.Personidentifikator;

                    var postkasseadresse = person.AppendChildElement("postkasseadresse", "ns9", NavneromUtility.DifiSdpSchema10, Context);
                    postkasseadresse.InnerText = digitalPostMottaker.Postkasseadresse;
                }
            }
            return mottaker;
        }

        private XmlElement DigitalPostInfoElement()
        {
            var digitalPostInfo = (DigitalPostInfo) Settings.Forsendelse.PostInfo;

            var digitalPostInfoElement = Context.CreateElement("ns9", "digitalPostInfo", NavneromUtility.DifiSdpSchema10);
            {
                var virkningstidspunkt = digitalPostInfoElement.AppendChildElement("virkningstidspunkt", "ns9", NavneromUtility.DifiSdpSchema10, Context);
                virkningstidspunkt.InnerText = digitalPostInfo.Virkningstidspunkt.ToStringWithUtcOffset();

                var aapningskvittering = digitalPostInfoElement.AppendChildElement("aapningskvittering", "ns9", NavneromUtility.DifiSdpSchema10, Context);
                aapningskvittering.InnerText = digitalPostInfo.Åpningskvittering.ToString().ToLower();

                var sikkerhetsnivaa = digitalPostInfoElement.AppendChildElement("sikkerhetsnivaa", "ns9", NavneromUtility.DifiSdpSchema10, Context);
                sikkerhetsnivaa.InnerText = ((int) digitalPostInfo.Sikkerhetsnivå).ToString();

                var ikkeSensitivTittel = digitalPostInfoElement.AppendChildElement("ikkeSensitivTittel", "ns9", NavneromUtility.DifiSdpSchema10, Context);
                ikkeSensitivTittel.SetAttribute("lang", Settings.Forsendelse.Språkkode.ToLower());
                ikkeSensitivTittel.InnerText = digitalPostInfo.IkkeSensitivTittel;

                var varsler = digitalPostInfoElement.AppendChildElement("varsler", "ns9", NavneromUtility.DifiSdpSchema10, Context);
                {
                    if (digitalPostInfo.EpostVarsel != null)
                    {
                        varsler.AppendChild(EpostVarselElement());
                    }
                    if (digitalPostInfo.SmsVarsel != null)
                    {
                        varsler.AppendChild(SmsVarselElement());
                    }
                }
            }
            return digitalPostInfoElement;
        }

        private XmlNode FysiskPostInfoElement()
        {
            var fysiskPost = new FysiskPostInfoElement(Settings, Context);
            return fysiskPost.Xml();
        }

        private XmlElement EpostVarselElement()
        {
            var digitalPostInfo = (DigitalPostInfo) Settings.Forsendelse.PostInfo;
            var epostVarsel = digitalPostInfo.EpostVarsel;
            return VarselElement(epostVarsel, "epostVarsel", "epostadresse", epostVarsel.Epostadresse, epostVarsel.Varslingstekst);
        }

        private XmlElement SmsVarselElement()
        {
            var digitalPostInfo = (DigitalPostInfo) Settings.Forsendelse.PostInfo;
            var smsVarsel = digitalPostInfo.SmsVarsel;
            return VarselElement(smsVarsel, "smsVarsel", "mobiltelefonnummer", smsVarsel.Mobilnummer, smsVarsel.Varslingstekst);
        }

        private XmlElement VarselElement(Varsel varselObjekt, string varselType, string kontakttype, string kontaktinfo, string varslingstekst)
        {
            var varsel = Context.CreateElement("ns9", varselType, NavneromUtility.DifiSdpSchema10);
            {
                var kontakt = varsel.AppendChildElement(kontakttype, "ns9", NavneromUtility.DifiSdpSchema10, Context);
                kontakt.InnerText = kontaktinfo;

                var varseltekst = varsel.AppendChildElement("varslingsTekst", "ns9", NavneromUtility.DifiSdpSchema10, Context);
                varseltekst.SetAttribute("lang", Settings.Forsendelse.Språkkode.ToUpper());
                varseltekst.InnerText = varslingstekst;

                var repetisjoner = varsel.AppendChildElement("repetisjoner", "ns9", NavneromUtility.DifiSdpSchema10, Context);
                {
                    foreach (var v in varselObjekt.VarselEtterDager)
                    {
                        var dagerEtter = repetisjoner.AppendChildElement("dagerEtter", "ns9", NavneromUtility.DifiSdpSchema10, Context);
                        dagerEtter.InnerText = v.ToString();
                    }
                }
            }
            return varsel;
        }

        private XmlElement DokumentpakkeFingeravtrykkElement()
        {
            var dokumentpakkefingeravtrykk = Context.CreateElement("ns9", "dokumentpakkefingeravtrykk", NavneromUtility.DifiSdpSchema10);
            {
                var digestMethod = dokumentpakkefingeravtrykk.AppendChildElement("DigestMethod", "ns5", NavneromUtility.XmlDsig, Context);
                digestMethod.SetAttribute("Algorithm", "http://www.w3.org/2001/04/xmlenc#sha256");

                var digestValue = dokumentpakkefingeravtrykk.AppendChildElement("DigestValue", "ns5", NavneromUtility.XmlDsig, Context);
                
                var hash = new byte[_managedSha256.GetDigestSize()];
                _managedSha256.BlockUpdate(Settings.DocumentBundle.BundleBytes, 0, Settings.DocumentBundle.BundleBytes.Length);
                _managedSha256.DoFinal(hash, 0);
                
                digestValue.InnerText = Convert.ToBase64String(hash);
            }
            return dokumentpakkefingeravtrykk;
        }
    }
}