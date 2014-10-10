using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using SikkerDigitalPost.Domene.Entiteter.Aktører;
using SikkerDigitalPost.Domene.Entiteter.Kvitteringer;
using SikkerDigitalPost.Domene.Enums;
using SikkerDigitalPost.Klient;

namespace MFPuller
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {

            }



        }

        private static void Pull()
        {

            X509Store store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            store.Open(OpenFlags.ReadOnly);
            var tekniskAvsenderSertifikat = store.Certificates.Find(X509FindType.FindByThumbprint, "8702F5E55217EC88CF2CCBADAC290BB4312594AC", true)[0];
            store.Close();

            var organisasjonsnummerPosten = "984661185";

            var organisasjonsnummerTekniskAvsender = organisasjonsnummerPosten;
            var tekniskAvsender = new Databehandler(organisasjonsnummerTekniskAvsender, tekniskAvsenderSertifikat);

            SikkerDigitalPostKlient sikkerDigitalPostKlient = new SikkerDigitalPostKlient(tekniskAvsender);

            var kvitteringsForespørsel = new Kvitteringsforespørsel(Prioritet.Prioritert);
            Forretningskvittering v = sikkerDigitalPostKlient.HentKvittering(kvitteringsForespørsel);

        }
    }
}
