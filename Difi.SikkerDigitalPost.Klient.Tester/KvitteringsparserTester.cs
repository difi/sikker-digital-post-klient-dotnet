using System;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer;
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
                const string referanseTilMeldingId = "716cffc1-58aa-4198-98df-281f4a1a1384";
                const string tidspunkt = "2015-11-10T08:37:24.695+01:00";

                IntegrasjonspunktKvittering kvittering = new IntegrasjonspunktKvittering(
                    1L, 
                    DateTime.Parse(tidspunkt), 
                    IntegrasjonspunktKvitteringType.LEVERT, 
                    "", 
                    xml.InnerXml, 
                    Guid.Parse(konversasjonsId), 
                    Guid.Parse(meldingsId), 
                    1L);
                
                //Act
                var leveringskvittering = Kvitteringsparser.TilLeveringskvittering(kvittering);

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
                const string referanseTilMeldingId = "2049057a-9b53-41bb-9cc3-d10f55fa0f87";
                const string tidspunkt = "2015-11-10T08:26:49.797+01:00";

                IntegrasjonspunktKvittering kvittering = new IntegrasjonspunktKvittering(
                    1L, 
                    DateTime.Parse(tidspunkt), 
                    IntegrasjonspunktKvitteringType.MOTTATT, 
                    "", 
                    xml.InnerXml, 
                    Guid.Parse(konversasjonsId), 
                    Guid.Parse(meldingsId), 
                    1L);
                
                //Act
                var mottakskvittering = Kvitteringsparser.TilMottakskvittering(kvittering);

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
                const string referanseTilMeldingId = "2049057a-9b53-41bb-9cc3-d10f55fa0f87";
                const string tidspunkt = "2015-11-10T08:26:49.797+01:00";

                IntegrasjonspunktKvittering kvittering = new IntegrasjonspunktKvittering(
                    1L, 
                    DateTime.Parse(tidspunkt), 
                    IntegrasjonspunktKvitteringType.ANNET, 
                    "", 
                    xml.InnerXml, 
                    Guid.Parse(konversasjonsId), 
                    Guid.Parse(meldingsId), 
                    1L);
                
                //Act
                var returpostkvittering = Kvitteringsparser.TilReturpostkvittering(kvittering);

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
                const string referanseTilMeldingId = "2049057a-9b53-41bb-9cc3-d10f55fa0f87";
                const string tidspunkt = "2015-11-10T08:26:49.797+01:00";
                const string beskrivelse = "Selvvalgt";
                const Varslingskanal varslingskanal = Varslingskanal.Sms;

                IntegrasjonspunktKvittering kvittering = new IntegrasjonspunktKvittering(
                    1L, 
                    DateTime.Parse(tidspunkt), 
                    IntegrasjonspunktKvitteringType.FEIL, 
                    beskrivelse, 
                    xml.InnerXml, 
                    Guid.Parse(konversasjonsId), 
                    Guid.Parse(meldingsId), 
                    1L);
                
                //Act
                var varslingfeiletkvittering = Kvitteringsparser.TilVarslingFeiletKvittering(kvittering);

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
                const string referanseTilMeldingId = "1d4aff36-b6d2-4506-bc0b-bd62ae6f8966";
                const string tidspunkt = "2015-11-09T16:11:31.171+01:00";

                IntegrasjonspunktKvittering kvittering = new IntegrasjonspunktKvittering(
                    1L, 
                    DateTime.Parse(tidspunkt), 
                    IntegrasjonspunktKvitteringType.LEST, 
                    "", 
                    xml.InnerXml, 
                    Guid.Parse(konversasjonsId), 
                    Guid.Parse(meldingsId), 
                    1L);
                
                //Act
                var åpningskvittering = Kvitteringsparser.TilÅpningskvittering(kvittering);

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
                const string referanseTilMeldingId = "2049057a-9b53-41bb-9cc3-d10f55fa0f87";
                const string tidspunkt = "2015-11-10T08:26:49.797+01:00";
                const string detaljer = "detaljer";
                const Feiltype feiltype = Feiltype.Server;

                IntegrasjonspunktKvittering kvittering = new IntegrasjonspunktKvittering(
                    1L, 
                    DateTime.Parse(tidspunkt), 
                    IntegrasjonspunktKvitteringType.FEIL, 
                    "", 
                    xml.InnerXml, 
                    Guid.Parse(konversasjonsId), 
                    Guid.Parse(meldingsId), 
                    1L);
                
                //Act
                var feilmelding = Kvitteringsparser.TilFeilmelding(kvittering);

                //Assert
                Assert.Equal(konversasjonsId, feilmelding.KonversasjonsId.ToString());
                Assert.Equal(meldingsId, feilmelding.MeldingsId);
                Assert.Equal(referanseTilMeldingId, feilmelding.ReferanseTilMeldingId);
                Assert.Equal(DateTime.Parse(tidspunkt), feilmelding.Feilet);
                Assert.Equal(detaljer, feilmelding.Detaljer);
                Assert.Equal(feiltype, feilmelding.Skyldig);
                Assert.Equal(xml, feilmelding.Xml);
            }
        }
    }
}