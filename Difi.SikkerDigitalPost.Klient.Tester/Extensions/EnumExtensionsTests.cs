using System;
using System.Linq;
using Difi.Felles.Utility;
using Xunit;
using Difi.SikkerDigitalPost.Klient.Domene.Extensions;

namespace Difi.SikkerDigitalPost.Klient.Tester.Extensions
{
    public class EnumExtensionsTests
    {
        public class ToNorwegianStringMethod
        {
            [Fact]
            public void ConvertsAllEnumValues()
            {
                var certificateValidationTypes = Enum.GetValues(typeof(CertificateValidationType)).Cast<CertificateValidationType>();

                foreach (var certificateValidationType in certificateValidationTypes)
                {
                    certificateValidationType.ToNorwegianString();
                }
            }
        }

    }
}
