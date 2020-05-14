using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer.Forretning;
using Difi.SikkerDigitalPost.Klient.Tester.Api;
using Xunit;

namespace Difi.SikkerDigitalPost.Klient.Tester
{
    public class SmokeTests
    {
        private SmokeTestsHelper _t;

        public SmokeTests()
        {
            var miljø = SikkerDigitalPostKlientTests.IntegrasjonsPunktLocalHostMiljø;

            _t = new SmokeTestsHelper(miljø);
        }

        [Fact]
        public void Send_digital_with_multiple_documents()
        {
            _t
                .Assert_Empty_Queue()
                .Create_Digital_Forsendelse_with_multiple_documents()
                .Send()
                .Expect_Message_Response_To_Be_TransportOkKvittering()
                .Fetch_Receipt()
                .Expect_Receipt_To_Be(typeof(Leveringskvittering))
                .ConfirmReceipt();
        }

        [Fact]
        public void Send_digital_with_EHF()
        {
            _t
                .Assert_Empty_Queue()
                .CreateDigitalForsendelseWithEHF()
                .Send()
                .Expect_Message_Response_To_Be_TransportOkKvittering()
                .Fetch_Receipt()
                .Expect_Receipt_To_Be(typeof(Leveringskvittering))
                .ConfirmReceipt();
        }

        [Fact]
        public void Send_digital_with_Datatype()
        {
            _t
                .Assert_Empty_Queue()
                .Create_Digital_Forsendelse_with_Datatype()
                .Send()
                .Expect_Message_Response_To_Be_TransportOkKvittering()
                .Fetch_Receipt()
                .Expect_Receipt_To_Be(typeof(Leveringskvittering))
                .ConfirmReceipt();
        }

        [Fact]
        public void Send_physical()
        {
            _t
                .Assert_Empty_Queue()
                .Create_Physical_Forsendelse()
                .Send()
                .Expect_Message_Response_To_Be_TransportOkKvittering()
                .Fetch_Receipt()
                .Expect_Receipt_To_Be(typeof(Mottakskvittering))
                .ConfirmReceipt();
        }

        [Fact]
        public void Send_on_behalf_of()
        {
            _t
                .Assert_Empty_Queue()
                .Create_digital_forsendelse_with_different_sender()
                .Send()
                .Expect_Message_Response_To_Be_TransportOkKvittering()
                .Fetch_Receipt()
                .ConfirmReceipt();
        }
    }
}
