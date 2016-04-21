using System;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Post;
using Difi.SikkerDigitalPost.Klient.Domene.Enums;
using Difi.SikkerDigitalPost.Klient.Tester.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Difi.SikkerDigitalPost.Klient.Tester.Entiteter.Post
{
    [TestClass]
    public class ForsendelseTester
    {
        [TestClass]
        public class KonstruktørMethod : ForsendelseTester
        {
            [TestMethod]
            public void EnkelKonstruktør()
            {
                //Arrange
                var forsendelse = new Forsendelse(
                    DomeneUtility.GetAvsender(),
                    DomeneUtility.GetDigitalPostInfoEnkel(),
                    DomeneUtility.GetDokumentpakkeUtenVedlegg()
                    );

                //Act

                //Assert
                Assert.IsNotNull(forsendelse.Avsender);
                Assert.IsNotNull(forsendelse.PostInfo);
                Assert.IsNotNull(forsendelse.Dokumentpakke);
            }

            [TestMethod]
            public void KonstruktørMedOptionalParametere()
            {
                //Arrange
                var prioritet = Prioritet.Normal;
                var mpcId = "mpcId";
                var språkkode = "NO";

                var forsendelse = new Forsendelse(
                    DomeneUtility.GetAvsender(),
                    DomeneUtility.GetDigitalPostInfoEnkel(),
                    DomeneUtility.GetDokumentpakkeUtenVedlegg(),
                    prioritet,
                    mpcId,
                    språkkode
                    );

                //Act

                //Assert
                Assert.IsNotNull(forsendelse.Avsender);
                Assert.IsNotNull(forsendelse.PostInfo);
                Assert.IsNotNull(forsendelse.Dokumentpakke);
                Assert.AreEqual(prioritet, forsendelse.Prioritet);
                Assert.AreEqual(mpcId, forsendelse.MpcId);
                Assert.AreEqual(språkkode, forsendelse.Språkkode);
            }

            [TestMethod]
            public void KonstruktørForIdentiskHash()
            {
                //Arrange
                var prioritet = Prioritet.Normal;
                var mpcId = "mpcId";
                var språkkode = "NO";

                var konversasjonsid = Guid.NewGuid();
                var forsendelse = new Forsendelse(
                    DomeneUtility.GetAvsender(),
                    DomeneUtility.GetDigitalPostInfoEnkel(),
                    DomeneUtility.GetDokumentpakkeUtenVedlegg(),
                    konversasjonsid,
                    prioritet,
                    mpcId,
                    språkkode);

                //Act

                //Assert

                Assert.AreEqual(konversasjonsid, forsendelse.KonversasjonsId);
                Assert.IsNotNull(forsendelse.Avsender);
                Assert.IsNotNull(forsendelse.PostInfo);
                Assert.IsNotNull(forsendelse.Dokumentpakke);
                Assert.AreEqual(prioritet, forsendelse.Prioritet);
                Assert.AreEqual(mpcId, forsendelse.MpcId);
                Assert.AreEqual(språkkode, forsendelse.Språkkode);
            }
        }
    }
}