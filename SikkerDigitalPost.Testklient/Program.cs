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
using SikkerDigitalPost.Domene.Entiteter;
using SikkerDigitalPost.Domene.Entiteter.Aktører;
using SikkerDigitalPost.Domene.Entiteter.Kvitteringer;
using SikkerDigitalPost.Domene.Entiteter.Post;
using SikkerDigitalPost.Domene.Enums;
using SikkerDigitalPost.Klient;
using SikkerDigitalPost.Klient.Utilities;

namespace SikkerDigitalPost.Testklient
{
    class Program
    {
        static void Main(string[] args)
        {
            SendPost();
        }

        private static bool SendPost()
        {
            bool isSent = true;
            /*
             * I dette eksemplet er det Posten som er den som produserer informasjon/brev/post som skal formidles (Behandlingsansvarlig),
             * Posten som er teknisk avsender, og det er Digipostkassen som skal motta meldingen. 
             */
            PostkasseInnstillinger postkasseInnstillinger = PostkasseInnstillinger.GetPosten();

            //Avsender
            var behandlingsansvarlig =
                new Behandlingsansvarlig(new Organisasjonsnummer(postkasseInnstillinger.OrgNummerBehandlingsansvarlig));
            var tekniskAvsender = new Databehandler(postkasseInnstillinger.OrgNummerDatabehandler,
                postkasseInnstillinger.Avsendersertifikat);
            behandlingsansvarlig.Avsenderidentifikator = "digipost";

            //Mottaker
            var mottaker = new Mottaker(postkasseInnstillinger.Personnummer, postkasseInnstillinger.Postkasseadresse,
                postkasseInnstillinger.Mottakersertifikat, postkasseInnstillinger.OrgnummerPostkasse);

            //Digital Post
            var digitalPost = new DigitalPost(mottaker, "Ikke-sensitiv tittel", Sikkerhetsnivå.Nivå3, åpningskvittering: false);
            digitalPost.Virkningstidspunkt = DateTime.Now.AddMinutes(1);

            string hoveddokumentsti =
                @"Z:\aleksander sjafjell On My Mac\Development\Shared\sdp-data\testdata\hoveddokument\Hoveddokument.txt";
            string vedleggsti = @"Z:\aleksander sjafjell On My Mac\Development\Shared\sdp-data\testdata\vedlegg\Vedlegg.txt";

            //Forsendelse
            string mpcId = "hest";
            var dokumentpakke = new Dokumentpakke(new Dokument("Sendt" + DateTime.Now, hoveddokumentsti, "text/plain", "NO", "Hoveddokument.txt"));
            dokumentpakke.LeggTilVedlegg(new Dokument("Vedlegg", vedleggsti, "text/plain", "NO", "Vedlegg.txt"));
            var forsendelse = new Forsendelse(behandlingsansvarlig, digitalPost, dokumentpakke, Prioritet.Prioritert, mpcId, "NO");

            //Send
            var klientkonfigurasjon = new Klientkonfigurasjon();
            LeggTilLogging(klientkonfigurasjon);
            klientkonfigurasjon.MeldingsformidlerUrl = new Uri("https://qaoffentlig.meldingsformidler.digipost.no/api/ebms");
            klientkonfigurasjon.DebugLoggTilFil = true;
            klientkonfigurasjon.StandardLoggSti = @"Z:\aleksander sjafjell On My Mac\Development\Shared\sdp-data\Logg";

            var sikkerDigitalPostKlient = new SikkerDigitalPostKlient(tekniskAvsender, klientkonfigurasjon);

            Console.WriteLine("--- STARTER Å SENDE POST ---");

            Transportkvittering transportkvittering = sikkerDigitalPostKlient.Send(forsendelse);
            Console.WriteLine(" > Post sendt. Status er ...");

            if (transportkvittering.GetType() == typeof(TransportOkKvittering))
            {
                WriteToConsoleWithColor(" > OK! En transportkvittering ble hentet og alt gikk fint.");
            }

            if (transportkvittering.GetType() == typeof(TransportFeiletKvittering))
            {
                var feiletkvittering = (TransportFeiletKvittering)transportkvittering;
                Console.WriteLine(" > {0}. Nå gikk det galt her. {1}", feiletkvittering.Alvorlighetsgrad,
                    feiletkvittering.Beskrivelse);
            }

            Console.WriteLine();
            Console.WriteLine("--- STARTER Å HENTE KVITTERINGER ---");

            Console.WriteLine(" > Starter å hente kvitteringer ...");

            Thread.Sleep(3000);

            while (true)
            {
                var kvitteringsForespørsel = new Kvitteringsforespørsel(Prioritet.Prioritert, mpcId);
                Console.WriteLine(" > Henter kvittering på kø '{0}'...", kvitteringsForespørsel.Mpc);

                Forretningskvittering kvittering = sikkerDigitalPostKlient.HentKvittering(kvitteringsForespørsel);

                if (kvittering == null)
                {
                    Console.WriteLine("  - Kø '{0}' er tom. Stopper å hente meldinger. ", kvitteringsForespørsel.Mpc);
                    break;
                }

                if (kvittering.GetType() == typeof(Leveringskvittering))
                {
                    WriteToConsoleWithColor("  - En leveringskvittering ble hentet!");
                }

                if (kvittering.GetType() == typeof(Åpningskvittering))
                {
                    WriteToConsoleWithColor("  - Har du sett. Noen har åpnet et brev. Moro.");
                }

                if (kvittering.GetType() == typeof(Feilmelding))
                {
                    WriteToConsoleWithColor("  - En feilmelding ble hentet, men den kan være gammel ...", true);
                    isSent = false;
                }

                Console.WriteLine("  - Bekreftelse på mottatt kvittering sendes ...");
                sikkerDigitalPostKlient.Bekreft(kvittering);
                Console.WriteLine("   - Kvittering sendt.");
            }


            Console.WriteLine();
            Console.WriteLine("--- FERDIG Å SENDE POST OG MOTTA KVITTERINGER :) --- ");
            Console.ReadKey();
            return isSent;
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
    }
}






