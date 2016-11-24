using System;
using System.Collections.Generic;
using Difi.SikkerDigitalPost.Klient.Domene.Exceptions;
using Xunit;

namespace Difi.SikkerDigitalPost.Klient.Tester.Exceptions
{
    public class XmlValidationExceptionTests
    {
        public class ConstructorMethod : XmlValidationExceptionTests
        {
            [Fact]
            public void Should_expose_validationmessages()
            {
                //Arrange
                var validationList = new List<string> { "validationmessage1", "validationmessage2" };

                //Act
                var xmlValidationException = new XmlValidationException("Error in stuff", validationList);

                //Assert
                Assert.NotNull(xmlValidationException.ValidationMessages);
                Assert.Equal(2, xmlValidationException.ValidationMessages.Count);
            }
        }
    }

}