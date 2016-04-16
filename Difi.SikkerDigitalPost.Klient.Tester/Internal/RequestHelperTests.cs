using Difi.SikkerDigitalPost.Klient.Internal;
using Difi.SikkerDigitalPost.Klient.XmlValidering;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Difi.SikkerDigitalPost.Klient.Tester.Internal
{
    [TestClass]
    public class RequestHelperTests
    {
        [TestClass]
        public class ConstructorMethod : RequestHelperTests
        {
            [TestMethod]
            public void InitializesFields()
            {
                //Arrange
                var clientConfiguration = new Klientkonfigurasjon(Miljø.FunksjoneltTestmiljø);

                //Act
                var requestHelper = new RequestHelper(clientConfiguration);

                //Assert
                Assert.AreEqual(clientConfiguration, requestHelper.ClientConfiguration);
            } 
        }

        [TestClass]
        public class SendMethod : RequestHelperTests
        {
            [TestMethod]
            public void SendsNewMessageSuccessfully()
            {
                //Arrange
                

                //Act

                //Assert
                Assert.Fail();
            }

            [TestMethod]
            public void ThrowSendExceptionOnNoConnection()
            {
                //Arrange
                

                //Act

                //Assert
                Assert.Fail();
            }

            [TestMethod]
            public void ReturnsTransportErrorReceiptOnError()
            {
                //Arrange
                //Todo: There is done special parsing if message has no content in case of error. We need to check that this is done correctly somehow.

                //Act

                //Assert
                Assert.Fail();
            }

        }
    }
}
