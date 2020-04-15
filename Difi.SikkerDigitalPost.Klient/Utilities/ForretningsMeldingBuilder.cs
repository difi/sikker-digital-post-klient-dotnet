using System;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Aktører;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.FysiskPost;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Post;
using Difi.SikkerDigitalPost.Klient.SBDH;

namespace Difi.SikkerDigitalPost.Klient.Utilities
{
    public static class ForretningsMeldingBuilder
    {
        public static ForretningsMelding BuildForretningsMelding(Forsendelse forsendelse)
        {
            return forsendelse.PostInfo is DigitalPostInfo
                ? BuildDigitalForretningsMelding(forsendelse)
                : BuildFysiskForretningsMelding(forsendelse);
        }

        private static ForretningsMelding BuildDigitalForretningsMelding(Forsendelse forsendelse)
        {
            DigitalForretningsMelding digitalForretningsMelding = new DigitalForretningsMelding(
                forsendelse.Dokumentpakke.Hoveddokument.Tittel);

            DigitalPostInfo digitalPostInfo = forsendelse.PostInfo as DigitalPostInfo;
            
            if (digitalPostInfo == null)
            {
                throw new NullReferenceException("PostInfo må være en DigitalPostInfo");
            }
            
            digitalForretningsMelding.hoveddokument = forsendelse.Dokumentpakke.Hoveddokument.Filnavn;
            digitalForretningsMelding.digitalPostInfo = new DigitalPostInfo
            {
                Virkningstidspunkt = digitalPostInfo.Virkningstidspunkt,
                Åpningskvittering = digitalPostInfo.Åpningskvittering, 
            };
            
            //digitalForretningsMelding.digitalPostInfo = digitalPostInfo;
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
            digitaltVarsel.epostTekst = digitalPostInfo.EpostVarsel.Varslingstekst;
            digitaltVarsel.smsTekst = digitalPostInfo.SmsVarsel.Varslingstekst;
            
            digitalForretningsMelding.varsler = digitaltVarsel;

            foreach (Dokument dok in forsendelse.Dokumentpakke.Vedlegg)
            {
                //Do stuff with metadata
            }
            
            return digitalForretningsMelding;
        }

        private static ForretningsMelding BuildFysiskForretningsMelding(Forsendelse forsendelse)
        {
            FysiskForretningsMelding fysiskForretningsMelding = new FysiskForretningsMelding();

            FysiskPostInfo fysiskPostInfo = forsendelse.PostInfo as FysiskPostInfo;

            if (fysiskPostInfo == null)
            {
                throw new NullReferenceException("PostInfo må være en FysiskPostInfo");
            }
            
            fysiskForretningsMelding.hoveddokument = forsendelse.Dokumentpakke.Hoveddokument.Filnavn;

            FysiskPostMottaker fysiskPostMottaker = forsendelse.PostInfo.Mottaker as FysiskPostMottaker;

            if (fysiskPostMottaker == null)
            {
                throw new NullReferenceException("Motakker må være en FysiskPostMotakker");
            }
            
            fysiskForretningsMelding.mottakerAdresse = fysiskPostMottaker.Adresse;

            fysiskForretningsMelding.posttype = fysiskPostInfo.Posttype;

            fysiskForretningsMelding.returadresse = fysiskPostInfo.ReturpostMottaker.Adresse;

            fysiskForretningsMelding.returhaandtering = fysiskPostInfo.Posthåndtering;

            fysiskForretningsMelding.utskriftsfarge = fysiskPostInfo.Utskriftsfarge;
            
            return fysiskForretningsMelding;
        }
    }
}
