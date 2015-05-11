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
using System.Diagnostics;
using System.Threading;
using ApiClientShared;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Aktører;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.FysiskPost;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer.Forretning;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer.Transport;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Post;
using Difi.SikkerDigitalPost.Klient.Domene.Enums;

namespace Difi.SikkerDigitalPost.Klient.Testklient
{
    class Program
    {
        private const string MpcId = "queue1";
        private const bool ErDigitalPostMottaker = true;
        private const bool ErNorskBrev = true;


        static void Main(string[] args)
        {
            SendPost();
        }

        private static void SendPost()
        {
            /*
             * I dette eksemplet er det Posten som er den som produserer informasjon/brev/post som skal formidles (Avsender),
             * Posten som er teknisk avsender, og det er Digipostkassen som skal motta meldingen. 
             */

            Console.WriteLine("--- STARTER Å SENDE POST ---");

            /*
             * SETT OPP MOTTAKER OG INNSTILLINGER
             */
            PostkasseInnstillinger postkasseInnstillinger = PostkasseInnstillinger.GetPosten();
            var postInfo = GenererPostInfo(postkasseInnstillinger, ErDigitalPostMottaker, ErNorskBrev);
            var avsender = new Avsender(postkasseInnstillinger.OrgNummerBehandlingsansvarlig);

            var databehandler = new Databehandler(postkasseInnstillinger.OrgNummerDatabehandler, "8702F5E55217EC88CF2CCBADAC290BB4312594AC");
            avsender.Avsenderidentifikator = "digipost";

            var forsendelse = GenererForsendelse(avsender, postInfo);
            var klientkonfigurasjon = SettOppKlientkonfigurasjon();
            var sikkerDigitalPostKlient = new SikkerDigitalPostKlient(databehandler, klientkonfigurasjon);


            /**
             * SEND POST OG MOTTA KVITTERINGER
             */
            SendPost(sikkerDigitalPostKlient, forsendelse);

            Console.WriteLine("--- STARTER Å HENTE KVITTERINGER ---");

            HentKvitteringer(sikkerDigitalPostKlient);

            Console.WriteLine();
            Console.WriteLine("--- FERDIG Å SENDE POST OG MOTTA KVITTERINGER :) --- ");
            Console.ReadKey();
        }

        private static void SendPost(SikkerDigitalPostKlient sikkerDigitalPostKlient, Forsendelse forsendelse)
        {
            Transportkvittering transportkvittering = sikkerDigitalPostKlient.Send(forsendelse);
            Console.WriteLine(" > Post sendt. Status er ...");

            if (transportkvittering.GetType() == typeof(TransportOkKvittering))
            {
                WriteToConsoleWithColor(" > OK! En transportkvittering ble hentet og alt gikk fint.");
            }

            if (transportkvittering.GetType() == typeof(TransportFeiletKvittering))
            {
                var feiletkvittering = (TransportFeiletKvittering)transportkvittering;
                WriteToConsoleWithColor(String.Format(" > {0}. Nå gikk det galt her. {1}", feiletkvittering.Alvorlighetsgrad,
                    feiletkvittering.Beskrivelse), true);
            }
        }

        private static void HentKvitteringer(SikkerDigitalPostKlient sikkerDigitalPostKlient)
        {
            Console.WriteLine();

            Console.WriteLine(" > Starter å hente kvitteringer ...");

            Thread.Sleep(3000);

            while (true)
            {
                var kvitteringsForespørsel = new Kvitteringsforespørsel(Prioritet.Prioritert, MpcId);
                Console.WriteLine(" > Henter kvittering på kø '{0}'...", kvitteringsForespørsel.Mpc);

                Kvittering kvittering = sikkerDigitalPostKlient.HentKvittering(kvitteringsForespørsel);

                if (kvittering == null)
                {
                    Console.WriteLine("  - Kø '{0}' er tom. Stopper å hente meldinger. ", kvitteringsForespørsel.Mpc);
                    break;
                }

                if (kvittering is TransportFeiletKvittering)
                {
                    var feil = ((TransportFeiletKvittering)kvittering).Beskrivelse;
                    WriteToConsoleWithColor(
                        "En feil skjedde under transport. Forespørsel for kvittering ble ikke godtatt av Meldingsformidler: " +
                        feil, isError: true);
                    break;
                }

                if (kvittering is Leveringskvittering)
                {
                    WriteToConsoleWithColor("  - En leveringskvittering ble hentet!");
                }

                if (kvittering is Åpningskvittering)
                {
                    WriteToConsoleWithColor("  - Har du sett. Noen har åpnet et brev. Moro.");
                }

                if (kvittering is Returpostkvittering)
                    WriteToConsoleWithColor("  - Du har fått en returpostkvittering for fysisk post.");

                if (kvittering is Mottakskvittering)
                    WriteToConsoleWithColor("  - Kvittering på sending av fysisk post mottatt.");

                if (kvittering is Feilmelding)
                {
                    var feil = (Feilmelding)kvittering;
                    WriteToConsoleWithColor("  - En feilmelding ble hentet :" + feil.Detaljer, true);
                }

                Console.WriteLine("  - Bekreftelse på mottatt kvittering sendes ...");
                sikkerDigitalPostKlient.Bekreft((Forretningskvittering)kvittering);
                Console.WriteLine("   - Kvittering sendt.");
            }
        }

        private static Klientkonfigurasjon SettOppKlientkonfigurasjon()
        {
            var klientkonfigurasjon = new Klientkonfigurasjon();
            LeggTilLogging(klientkonfigurasjon);
            klientkonfigurasjon.MeldingsformidlerUrl = new Uri("https://qaoffentlig.meldingsformidler.digipost.no/api/ebms");
            klientkonfigurasjon.LoggXmlTilFil = false;
            klientkonfigurasjon.StandardLoggSti = @"Z:\aleksander sjafjell On My Mac\Development\Shared\sdp-data\Logg";
            return klientkonfigurasjon;
        }

        private static Forsendelse GenererForsendelse(Avsender avsender, PostInfo postInfo)
        {
            ResourceUtility resourceUtility = new ResourceUtility("Difi.SikkerDigitalPost.Klient.Testklient.Resources");
            
            var hoveddokument = resourceUtility.ReadAllBytes(true, "Hoveddokument.pdf");
            var vedlegg = resourceUtility.ReadAllBytes(true, "Vedlegg.txt");
            
            //Forsendelse
            var dokumentpakke =
                new Dokumentpakke(new Dokument("Sendt" + DateTime.Now, hoveddokument, "application/pdf", "NO",
                    "OWASP TOP 10.pdf"));
            dokumentpakke.LeggTilVedlegg(new Dokument("Vedlegg", vedlegg, "plain/txt", "NO", "Vedlegg.txt"));
            return new Forsendelse(avsender, postInfo, dokumentpakke, Prioritet.Prioritert, MpcId, "NO");

        }

        private static void WriteToConsoleWithColor(string message, bool isError = false)
        {
            Console.ForegroundColor = isError ? ConsoleColor.Red : ConsoleColor.Green;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        private static void LeggTilLogging(Klientkonfigurasjon klientkonfigurasjon)
        {
            // Legger til logging til outputvinduet
            klientkonfigurasjon.Logger = (severity, konversasjonsId, metode, melding) =>
            {
                Debug.WriteLine("{0} - {1} [{2}]", DateTime.Now, melding, konversasjonsId.GetValueOrDefault());
            };
        }

        private static PostInfo GenererPostInfo(PostkasseInnstillinger postkasseInnstillinger, bool erDigitalPostMottaker, bool erNorskBrev)
        {
            PostInfo postInfo;
            PostMottaker mottaker;

            var mottakerSertifikatThumbprint = "B43CAAA0FBEE6C8DA85B47D1E5B7BCAB42AB9ADD";

            if (erDigitalPostMottaker)
            {
                mottaker = new DigitalPostMottaker(
                    personidentifikator: postkasseInnstillinger.Personnummer, 
                    postkasseadresse: postkasseInnstillinger.Postkasseadresse, 
                    sertifikatThumbprint: mottakerSertifikatThumbprint, 
                    organisasjonsnummerPostkasse: postkasseInnstillinger.OrgnummerPostkasse
                    );

                postInfo = new DigitalPostInfo((DigitalPostMottaker)mottaker, "Ikke-sensitiv tittel", Sikkerhetsnivå.Nivå3, åpningskvittering: false);
                ((DigitalPostInfo)postInfo).Virkningstidspunkt = DateTime.Now.AddMinutes(0);
            }
            else
            {
                Adresse adresse;
                if (erNorskBrev)
                    adresse = new NorskAdresse("0566", "Oslo");
                else
                    adresse = new UtenlandskAdresse("SE", "Saltkråkan 22");

                mottaker = new FysiskPostMottaker("Rolf Rolfsen", adresse,
                    mottakerSertifikatThumbprint, postkasseInnstillinger.OrgnummerPostkasse);

                var returMottaker = new FysiskPostMottaker("ReturKongen", new NorskAdresse("1533", "Søppeldynga"))
                {
                    Adresse = { Adresselinje1 = "Søppelveien 33" }
                };

                postInfo = new FysiskPostInfo(mottaker, Posttype.A, Utskriftsfarge.SortHvitt, Posthåndtering.MakuleringMedMelding, returMottaker);
            }
            return postInfo;
        }
    }
}
