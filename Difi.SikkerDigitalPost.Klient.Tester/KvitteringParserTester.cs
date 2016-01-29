using Microsoft.VisualStudio.TestTools.UnitTesting;
using Difi.SikkerDigitalPost.Klient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using ApiClientShared;
using Difi.Felles.Utility.Utilities;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer.Forretning;

namespace Difi.SikkerDigitalPost.Klient.Tests
{
    [TestClass()]
    public class KvitteringParserTester
    {
        ResourceUtility ResourceUtility = new ResourceUtility("Difi.SikkerDigitalPost.Klient.Tester.Skjema.Eksempler.Kvitteringer");

        [TestClass]
        public class TilKvitteringMethod : KvitteringParserTester
        {
            [TestMethod]
            public void ParserLeveringskvittering()
            {
                //Arrange
                var xml = TilXmlDokument("Leveringskvittering.xml");
                const string konversjonsId = "716cffc1-58aa-4198-98df-281f4a1a1384";
                const string meldingsId = "5a93d7e9-e9e5-4013-ab19-c32d9eb0f3d0";
                const string referanseTilMeldingId = "03eafe0f-43ae-4184-82f6-ab194dd1b426";
                const string tidspunkt = "2015-11-10T08:37:24.695+01:00";

                //Act
                var leveringskvittering = KvitteringParser.TilLeveringskvittering(xml);

                //Assert
                Assert.AreEqual(konversjonsId, leveringskvittering.KonversasjonsId.ToString());
                Assert.AreEqual(meldingsId, leveringskvittering.MeldingsId);
                Assert.AreEqual(referanseTilMeldingId, leveringskvittering.ReferanseTilMeldingId);
                Assert.AreEqual(DateTime.Parse(tidspunkt), leveringskvittering.Levert);
            }

            [TestMethod]
            public void ParserMottakskvittering()
            {
                //Arrange
                var varslingFeiletKvittering = TilXmlDokument("Mottakskvittering.xml");


                //Act

                //Assert
            }

            [TestMethod]
            public void ParserReturpostkvittering()
            {
                //Arrange
                var xml = TilXmlDokument("Returpostkvittering.xml");

                //Act

                //Assert
            }

            [TestMethod]
            public void ParserVarslingFeiletKvittering()
            {
                //Arrange
                var xml = TilXmlDokument("VarslingFeiletKvittering.xml");

                //Act

                //Assert
            }

            private XmlDocument TilXmlDokument(string kvittering)
            {
                return XmlUtility.TilXmlDokument(Encoding.UTF8.GetString(ResourceUtility.ReadAllBytes(true, kvittering)));
            }

            [TestMethod]
            public void ParserÅpningskvittering()
            {
                //Arrange
                //XmlUtility.TilXmlDokument(ResourceUtility.ReadAllBytes(true, k));

                //Act

                //Assert
            }

        }

    }
}