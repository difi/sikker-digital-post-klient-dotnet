using System;
using Difi.SikkerDigitalPost.Klient.Domene.Enums;
using Difi.SikkerDigitalPost.Klient.Tester.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Difi.SikkerDigitalPost.Klient.Tester
{
    [TestClass]
    public class KvitteringsparserTester
    {
        [TestClass]
        public class TilKvitteringMethod : KvitteringsparserTester
        {
            [TestMethod]
            public void ParserLeveringskvittering()
            {
                //Arrange
                var xml = KvitteringsUtility.Forretningskvittering.LeveringskvitteringXml();
                const string konversasjonsId = "716cffc1-58aa-4198-98df-281f4a1a1384";
                const string meldingsId = "5a93d7e9-e9e5-4013-ab19-c32d9eb0f3d0";
                const string referanseTilMeldingId = "03eafe0f-43ae-4184-82f6-ab194dd1b426";
                const string tidspunkt = "2015-11-10T08:37:24.695+01:00";

                //Act
                var leveringskvittering = Kvitteringsparser.TilLeveringskvittering(xml);

                //Assert
                Assert.AreEqual(konversasjonsId, leveringskvittering.KonversasjonsId.ToString());
                Assert.AreEqual(meldingsId, leveringskvittering.MeldingsId);
                Assert.AreEqual(referanseTilMeldingId, leveringskvittering.ReferanseTilMeldingId);
                Assert.AreEqual(DateTime.Parse(tidspunkt), leveringskvittering.Levert);
            }

            [TestMethod]
            public void ParserMottakskvittering()
            {
                //Arrange
                var xml = KvitteringsUtility.Forretningskvittering.MottakskvitteringXml();
                const string konversasjonsId = "2049057a-9b53-41bb-9cc3-d10f55fa0f87";
                const string meldingsId = "7142d8ab-9408-4cb5-8b80-dca3618dd722";
                const string referanseTilMeldingId = "312034c8-c63a-46ac-8eec-bc22d0e534d8";
                const string tidspunkt = "2015-11-10T08:26:49.797+01:00";

                //Act
                var mottakskvittering = Kvitteringsparser.TilMottakskvittering(xml);

                //Assert
                Assert.AreEqual(konversasjonsId, mottakskvittering.KonversasjonsId.ToString());
                Assert.AreEqual(meldingsId, mottakskvittering.MeldingsId);
                Assert.AreEqual(referanseTilMeldingId, mottakskvittering.ReferanseTilMeldingId);
                Assert.AreEqual(DateTime.Parse(tidspunkt), mottakskvittering.Mottatt);
            }

            [TestMethod]
            public void ParserReturpostkvittering()
            {
                //Arrange
                var xml = KvitteringsUtility.Forretningskvittering.ReturpostkvitteringXml();
                const string konversasjonsId = "2049057a-9b53-41bb-9cc3-d10f55fa0f87";
                const string meldingsId = "7142d8ab-9408-4cb5-8b80-dca3618dd722";
                const string referanseTilMeldingId = "312034c8-c63a-46ac-8eec-bc22d0e534d8";
                const string tidspunkt = "2015-11-10T08:26:49.797+01:00";

                //Act
                var returpostkvittering = Kvitteringsparser.TilReturpostkvittering(xml);

                //Assert
                Assert.AreEqual(konversasjonsId, returpostkvittering.KonversasjonsId.ToString());
                Assert.AreEqual(meldingsId, returpostkvittering.MeldingsId);
                Assert.AreEqual(referanseTilMeldingId, returpostkvittering.ReferanseTilMeldingId);
                Assert.AreEqual(DateTime.Parse(tidspunkt), returpostkvittering.Returnert);
                Assert.AreEqual(xml.OuterXml, returpostkvittering.Rådata);
            }

            [TestMethod]
            public void ParserVarslingFeiletKvittering()
            {
                //Arrange
                var xml = KvitteringsUtility.Forretningskvittering.VarslingFeiletKvitteringXml();
                const string konversasjonsId = "2049057a-9b53-41bb-9cc3-d10f55fa0f87";
                const string meldingsId = "7142d8ab-9408-4cb5-8b80-dca3618dd722";
                const string referanseTilMeldingId = "312034c8-c63a-46ac-8eec-bc22d0e534d8";
                const string tidspunkt = "2015-11-10T08:26:49.797+01:00";
                const string beskrivelse = "Selvvalgt";
                const Varslingskanal varslingskanal = Varslingskanal.Sms;

                //Act
                var varslingfeiletkvittering = Kvitteringsparser.TilVarslingFeiletKvittering(xml);

                //Assert
                Assert.AreEqual(konversasjonsId, varslingfeiletkvittering.KonversasjonsId.ToString());
                Assert.AreEqual(meldingsId, varslingfeiletkvittering.MeldingsId);
                Assert.AreEqual(referanseTilMeldingId, varslingfeiletkvittering.ReferanseTilMeldingId);
                Assert.AreEqual(DateTime.Parse(tidspunkt), varslingfeiletkvittering.Feilet);
                Assert.AreEqual(beskrivelse, varslingfeiletkvittering.Beskrivelse);
                Assert.AreEqual(varslingskanal, varslingfeiletkvittering.Varslingskanal);
                Assert.AreEqual(xml.OuterXml, varslingfeiletkvittering.Rådata);
            }

            [TestMethod]
            public void ParserÅpningskvittering()
            {
                //Arrange
                var xml = KvitteringsUtility.Forretningskvittering.ÅpningskvitteringXml();
                const string konversasjonsId = "1d4aff36-b6d2-4506-bc0b-bd62ae6f8966";
                const string meldingsId = "2d476cb1-cf9a-4210-ba74-ee095f41c9f2";
                const string referanseTilMeldingId = "b32d2b7c-2c88-456d-9d74-de348d7c30f8";
                const string tidspunkt = "2015-11-09T16:11:31.171+01:00";

                //Act
                var åpningskvittering = Kvitteringsparser.TilÅpningskvittering(xml);

                //Assert
                Assert.AreEqual(konversasjonsId, åpningskvittering.KonversasjonsId.ToString());
                Assert.AreEqual(meldingsId, åpningskvittering.MeldingsId);
                Assert.AreEqual(referanseTilMeldingId, åpningskvittering.ReferanseTilMeldingId);
                Assert.AreEqual(DateTime.Parse(tidspunkt), åpningskvittering.Åpnet);
                Assert.AreEqual(xml.OuterXml, åpningskvittering.Rådata);
            }

            [TestMethod]
            public void ParserFeilmelding()
            {
                //Arrange
                var xml = KvitteringsUtility.Forretningskvittering.FeilmeldingXml();
                const string konversasjonsId = "2049057a-9b53-41bb-9cc3-d10f55fa0f87";
                const string meldingsId = "7142d8ab-9408-4cb5-8b80-dca3618dd722";
                const string referanseTilMeldingId = "312034c8-c63a-46ac-8eec-bc22d0e534d8";
                const string tidspunkt = "2015-11-10T08:26:49.797+01:00";
                const string detaljer = "detaljer";
                const Feiltype feiltype = Feiltype.Server;

                //Act
                var feilmelding = Kvitteringsparser.TilFeilmelding(xml);

                //Assert
                Assert.AreEqual(konversasjonsId, feilmelding.KonversasjonsId.ToString());
                Assert.AreEqual(meldingsId, feilmelding.MeldingsId);
                Assert.AreEqual(referanseTilMeldingId, feilmelding.ReferanseTilMeldingId);
                Assert.AreEqual(DateTime.Parse(tidspunkt), feilmelding.Feilet);
                Assert.AreEqual(detaljer, feilmelding.Detaljer);
                Assert.AreEqual(feiltype, feilmelding.Skyldig);
                Assert.AreEqual(xml.OuterXml, feilmelding.Rådata);
            }

            [TestMethod]
            public void ParserTomKøKvittering()
            {
                //Arrange
                var xml = KvitteringsUtility.Transportkvittering.TomKøKvitteringXml();
                const string tidspunkt = "2015-11-10T12:23:05.792+01:00";
                const string meldingsId = "b468901b-4c8d-4a8f-a10e-be4f8c8f9d69";
                const string referanseTilMeldingId = "0e38fc67-0fac-45dd-b9c2-3e2ff703a656";

                //Act
                var tomKøKvittering = Kvitteringsparser.TilTomKøKvittering(xml);

                //Assert
                Assert.AreEqual(DateTime.Parse(tidspunkt), tomKøKvittering.SendtTidspunkt);
                Assert.AreEqual(meldingsId, tomKøKvittering.MeldingsId);
                Assert.AreEqual(referanseTilMeldingId, tomKøKvittering.ReferanseTilMeldingId);
                Assert.AreEqual(xml.OuterXml, tomKøKvittering.Rådata);
            }

            [TestMethod]
            public void ParserTransportFeiletKvittering()
            {
                //Arrange
                var xml = KvitteringsUtility.Transportkvittering.TransportFeiletKvitteringXml();
                const string alvorlighetsgrad = "failure";
                const string beskrivelse = "Invalid timestamp: The security semantics of the message have expired; nested exception is org.apache.wss4j.common.ext.WSSecurityException: Invalid timestamp: The security semantics of the message have expired";
                const string feilkode = "EBMS:0103";
                const string kategori = "Processing";
                const string meldingsId = "e0df4e6c-c4d7-426b-a3fd-dac2e241f313";
                const string opprinnelse = "security";
                object referanseTilMeldingId = null;
                const string sendtTidspunkt = "2015-11-10T14:58:23.408+01:00";
                var skyldig = Feiltype.Klient;

                //Act
                var transportFeiletKvittering = Kvitteringsparser.TilTransportFeiletKvittering(xml);

                //Assert
                Assert.AreEqual(alvorlighetsgrad, transportFeiletKvittering.Alvorlighetsgrad);
                Assert.AreEqual(beskrivelse, transportFeiletKvittering.Beskrivelse);
                Assert.AreEqual(feilkode, transportFeiletKvittering.Feilkode);
                Assert.AreEqual(kategori, transportFeiletKvittering.Kategori);
                Assert.AreEqual(meldingsId, transportFeiletKvittering.MeldingsId);
                Assert.AreEqual(opprinnelse, transportFeiletKvittering.Opprinnelse);
                Assert.AreEqual(referanseTilMeldingId, transportFeiletKvittering.ReferanseTilMeldingId);
                Assert.AreEqual(DateTime.Parse(sendtTidspunkt), transportFeiletKvittering.SendtTidspunkt);
                Assert.AreEqual(skyldig, transportFeiletKvittering.Skyldig);
                Assert.AreEqual(xml.OuterXml, transportFeiletKvittering.Rådata);
            }

            [TestMethod]
            public void ParserTransportOkKvittering()
            {
                //Arrange
                var xml = KvitteringsUtility.Transportkvittering.TransportOkKvitteringXml();
                const string tidspunkt = "2015-11-10T15:02:39.476+01:00";
                const string meldingsId = "dd1f3d46-f79a-4152-bf05-4b3ceef3ce67";
                const string referanseTilMeldingId = "8d70d61c-d15d-4d59-b0fe-17e13fca8ccb";

                //Act
                var transportOkKvittering = Kvitteringsparser.TilTransportOkKvittering(xml);

                //Assert
                Assert.AreEqual(DateTime.Parse(tidspunkt), transportOkKvittering.SendtTidspunkt);
                Assert.AreEqual(meldingsId, transportOkKvittering.MeldingsId);
                Assert.AreEqual(referanseTilMeldingId, transportOkKvittering.ReferanseTilMeldingId);
                Assert.AreEqual(xml.OuterXml, transportOkKvittering.Rådata);
            }
        }
    }
}