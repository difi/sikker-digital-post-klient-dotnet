using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using ApiClientShared;

namespace Difi.SikkerDigitalPost.Klient.Utilities
{
    public static class SertifikatUtility
    {
        static readonly ResourceUtility ResourceUtility = new ResourceUtility("Difi.SikkerDigitalPost.Klient.Resources.sertifikater");

        public static X509Certificate2Collection FunksjoneltTestmiljøSertifikater()
        {
            var difiTestkjedesertifikater = new List<X509Certificate2>
            {
                new X509Certificate2(ResourceUtility.ReadAllBytes(true,"test", "Buypass_Class_3_Test4_CA_3.cer")),
                new X509Certificate2(ResourceUtility.ReadAllBytes(true,"test", "Buypass_Class_3_Test4_Root_CA.cer")),
                new X509Certificate2(ResourceUtility.ReadAllBytes(true,"test", "intermediate - commfides cpn enterprise-norwegian sha256 ca - test2.crt")),
                new X509Certificate2(ResourceUtility.ReadAllBytes(true,"test", "root - cpn root sha256 ca - test.crt"))
            };
            return new X509Certificate2Collection(difiTestkjedesertifikater.ToArray());
        }

        public static X509Certificate2Collection ProduksjonsSertifikater()
        {
            var difiProduksjonssertifikater = new List<X509Certificate2>
            {
                new X509Certificate2(ResourceUtility.ReadAllBytes(true, "prod", "BPClass3CA3.cer")),
                new X509Certificate2(ResourceUtility.ReadAllBytes(true, "prod", "BPClass3RootCA.cer")),
                new X509Certificate2(ResourceUtility.ReadAllBytes(true, "prod", "cpn enterprise sha256 class 3.crt")),
                new X509Certificate2(ResourceUtility.ReadAllBytes(true, "prod", "cpn rootca sha256 class 3.crt"))
            }
;
            return new X509Certificate2Collection(difiProduksjonssertifikater.ToArray());
        }
    }
}