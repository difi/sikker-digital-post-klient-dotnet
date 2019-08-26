using System;
using System.Linq;
using Xunit;
using Difi.SikkerDigitalPost.Klient.Domene.Extensions;
using Digipost.Api.Client.Shared.Certificate;

namespace Difi.SikkerDigitalPost.Klient.Tester.Extensions
{
    public class EnumExtensionsTests
    {
        public class ToNorwegianStringMethod
        {
            [Fact]
            public void Converts_all_enum_values()
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
