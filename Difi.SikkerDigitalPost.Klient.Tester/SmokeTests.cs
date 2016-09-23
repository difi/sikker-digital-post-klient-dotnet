using System;
using System.Threading.Tasks;
using Difi.SikkerDigitalPost.Klient.Api;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Aktører;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer.Forretning;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer.Transport;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Post;
using Difi.SikkerDigitalPost.Klient.Domene.Enums;
using Difi.SikkerDigitalPost.Klient.Tester.Utilities;
using Difi.SikkerDigitalPost.Klient.XmlValidering;
using Xunit;

namespace Difi.SikkerDigitalPost.Klient.Tester
{
    public class SmokeTests
    {
        private SmokeTestsHelper _t;

        public SmokeTests()
        {
            var miljø = Miljø.FunksjoneltTestmiljø;

            _t = new SmokeTestsHelper(miljø);
        }

        [Fact]
        public void Send_digital_with_multiple_documents()
        {
            _t
                .Create_Digital_Forsendelse_with_multiple_documents()
                .Send()
                .Expect_Message_Response_To_Be_TransportOkKvittering()
                .Fetch_Receipt()
                .ConfirmReceipt();
        }

        [Fact]
        public void Send_physical()
        {
            _t
                .Create_Physical_Forsendelse()
                .Send()
                .Expect_Message_Response_To_Be_TransportOkKvittering()
                .Fetch_Receipt()
                .ConfirmReceipt();
        }

        [Fact]
        public void Send_on_behalf_of()
        {
            _t
                .Create_digital_forsendelse_with_different_sender()
                .Send()
                .Expect_Message_Response_To_Be_TransportOkKvittering()
                .Fetch_Receipt()
                .ConfirmReceipt();
        }
    }
}