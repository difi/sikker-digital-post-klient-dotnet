using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using KontaktregisteretGateway;
using KontaktregisteretGateway.Difi;
using SikkerDigitalPost.Domene.Entiteter;
using SikkerDigitalPost.Domene.Entiteter.Aktører;
using Person = SikkerDigitalPost.Domene.Entiteter.Aktører.Person;

namespace SikkerDigitalPost.Testklient
{
    public static class Kontaktregisteret
    {
        public static IEnumerable<Mottaker> HentPersoner(IEnumerable<string> fødselsnummer)
        {
            var service = new X509Certificate2(@"../../../Kontaktregisteretsertifikater/idporten-ver2.difi.no-v2.crt"
                , "changeit");
            var client = new X509Certificate2(@"../../../Kontaktregisteretsertifikater/WcfClient.pfx", "changeit");
            var settings = new DifiGatewaySettings(client, service);

            var _kontaktregisteretGateway = new KontaktregisteretGateway.KontaktregisteretGateway(settings);
            
            var request = new HentPersonerForespoersel();
            request.informasjonsbehov = new informasjonsbehov[4];
            request.informasjonsbehov[0] = informasjonsbehov.Kontaktinfo;
            request.informasjonsbehov[1] = informasjonsbehov.Person;
            request.informasjonsbehov[2] = informasjonsbehov.Sertifikat;
            request.informasjonsbehov[3] = informasjonsbehov.SikkerDigitalPost;

            request.personidentifikator = fødselsnummer.ToArray();

            var personer = _kontaktregisteretGateway.HentPersoner(request);

            var digipostPersoner = new List<Mottaker>();

            foreach (KontaktregisteretGateway.Difi.Person difiPerson in personer)
            {
                var orgnr = new Organisasjonsnummer(difiPerson.SikkerDigitalPostAdresse.postkasseleverandoerAdresse);
                var sertifikat = new X509Certificate2(difiPerson.X509Sertifikat);
                Mottaker mottaker = new Mottaker(difiPerson.personidentifikator, difiPerson.SikkerDigitalPostAdresse.postkasseadresse, sertifikat, orgnr );

                digipostPersoner.Add(mottaker);
            }

            return digipostPersoner;
        }
    }
}
