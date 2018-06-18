using System.Linq;
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

        public FysiskPostInfoElement(EnvelopeSettings settings, XmlDocument context)
            : base(settings, context)
        {
            _fysiskPostInfo = (FysiskPostInfo) Settings.Forsendelse.PostInfo;
            _returmottaker = _fysiskPostInfo.ReturpostMottaker;
        }

        public override XmlNode Xml()
        {
            var fysiskPostInfoElement = Context.CreateElement("ns9", "fysiskPostInfo", NavneromUtility.DifiSdpSchema10);
            {
                var fysiskPostMottaker = (FysiskPostMottaker) _fysiskPostInfo.Mottaker;

                var mottakerElement = MottakerElement(fysiskPostMottaker.Navn, fysiskPostMottaker.Adresse);
                fysiskPostInfoElement.AppendChild(mottakerElement);

                var posttype = fysiskPostInfoElement.AppendChildElement("posttype", "ns9", NavneromUtility.DifiSdpSchema10, Context);
                posttype.InnerText = _fysiskPostInfo.Posttype.ToString();

                var utskriftsfarge = fysiskPostInfoElement.AppendChildElement("utskriftsfarge", "ns9", NavneromUtility.DifiSdpSchema10, Context);
                utskriftsfarge.InnerText = UtskriftsfargeHelper.EnumToString(_fysiskPostInfo.Utskriftsfarge);

                var retur = Context.CreateElement("ns9", "retur", NavneromUtility.DifiSdpSchema10);
                {
                    retur.AppendChild(MottakerElement(_returmottaker.Navn, _returmottaker.Adresse));
                }

                fysiskPostInfoElement.AppendChild(ReturElement());

                if (_fysiskPostInfo.Printinstruksjoner.Any())
                {
                    fysiskPostInfoElement.AppendChild(PrintinstruksjonerElement());
                }
            }

            return fysiskPostInfoElement;
        }

        private XmlNode MottakerElement(string mottakerNavn, Adresse adresse)
        {
            var mottaker = Context.CreateElement("ns9", "mottaker", NavneromUtility.DifiSdpSchema10);
            {
                var navn = mottaker.AppendChildElement("navn", "ns9", NavneromUtility.DifiSdpSchema10, Context);
                navn.InnerText = mottakerNavn;

                if (adresse is NorskAdresse)
                    mottaker.AppendChild(NorskAdresseNode((NorskAdresse) adresse));
                else
                    mottaker.AppendChild(UtenlandskAdresseNode((UtenlandskAdresse) adresse));
            }

            return mottaker;
        }

        private XmlNode ReturElement()
        {
            var returElement = Context.CreateElement("ns9", "retur", NavneromUtility.DifiSdpSchema10);
            {
                var postHåndtering = returElement.AppendChildElement("postHaandtering", "ns9", NavneromUtility.DifiSdpSchema10, Context);
                postHåndtering.InnerText = PosthåndteringHelper.EnumToString(_fysiskPostInfo.Posthåndtering);

                returElement.AppendChild(MottakerElement(_returmottaker.Navn, _returmottaker.Adresse));
            }

            return returElement;
        }

        private XmlElement PrintinstruksjonerElement()
        {
            var printinstruksjonerElement = Context.CreateElement("ns9", "printinstruksjoner", NavneromUtility.DifiSdpSchema10);
            {
                foreach (var printinstruksjon in _fysiskPostInfo.Printinstruksjoner)
                {
                    var printinstruksjonElement = Context.CreateElement("ns9", "printinstruksjon", NavneromUtility.DifiSdpSchema10);
                    {
                        var navnElement = printinstruksjonElement.AppendChildElement("navn", "ns9", NavneromUtility.DifiSdpSchema10, Context);
                        navnElement.InnerText = printinstruksjon.Navn;
                        var verdiElement = printinstruksjonElement.AppendChildElement("verdi", "ns9", NavneromUtility.DifiSdpSchema10, Context);
                        verdiElement.InnerText = printinstruksjon.Verdi;
                    }
                    printinstruksjonerElement.AppendChild(printinstruksjonElement);
                }
            }
            return printinstruksjonerElement;
        }

        private XmlNode UtenlandskAdresseNode(UtenlandskAdresse adresse)
        {
            var utenlandskAdresseElement = Context.CreateElement("ns9", "utenlandskAdresse", NavneromUtility.DifiSdpSchema10);
            {
                LeggTilAdresselinje(utenlandskAdresseElement, adresse, 1);
                LeggTilAdresselinje(utenlandskAdresseElement, adresse, 2);
                LeggTilAdresselinje(utenlandskAdresseElement, adresse, 3);
                LeggTilAdresselinje(utenlandskAdresseElement, adresse, 4);

                if (adresse.Landkode != null)
                {
                    var landKode = utenlandskAdresseElement.AppendChildElement("landkode", "ns9", NavneromUtility.DifiSdpSchema10, Context);
                    landKode.InnerText = adresse.Landkode;
                }
                else
                {
                    var landKode = utenlandskAdresseElement.AppendChildElement("land", "ns9", NavneromUtility.DifiSdpSchema10, Context);
                    landKode.InnerText = adresse.Land;
                }
            }

            return utenlandskAdresseElement;
        }

        private XmlNode NorskAdresseNode(NorskAdresse adresse)
        {
            var norskAdresseElement = Context.CreateElement("ns9", "norskAdresse", NavneromUtility.DifiSdpSchema10);
            {
                LeggTilAdresselinje(norskAdresseElement, adresse, 1);
                LeggTilAdresselinje(norskAdresseElement, adresse, 2);
                LeggTilAdresselinje(norskAdresseElement, adresse, 3);

                var postnummer = norskAdresseElement.AppendChildElement("postnummer", "ns9", NavneromUtility.DifiSdpSchema10, Context);
                postnummer.InnerText = adresse.Postnummer;

                var poststed = norskAdresseElement.AppendChildElement("poststed", "ns9", NavneromUtility.DifiSdpSchema10, Context);
                poststed.InnerText = adresse.Poststed;
            }

            return norskAdresseElement;
        }

        private void LeggTilAdresselinje(XmlElement norskAdresse, Adresse adresse, int adresseLinjeNr)
        {
            var adresseLinjeData = adresse.AdresseLinje(adresseLinjeNr);

            if (string.IsNullOrEmpty(adresseLinjeData))
                return;

            var adresselinje = "adresselinje" + adresseLinjeNr;
            var linje = Context.CreateElement("ns9", adresselinje, NavneromUtility.DifiSdpSchema10);
            linje.InnerText = adresseLinjeData;

            norskAdresse.AppendChild(linje);
        }
    }
}