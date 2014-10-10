using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Xml.Linq;
using SikkerDigitalPost.Domene.Entiteter.Aktører;
using SikkerDigitalPost.Domene.Entiteter.Kvitteringer;
using SikkerDigitalPost.Domene.Enums;
using SikkerDigitalPost.Klient;
using Timer = System.Timers.Timer;
using SikkerDigitalPost.Klient.Utilities;

namespace MFPuller
{
    class Program
    {
        static void Main(string[] args)
        {
            OnTimedEvent(null,null);

            Timer pullMessagesTimer = new Timer(600000); //Hvert 10. minutt.
            pullMessagesTimer.Elapsed += OnTimedEvent;
            pullMessagesTimer.Enabled = true;
            pullMessagesTimer.Start();

            while (true)
            {
                //keep running
                Thread.Sleep(15000);
            }
        }

        private static void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            var store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            store.Open(OpenFlags.ReadOnly);
            var tekniskAvsenderSertifikat = store.Certificates.Find(X509FindType.FindByThumbprint, "8702F5E55217EC88CF2CCBADAC290BB4312594AC", true)[0];
            store.Close();

            var organisasjonsnummerPosten = "984661185";

            var organisasjonsnummerTekniskAvsender = organisasjonsnummerPosten;
            var tekniskAvsender = new Databehandler(organisasjonsnummerTekniskAvsender, tekniskAvsenderSertifikat);

            var sikkerDigitalPostKlient = new SikkerDigitalPostKlient(databehandler: tekniskAvsender);

            var kvitteringsForespørsel = new Kvitteringsforespørsel(Prioritet.Prioritert);

            try
            {
                string textKvittering = sikkerDigitalPostKlient.HentKvittering(kvitteringsForespørsel);
                string fileName = String.Format("{0}.txt", DateTime.Now.ToString("s").Replace(":","."));
                SaveTextToFile(fileName,textKvittering);
            }
            catch (Exception error)
            {
                SaveTextToFile("Feilmelding.txt", String.Format("{0}, {1}", error.Message, error.InnerException));
            }
        }

        private static void SaveTextToFile(string filename, string text)
        {
            XDocument doc = XDocument.Parse(text);
            string path = Path.Combine("kvitteringer", filename);
            FileUtility.AppendToFileInBasePath(path,text);
            
            File.AppendAllText(path, doc.ToString());
        }
    }
}
