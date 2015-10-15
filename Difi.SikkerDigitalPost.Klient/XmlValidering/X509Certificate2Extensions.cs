using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using ApiClientShared;

namespace Difi.SikkerDigitalPost.Klient.XmlValidering
{
    public static class X509Certificate2Extensions
    {
        public static X509Certificate2Collection TestSertifikater(this X509Certificate2 sertifikat)
        {
            ResourceUtility resourceUtility = new ResourceUtility("Difi.SikkerDigitalPost.Klient.Resources.test");

            var difiTestkjedesertifikater = new List<X509Certificate2>
            {
                new X509Certificate2(resourceUtility.ReadAllBytes(true, "Buypass_Class_3_Test4_CA_3.cer")),
                new X509Certificate2(resourceUtility.ReadAllBytes(true, "Buypass_Class_3_Test4_Root_CA.cer")),
                new X509Certificate2(resourceUtility.ReadAllBytes(true, "intermediate - commfides cpn enterprise-norwegian sha256 ca - test2.crt")),
                new X509Certificate2(resourceUtility.ReadAllBytes(true, "root - cpn root sha256 ca - test.crt"))
            };
            return new X509Certificate2Collection(difiTestkjedesertifikater.ToArray());
        }
    }
}
