using System;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Post;
using Difi.SikkerDigitalPost.Klient.Domene.Enums;
using Difi.SikkerDigitalPost.Klient.Tester.Utilities;
using Xunit;

namespace Difi.SikkerDigitalPost.Klient.Tester.Entiteter.Post
{
    public class ForsendelseTester
    {
        public class KonstruktørMethod : ForsendelseTester
        {
            [Fact]
            public void EnkelKonstruktør()
            {
                //Arrange
                var forsendelse = new Forsendelse(
                    DomainUtility.GetAvsender(),
                    DomainUtility.GetDigitalPostInfoSimple(),
                    DomainUtility.GetDokumentpakkeWithoutAttachments()
                    );

                //Act

                //Assert
                Assert.NotNull(forsendelse.Avsender);
                Assert.NotNull(forsendelse.PostInfo);
                Assert.NotNull(forsendelse.Dokumentpakke);
            }

            [Fact]
            public void KonstruktørMedOptionalParametere()
            {
                //Arrange
                var prioritet = Prioritet.Normal;
                var mpcId = "mpcId";
                var språkkode = "NO";

                var forsendelse = new Forsendelse(
                    DomainUtility.GetAvsender(),
                    DomainUtility.GetDigitalPostInfoSimple(),
                    DomainUtility.GetDokumentpakkeWithoutAttachments(),
                    prioritet,
                    mpcId,
                    språkkode
                    );

                //Act

                //Assert
                Assert.NotNull(forsendelse.Avsender);
                Assert.NotNull(forsendelse.PostInfo);
                Assert.NotNull(forsendelse.Dokumentpakke);
                Assert.Equal(prioritet, forsendelse.Prioritet);
                Assert.Equal(mpcId, forsendelse.MpcId);
                Assert.Equal(språkkode, forsendelse.Språkkode);
            }

            [Fact]
            public void KonstruktørForIdentiskHash()
            {
                //Arrange
                var prioritet = Prioritet.Normal;
                var mpcId = "mpcId";
                var språkkode = "NO";

                var konversasjonsid = Guid.NewGuid();
                var forsendelse = new Forsendelse(
                    DomainUtility.GetAvsender(),
                    DomainUtility.GetDigitalPostInfoSimple(),
                    DomainUtility.GetDokumentpakkeWithoutAttachments(),
                    konversasjonsid,
                    prioritet,
                    mpcId,
                    språkkode);

                //Act

                //Assert

                Assert.Equal(konversasjonsid, forsendelse.KonversasjonsId);
                Assert.NotNull(forsendelse.Avsender);
                Assert.NotNull(forsendelse.PostInfo);
                Assert.NotNull(forsendelse.Dokumentpakke);
                Assert.Equal(prioritet, forsendelse.Prioritet);
                Assert.Equal(mpcId, forsendelse.MpcId);
                Assert.Equal(språkkode, forsendelse.Språkkode);
            }
        }
    }
}