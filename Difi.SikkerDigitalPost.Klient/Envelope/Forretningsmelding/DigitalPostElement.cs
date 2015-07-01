/** 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *         http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Security.Cryptography;
using System.Xml;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Aktører;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Post;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Varsel;
using Difi.SikkerDigitalPost.Klient.Domene.Enums;
using Difi.SikkerDigitalPost.Klient.Domene.Extensions;
using Difi.SikkerDigitalPost.Klient.Envelope.Abstract;
using Difi.SikkerDigitalPost.Klient.Extensions;
using Difi.SikkerDigitalPost.Klient.Utilities;

namespace Difi.SikkerDigitalPost.Klient.Envelope.Forretningsmelding
{
    internal class DigitalPostElement : EnvelopeXmlPart
    {
        private readonly SHA256Managed _managedSha256;

        public DigitalPostElement(EnvelopeSettings settings, XmlDocument context)
            : base(settings, context)
        {
            _managedSha256 = new SHA256Managed();
        }

        public override XmlNode Xml()
        {
            XmlElement digitalPostElement = Context.CreateElement("ns9", "digitalPost", NavneromUtility.DifiSdpSchema10);
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
            XmlElement avsender = Context.CreateElement("ns9", "avsender", NavneromUtility.DifiSdpSchema10);
            {
                XmlElement organisasjon = avsender.AppendChildElement("organisasjon", "ns9", NavneromUtility.DifiSdpSchema10, Context);
                organisasjon.SetAttribute("authority", "iso6523-actorid-upis");
                organisasjon.InnerText = Settings.Forsendelse.Avsender.Organisasjonsnummer.Iso6523();

                var avsenderIdentifikator = Settings.Forsendelse.Avsender.Avsenderidentifikator;
                if(avsenderIdentifikator != String.Empty){
                    XmlElement avsenderidentifikator = 
                        avsender.AppendChildElement("avsenderidentifikator", "ns9", NavneromUtility.DifiSdpSchema10, Context);
                    avsenderidentifikator.InnerText = avsenderIdentifikator;
                }

                XmlElement fakturaReferanse = avsender.AppendChildElement("fakturaReferanse", "ns9", NavneromUtility.DifiSdpSchema10, Context);
                fakturaReferanse.InnerText = Settings.Forsendelse.Avsender.Fakturareferanse;
            }
            return avsender;
        }

        private XmlElement MottakerElement()
        {
            var digitalPostMottaker = (DigitalPostMottaker) Settings.Forsendelse.PostInfo.Mottaker;
            XmlElement mottaker = Context.CreateElement("ns9", "mottaker", NavneromUtility.DifiSdpSchema10);
            {
                XmlElement person = mottaker.AppendChildElement("person", "ns9", NavneromUtility.DifiSdpSchema10, Context);
                {
                    XmlElement personidentifikator = person.AppendChildElement("personidentifikator", "ns9", NavneromUtility.DifiSdpSchema10, Context);
                    personidentifikator.InnerText = digitalPostMottaker.Personidentifikator;

                    XmlElement postkasseadresse = person.AppendChildElement("postkasseadresse", "ns9", NavneromUtility.DifiSdpSchema10, Context);
                    postkasseadresse.InnerText = digitalPostMottaker.Postkasseadresse;
                }
            }
            return mottaker;
        }

        private XmlElement DigitalPostInfoElement()
        {
            var digitalPostInfo = (DigitalPostInfo) Settings.Forsendelse.PostInfo;

            XmlElement digitalPostInfoElement = Context.CreateElement("ns9", "digitalPostInfo", NavneromUtility.DifiSdpSchema10);
            {
                XmlElement virkningstidspunkt = digitalPostInfoElement.AppendChildElement("virkningstidspunkt", "ns9", NavneromUtility.DifiSdpSchema10, Context);
                virkningstidspunkt.InnerText = digitalPostInfo.Virkningstidspunkt.ToStringWithUtcOffset();

                XmlElement aapningskvittering = digitalPostInfoElement.AppendChildElement("aapningskvittering", "ns9", NavneromUtility.DifiSdpSchema10, Context);
                aapningskvittering.InnerText = digitalPostInfo.Åpningskvittering.ToString().ToLower();

                XmlElement sikkerhetsnivaa = digitalPostInfoElement.AppendChildElement("sikkerhetsnivaa", "ns9", NavneromUtility.DifiSdpSchema10, Context);
                sikkerhetsnivaa.InnerText = ((int)digitalPostInfo.Sikkerhetsnivå).ToString();

                XmlElement ikkeSensitivTittel = digitalPostInfoElement.AppendChildElement("ikkeSensitivTittel", "ns9", NavneromUtility.DifiSdpSchema10, Context);
                ikkeSensitivTittel.SetAttribute("lang", Settings.Forsendelse.Språkkode.ToLower());
                ikkeSensitivTittel.InnerText = digitalPostInfo.IkkeSensitivTittel;

                XmlElement varsler = digitalPostInfoElement.AppendChildElement("varsler", "ns9", NavneromUtility.DifiSdpSchema10, Context);
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
            var digitalPostInfo = (DigitalPostInfo)Settings.Forsendelse.PostInfo;
            var smsVarsel = digitalPostInfo.SmsVarsel;
            return VarselElement(smsVarsel, "smsVarsel", "mobiltelefonnummer", smsVarsel.Mobilnummer, smsVarsel.Varslingstekst);
        }

        private XmlElement VarselElement(Varsel varselObjekt, string varselType, string kontakttype, string kontaktinfo, string varslingstekst)
        {
            XmlElement varsel = Context.CreateElement("ns9", varselType, NavneromUtility.DifiSdpSchema10);
            {
                XmlElement kontakt = varsel.AppendChildElement(kontakttype, "ns9", NavneromUtility.DifiSdpSchema10, Context);
                kontakt.InnerText = kontaktinfo;

                XmlElement varseltekst = varsel.AppendChildElement("varslingsTekst", "ns9", NavneromUtility.DifiSdpSchema10, Context);
                varseltekst.SetAttribute("lang", Settings.Forsendelse.Språkkode.ToUpper());
                varseltekst.InnerText = varslingstekst;

                XmlElement repetisjoner = varsel.AppendChildElement("repetisjoner", "ns9", NavneromUtility.DifiSdpSchema10, Context);
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
            XmlElement dokumentpakkefingeravtrykk = Context.CreateElement("ns9", "dokumentpakkefingeravtrykk", NavneromUtility.DifiSdpSchema10);
            {
                XmlElement digestMethod = dokumentpakkefingeravtrykk.AppendChildElement("DigestMethod", "ns5", NavneromUtility.XmlDsig, Context);
                digestMethod.SetAttribute("Algorithm", "http://www.w3.org/2001/04/xmlenc#sha256");

                XmlElement digestValue = dokumentpakkefingeravtrykk.AppendChildElement("DigestValue", "ns5", NavneromUtility.XmlDsig, Context);
                digestValue.InnerText = Convert.ToBase64String(_managedSha256.ComputeHash(
                    Settings.AsicEArkiv.Bytes));
            }
            return dokumentpakkefingeravtrykk;
        }
    }
}
