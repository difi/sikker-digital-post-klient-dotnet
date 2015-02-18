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
        public FysiskPostInfoElement(EnvelopeSettings settings, XmlDocument context) : base(settings, context)
        {
          
        }
        
        public override XmlNode Xml()
        {
            var fysiskPostInfo = ((FysiskPostInfo)Settings.Forsendelse.PostInfo);

            XmlElement fysiskPostInfoElement = Context.CreateElement("ns9", "fysiskPostInfo", Navnerom.Ns9);
            {
                fysiskPostInfoElement.AppendChild(MottakerElement(new FysiskPostMottaker()));

                XmlElement posttype = fysiskPostInfoElement.AppendChildElement("posttype", "ns9", Navnerom.Ns9, Context);
                posttype.InnerText = fysiskPostInfo.Posttype.ToString();

                XmlElement utskriftsfarge = fysiskPostInfoElement.AppendChildElement("utskriftsfarge", "ns9", Navnerom.Ns9, Context);
                utskriftsfarge.InnerText = UtskriftsfargeHelper.EnumToString(fysiskPostInfo.Utskriftsfarge);

                fysiskPostInfoElement.AppendChild(ReturElement());
            }

            return fysiskPostInfoElement;
        }

        private XmlNode MottakerElement(FysiskPostMottaker fysiskPostMottaker)
        {
            XmlElement mottaker = Context.CreateElement("ns9", "mottaker", Navnerom.Ns9);
            {
                XmlElement navn = mottaker.AppendChildElement("navn", "ns9", Navnerom.Ns9, Context);
                navn.InnerText = fysiskPostMottaker.Navn;

                XmlElement norskAdresseElement = mottaker.AppendChildElement("norskAdresse", "ns9", Navnerom.Ns9, Context);
                {
                    var adresse = fysiskPostMottaker.NorskAdresse;
                    
                    LeggTilAdresselinje(norskAdresseElement, adresse, 1);
                    LeggTilAdresselinje(norskAdresseElement, adresse, 2);
                    LeggTilAdresselinje(norskAdresseElement, adresse, 3);

                    XmlElement postnummer = norskAdresseElement.AppendChildElement("postnummer", "ns9", Navnerom.Ns9, Context);
                    postnummer.InnerText = fysiskPostMottaker.NorskAdresse.Postnummer.ToString();

                    XmlElement poststed = norskAdresseElement.AppendChildElement("poststed", "ns9", Navnerom.Ns9, Context);
                    poststed.InnerText = fysiskPostMottaker.NorskAdresse.Poststed;
                }
            }
            
            return mottaker;
        }

        private XmlNode ReturElement()
        {
            var fysiskPostInfo = ((FysiskPostInfo) Settings.Forsendelse.PostInfo);

            XmlElement retur = Context.CreateElement("ns9", "retur", Navnerom.Ns9);
            {
                XmlElement postHåndtering = retur.AppendChildElement("postHaandtering", "ns9", Navnerom.Ns9, Context);
                postHåndtering.InnerText = PosthåndteringHelper.EnumToString(fysiskPostInfo.Posthåndtering);

                retur.AppendChild(MottakerElement(new FysiskPostMottaker()));
            }

            return MottakerElement(new FysiskPostMottaker());

        }

        private void LeggTilAdresselinje(XmlElement norskAdresse, NorskAdresse adresse, int adresseLinjeNr)
        {
            var adresseLinjeData = adresse.AdresseLinje(adresseLinjeNr);

            if (adresseLinjeData == string.Empty)
                return;

            string adresselinje = "adresselinje" + adresseLinjeNr;
            XmlElement linje = Context.CreateElement("ns9", adresselinje, Navnerom.Ns9);
            linje.InnerText = adresseLinjeData;

            norskAdresse.AppendChild(linje);
        }
    }
}
