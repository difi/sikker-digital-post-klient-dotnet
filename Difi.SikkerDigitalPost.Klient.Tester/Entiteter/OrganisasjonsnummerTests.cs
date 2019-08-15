using Difi.SikkerDigitalPost.Klient.Domene.Entiteter;
using Difi.SikkerDigitalPost.Klient.Domene.Exceptions;
using Xunit;
using Assert = Xunit.Assert;

namespace Difi.SikkerDigitalPost.Klient.Tester.Entiteter
{
    public class OrganisasjonsnummerTests
    {
        public class ConstructorMethod
        {
            [Fact]
            public void Initializes_verdi_properly()
            {
                //Arrange
                const string orgnr = "123456789";

                //Act
                var organisasjonsnummer = new Organisasjonsnummer(orgnr);

                //Assert
                Assert.Equal(orgnr, organisasjonsnummer.Verdi);
            }

            [Fact]
            public void Throws_exception_on_invalid()
            {
                //Arrange
                const string orgnr = "1234567";

                //Act
                Assert.Throws<KonfigurasjonsException>(() => new Organisasjonsnummer(orgnr));
            }
        }

        public class WithCountryCodeProperty
        {
            [Fact]
            public void Returns_organisasjonsnummer_with_country_prefix()
            {
                //Arrange
                const string source = "123456789";
                const string expected = "9908:123456789";
                var organisasjonsnummer = new Organisasjonsnummer(source);

                //Act
                var actual = organisasjonsnummer.WithCountryCode;

                //Assert
                Assert.Equal(expected, actual);

            }
        }
    }
}