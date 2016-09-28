using System.Collections.Generic;
using Difi.SikkerDigitalPost.Klient.Domene.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Difi.SikkerDigitalPost.Klient.Tester.Exceptions
{
    [TestClass]
    public class XmlValidationExceptionTests
    {
        [TestClass]
        public class ConstructorMethod : XmlValidationExceptionTests
        {
            [TestMethod]
            [ExpectedException(typeof (XmlValidationException))]
            public void XmlValidationExceptionWithValidationMessagesShouldExposeValidationmessages()
            {
                //Arrange
                var validationList = new List<string> {"validationmessage1", "validationmessage2"};

                //Act
                try
                {
                    throw new XmlValidationException("Error in stuff", validationList);
                }
                catch (XmlValidationException exception)
                {
                    //Assert
                    Assert.IsNotNull(exception.ValidationMessages);
                    Assert.AreEqual(2, exception.ValidationMessages.Count);
                    throw;
                }
            }
        }
    }
}