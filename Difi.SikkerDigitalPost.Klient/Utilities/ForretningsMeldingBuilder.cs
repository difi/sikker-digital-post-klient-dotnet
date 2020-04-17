using System;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.FysiskPost;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Post;
using Difi.SikkerDigitalPost.Klient.SBDH;

namespace Difi.SikkerDigitalPost.Klient.Utilities
{
    public static class ForretningsMeldingBuilder
    {
        public static ForretningsMelding BuildForretningsMelding(Forsendelse forsendelse)
        {
            var forretningsMelding = forsendelse.PostInfo is DigitalPostInfo
                ? BuildDigitalForretningsMelding(forsendelse)
                : BuildFysiskForretningsMelding(forsendelse);

            forretningsMelding.hoveddokument = forsendelse.Dokumentpakke.Hoveddokument.Filnavn;
            forretningsMelding.avsenderId = forsendelse.Avsender.Avsenderidentifikator == string.Empty ? null : forsendelse.Avsender.Avsenderidentifikator;
            forretningsMelding.fakturaReferanse = forsendelse.Avsender.Fakturareferanse;
            return forretningsMelding;
        }

        private static ForretningsMelding BuildDigitalForretningsMelding(Forsendelse forsendelse)
        {
            DigitalForretningsMelding digitalForretningsMelding =
                new DigitalForretningsMelding(forsendelse.Dokumentpakke.Hoveddokument.Tittel);

            if (!(forsendelse.PostInfo is DigitalPostInfo digitalPostInfo))
            {
                throw new NullReferenceException("PostInfo må være en DigitalPostInfo");
            }

            digitalForretningsMelding.digitalPostInfo = new DigitalPostInfo
            {
                Virkningstidspunkt = digitalPostInfo.Virkningstidspunkt,
                Åpningskvittering = digitalPostInfo.Åpningskvittering,
            };

            digitalForretningsMelding.tittel = digitalPostInfo.IkkeSensitivTittel;

            switch (forsendelse.Språkkode)
            {
                case "NO":
                    digitalForretningsMelding.spraak = Språk.NO;
                    break;
                case "NB":
                    digitalForretningsMelding.spraak = Språk.NB;
                    break;
                case "EN":
                    digitalForretningsMelding.spraak = Språk.EN;
                    break;
                default:
                    throw new Exception("Ugyldig språk kode! Velg en av NO, NB eller EN.");
            }

            digitalForretningsMelding.sikkerhetsnivaa = digitalPostInfo.Sikkerhetsnivå;

            DigitaltVarsel digitaltVarsel = new DigitaltVarsel();
            if (digitalPostInfo.EpostVarsel != null)
                digitaltVarsel.epostTekst = digitalPostInfo.EpostVarsel.Varslingstekst;
            if (digitalPostInfo.SmsVarsel != null) digitaltVarsel.smsTekst = digitalPostInfo.SmsVarsel.Varslingstekst;

            digitalForretningsMelding.varsler = digitaltVarsel;


            if (forsendelse.MetadataDocument != null)
            {
                digitalForretningsMelding.addMetadataMapping(forsendelse.Dokumentpakke.Hoveddokument.Filnavn,
                    forsendelse.MetadataDocument.Filnavn);
            }

            return digitalForretningsMelding;
        }

        private static ForretningsMelding BuildFysiskForretningsMelding(Forsendelse forsendelse)
        {
            FysiskForretningsMelding fysiskForretningsMelding = new FysiskForretningsMelding();

            if (!(forsendelse.PostInfo is FysiskPostInfo fysiskPostInfo))
            {
                throw new NullReferenceException("PostInfo må være en FysiskPostInfo");
            }
            if (!(forsendelse.PostInfo.Mottaker is FysiskPostMottaker fysiskPostMottaker))
            {
                throw new NullReferenceException("Motakker må være en FysiskPostMotakker");
            }

            fysiskForretningsMelding.mottaker = fysiskPostMottaker;

            fysiskForretningsMelding.posttype = fysiskPostInfo.Posttype;

            fysiskForretningsMelding.returadresse = fysiskPostInfo.ReturpostMottaker.Adresse;

            fysiskForretningsMelding.utskriftsfarge = fysiskPostInfo.Utskriftsfarge;

            fysiskForretningsMelding.retur = new Retur
            {
                mottaker = fysiskPostMottaker,
                returhaandtering = fysiskPostInfo.Posthåndtering
            };
            
            return fysiskForretningsMelding;
        }
    }
}
