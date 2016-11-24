using System;
using Difi.SikkerDigitalPost.Klient.Domene.Enums;
using Difi.SikkerDigitalPost.Klient.Tester.Utilities;
using Xunit;

namespace Difi.SikkerDigitalPost.Klient.Tester
{
    public class KvitteringsparserTester
    {
        public class TilKvitteringMethod : KvitteringsparserTester
        {
            [Fact]
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
                Assert.Equal(konversasjonsId, leveringskvittering.KonversasjonsId.ToString());
                Assert.Equal(meldingsId, leveringskvittering.MeldingsId);
                Assert.Equal(referanseTilMeldingId, leveringskvittering.ReferanseTilMeldingId);
                Assert.Equal(DateTime.Parse(tidspunkt), leveringskvittering.Levert);
            }

            [Fact]
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
                Assert.Equal(konversasjonsId, mottakskvittering.KonversasjonsId.ToString());
                Assert.Equal(meldingsId, mottakskvittering.MeldingsId);
                Assert.Equal(referanseTilMeldingId, mottakskvittering.ReferanseTilMeldingId);
                Assert.Equal(DateTime.Parse(tidspunkt), mottakskvittering.Mottatt);
            }

            [Fact]
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
                Assert.Equal(konversasjonsId, returpostkvittering.KonversasjonsId.ToString());
                Assert.Equal(meldingsId, returpostkvittering.MeldingsId);
                Assert.Equal(referanseTilMeldingId, returpostkvittering.ReferanseTilMeldingId);
                Assert.Equal(DateTime.Parse(tidspunkt), returpostkvittering.Returnert);
                Assert.Equal(xml, returpostkvittering.Xml);
            }

            [Fact]
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
                Assert.Equal(konversasjonsId, varslingfeiletkvittering.KonversasjonsId.ToString());
                Assert.Equal(meldingsId, varslingfeiletkvittering.MeldingsId);
                Assert.Equal(referanseTilMeldingId, varslingfeiletkvittering.ReferanseTilMeldingId);
                Assert.Equal(DateTime.Parse(tidspunkt), varslingfeiletkvittering.Feilet);
                Assert.Equal(beskrivelse, varslingfeiletkvittering.Beskrivelse);
                Assert.Equal(varslingskanal, varslingfeiletkvittering.Varslingskanal);
                Assert.Equal(xml, varslingfeiletkvittering.Xml);
            }

            [Fact]
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
                Assert.Equal(konversasjonsId, åpningskvittering.KonversasjonsId.ToString());
                Assert.Equal(meldingsId, åpningskvittering.MeldingsId);
                Assert.Equal(referanseTilMeldingId, åpningskvittering.ReferanseTilMeldingId);
                Assert.Equal(DateTime.Parse(tidspunkt), åpningskvittering.Åpnet);
                Assert.Equal(xml, åpningskvittering.Xml);
            }

            [Fact]
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
                Assert.Equal(konversasjonsId, feilmelding.KonversasjonsId.ToString());
                Assert.Equal(meldingsId, feilmelding.MeldingsId);
                Assert.Equal(referanseTilMeldingId, feilmelding.ReferanseTilMeldingId);
                Assert.Equal(DateTime.Parse(tidspunkt), feilmelding.Feilet);
                Assert.Equal(detaljer, feilmelding.Detaljer);
                Assert.Equal(feiltype, feilmelding.Skyldig);
                Assert.Equal(xml, feilmelding.Xml);
            }

            [Fact]
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
                Assert.Equal(DateTime.Parse(tidspunkt), tomKøKvittering.SendtTidspunkt);
                Assert.Equal(meldingsId, tomKøKvittering.MeldingsId);
                Assert.Equal(referanseTilMeldingId, tomKøKvittering.ReferanseTilMeldingId);
                Assert.Equal(xml, tomKøKvittering.Xml);
            }

            [Fact]
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
                Assert.Equal(alvorlighetsgrad, transportFeiletKvittering.Alvorlighetsgrad);
                Assert.Equal(beskrivelse, transportFeiletKvittering.Beskrivelse);
                Assert.Equal(feilkode, transportFeiletKvittering.Feilkode);
                Assert.Equal(kategori, transportFeiletKvittering.Kategori);
                Assert.Equal(meldingsId, transportFeiletKvittering.MeldingsId);
                Assert.Equal(opprinnelse, transportFeiletKvittering.Opprinnelse);
                Assert.Equal(referanseTilMeldingId, transportFeiletKvittering.ReferanseTilMeldingId);
                Assert.Equal(DateTime.Parse(sendtTidspunkt), transportFeiletKvittering.SendtTidspunkt);
                Assert.Equal(skyldig, transportFeiletKvittering.Skyldig);
                Assert.Equal(xml, transportFeiletKvittering.Xml);
            }

            [Fact]
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
                Assert.Equal(DateTime.Parse(tidspunkt), transportOkKvittering.SendtTidspunkt);
                Assert.Equal(meldingsId, transportOkKvittering.MeldingsId);
                Assert.Equal(referanseTilMeldingId, transportOkKvittering.ReferanseTilMeldingId);
                Assert.Equal(xml, transportOkKvittering.Xml);
            }
        }
    }
}