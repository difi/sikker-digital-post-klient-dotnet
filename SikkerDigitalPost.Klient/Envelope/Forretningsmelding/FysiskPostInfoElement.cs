using System;
using System.Xml;
using SikkerDigitalPost.Domene.Entiteter.FysiskPost;
using SikkerDigitalPost.Domene.Entiteter.Post;
using SikkerDigitalPost.Domene.Enums;
using SikkerDigitalPost.Domene.Extensions;
using SikkerDigitalPost.Klient.Envelope.Abstract;

namespace SikkerDigitalPost.Klient.Envelope.Forretningsmelding
{
    internal class FysiskPostInfoElement : EnvelopeXmlPart
    {
        private readonly FysiskPostInfo _fysiskPostInfo;
        private readonly FysiskPostMottaker _returmottaker;

        public FysiskPostInfoElement(EnvelopeSettings settings, XmlDocument context) : base(settings, context)
        {
            _fysiskPostInfo = ((FysiskPostInfo)Settings.Forsendelse.PostInfo);
            _returmottaker = _fysiskPostInfo.ReturMottaker;
        }
        
        public override XmlNode Xml()
        {
            XmlElement fysiskPostInfoElement = Context.CreateElement("ns9", "fysiskPostInfo", Navnerom.Ns9);
            {
                var fysiskPostMottaker = (FysiskPostMottaker) _fysiskPostInfo.Mottaker;
                var mottakerElement = MottakerElement(fysiskPostMottaker.Navn, fysiskPostMottaker.NorskAdresse); 
                fysiskPostInfoElement.AppendChild(mottakerElement);

                XmlElement posttype = fysiskPostInfoElement.AppendChildElement("posttype", "ns9", Navnerom.Ns9, Context);
                posttype.InnerText = _fysiskPostInfo.Posttype.ToString();

                XmlElement utskriftsfarge = fysiskPostInfoElement.AppendChildElement("utskriftsfarge", "ns9", Navnerom.Ns9, Context);
                utskriftsfarge.InnerText = UtskriftsfargeHelper.EnumToString(_fysiskPostInfo.Utskriftsfarge);

                XmlElement retur = Context.CreateElement("ns9", "retur", Navnerom.Ns9);
                {
                    retur.AppendChild(MottakerElement(_returmottaker.Navn, _returmottaker.NorskAdresse));
                }
                
                fysiskPostInfoElement.AppendChild(ReturElement());
            }

            return fysiskPostInfoElement;
        }

        private XmlNode MottakerElement(string mottakerNavn, NorskAdresse norskAdresse)
        {
            XmlElement mottaker = Context.CreateElement("ns9", "mottaker", Navnerom.Ns9);
            {
                XmlElement navn = mottaker.AppendChildElement("navn", "ns9", Navnerom.Ns9, Context);
                navn.InnerText = mottakerNavn;

                mottaker.AppendChild(AdresseNode(norskAdresse));
            }
            
            return mottaker;
        }
        
        private XmlNode ReturElement()
        {
            XmlElement returElement = Context.CreateElement("ns9", "retur", Navnerom.Ns9);
            {
                XmlElement postHåndtering = returElement.AppendChildElement("postHaandtering", "ns9", Navnerom.Ns9, Context);
                postHåndtering.InnerText = PosthåndteringHelper.EnumToString(_fysiskPostInfo.Posthåndtering);

                returElement.AppendChild(MottakerElement(_returmottaker.Navn, _returmottaker.NorskAdresse));
            }
            
            return returElement;
        }

        private XmlNode AdresseNode(NorskAdresse adresse)
        {
            XmlElement norskAdresseElement = Context.CreateElement("ns9", "norskAdresse", Navnerom.Ns9);
            {
                LeggTilAdresselinje(norskAdresseElement, adresse, 1);
                LeggTilAdresselinje(norskAdresseElement, adresse, 2);
                LeggTilAdresselinje(norskAdresseElement, adresse, 3);

                XmlElement postnummer = norskAdresseElement.AppendChildElement("postnummer", "ns9", Navnerom.Ns9, Context);
                postnummer.InnerText = adresse.Postnummer.ToString();

                XmlElement poststed = norskAdresseElement.AppendChildElement("poststed", "ns9", Navnerom.Ns9, Context);
                poststed.InnerText = adresse.Poststed;
            }

            return norskAdresseElement;
        }

        private void LeggTilAdresselinje(XmlElement norskAdresse, NorskAdresse adresse, int adresseLinjeNr)
        {
            var adresseLinjeData = adresse.AdresseLinje(adresseLinjeNr);

            if (String.IsNullOrEmpty(adresseLinjeData))
                return;

            string adresselinje = "adresselinje" + adresseLinjeNr;
            XmlElement linje = Context.CreateElement("ns9", adresselinje, Navnerom.Ns9);
            linje.InnerText = adresseLinjeData;

            norskAdresse.AppendChild(linje);
        }
    }
}
