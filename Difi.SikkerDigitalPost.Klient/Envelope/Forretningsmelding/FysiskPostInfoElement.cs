using System;
using System.Xml;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.FysiskPost;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Post;
using Difi.SikkerDigitalPost.Klient.Domene.Enums;
using Difi.SikkerDigitalPost.Klient.Domene.Extensions;
using Difi.SikkerDigitalPost.Klient.Envelope.Abstract;
using Difi.SikkerDigitalPost.Klient.Utilities;

namespace Difi.SikkerDigitalPost.Klient.Envelope.Forretningsmelding
{
    internal class FysiskPostInfoElement : EnvelopeXmlPart
    {
        private readonly FysiskPostInfo _fysiskPostInfo;
        private readonly FysiskPostMottakerAbstrakt _returmottaker;

        public FysiskPostInfoElement(EnvelopeSettings settings, XmlDocument context) : base(settings, context)
        {
            _fysiskPostInfo = ((FysiskPostInfo)Settings.Forsendelse.PostInfo);
            _returmottaker = _fysiskPostInfo.ReturMottaker;
        }
        
        public override XmlNode Xml()
        {
            XmlElement fysiskPostInfoElement = Context.CreateElement("ns9", "fysiskPostInfo", NavneromUtility.DifiSdpSchema10);
            {
                var fysiskPostMottaker = (FysiskPostMottaker) _fysiskPostInfo.Mottaker;

                var mottakerElement = MottakerElement(fysiskPostMottaker.Navn, fysiskPostMottaker.Adresse); 
                fysiskPostInfoElement.AppendChild(mottakerElement);

                XmlElement posttype = fysiskPostInfoElement.AppendChildElement("posttype", "ns9", NavneromUtility.DifiSdpSchema10, Context);
                posttype.InnerText = _fysiskPostInfo.Posttype.ToString();

                XmlElement utskriftsfarge = fysiskPostInfoElement.AppendChildElement("utskriftsfarge", "ns9", NavneromUtility.DifiSdpSchema10, Context);
                utskriftsfarge.InnerText = UtskriftsfargeHelper.EnumToString(_fysiskPostInfo.Utskriftsfarge);

                XmlElement retur = Context.CreateElement("ns9", "retur", NavneromUtility.DifiSdpSchema10);
                {
                    retur.AppendChild(MottakerElement(_returmottaker.Navn, _returmottaker.Adresse));
                }
                
                fysiskPostInfoElement.AppendChild(ReturElement());
            }

            return fysiskPostInfoElement;
        }


        private XmlNode MottakerElement(string mottakerNavn, Adresse adresse)
        {
            XmlElement mottaker = Context.CreateElement("ns9", "mottaker", NavneromUtility.DifiSdpSchema10);
            {
                XmlElement navn = mottaker.AppendChildElement("navn", "ns9", NavneromUtility.DifiSdpSchema10, Context);
                navn.InnerText = mottakerNavn;

                if (adresse is NorskAdresse)
                    mottaker.AppendChild(NorskAdresseNode((NorskAdresse)adresse));
                else
                    mottaker.AppendChild(UtenlandskAdresseNode((UtenlandskAdresse)adresse));
            }
            
            return mottaker;
        }
        
        private XmlNode ReturElement()
        {
            XmlElement returElement = Context.CreateElement("ns9", "retur", NavneromUtility.DifiSdpSchema10);
            {
                XmlElement postHåndtering = returElement.AppendChildElement("postHaandtering", "ns9", NavneromUtility.DifiSdpSchema10, Context);
                postHåndtering.InnerText = PosthåndteringHelper.EnumToString(_fysiskPostInfo.Posthåndtering);

                returElement.AppendChild(MottakerElement(_returmottaker.Navn, _returmottaker.Adresse));
            }
            
            return returElement;
        }

        private XmlNode UtenlandskAdresseNode(UtenlandskAdresse adresse)
        {
            XmlElement utenlandskAdresseElement = Context.CreateElement("ns9", "utenlandskAdresse", NavneromUtility.DifiSdpSchema10);
            {
                LeggTilAdresselinje(utenlandskAdresseElement, adresse, 1);
                LeggTilAdresselinje(utenlandskAdresseElement, adresse, 2);
                LeggTilAdresselinje(utenlandskAdresseElement, adresse, 3);
                LeggTilAdresselinje(utenlandskAdresseElement, adresse, 4);

                if (adresse.Landkode != null)
                {
                    XmlElement landKode = utenlandskAdresseElement.AppendChildElement("landkode", "ns9", NavneromUtility.DifiSdpSchema10, Context);
                    landKode.InnerText = adresse.Landkode;
                }
                else
                {
                    XmlElement landKode = utenlandskAdresseElement.AppendChildElement("land", "ns9", NavneromUtility.DifiSdpSchema10, Context);
                    landKode.InnerText = adresse.Land;
                }
                
            }

            return utenlandskAdresseElement;
        }

        private XmlNode NorskAdresseNode(NorskAdresse adresse)
        {
            XmlElement norskAdresseElement = Context.CreateElement("ns9", "norskAdresse", NavneromUtility.DifiSdpSchema10);
            {
                LeggTilAdresselinje(norskAdresseElement, adresse, 1);
                LeggTilAdresselinje(norskAdresseElement, adresse, 2);
                LeggTilAdresselinje(norskAdresseElement, adresse, 3);

                XmlElement postnummer = norskAdresseElement.AppendChildElement("postnummer", "ns9", NavneromUtility.DifiSdpSchema10, Context);
                postnummer.InnerText = adresse.Postnummer;

                XmlElement poststed = norskAdresseElement.AppendChildElement("poststed", "ns9", NavneromUtility.DifiSdpSchema10, Context);
                poststed.InnerText = adresse.Poststed;
            }

            return norskAdresseElement;
        }

        private void LeggTilAdresselinje(XmlElement norskAdresse, Adresse adresse, int adresseLinjeNr)
        {
            var adresseLinjeData = adresse.AdresseLinje(adresseLinjeNr);
            
            if (String.IsNullOrEmpty(adresseLinjeData))
                return;

            string adresselinje = "adresselinje" + adresseLinjeNr;
            XmlElement linje = Context.CreateElement("ns9", adresselinje, NavneromUtility.DifiSdpSchema10);
            linje.InnerText = adresseLinjeData;

            norskAdresse.AppendChild(linje);
        }
    }
}
