using System.Security.Cryptography.X509Certificates;

namespace Difi.SikkerDigitalPost.Klient.XmlValidering
{
    internal class SertifikatValidatorProduksjon : Sertifikatvalidator
    {
        public SertifikatValidatorProduksjon(X509Certificate2Collection sertifikatLager) : base(sertifikatLager)
        {
        }

        public override X509ChainStatus[] ValiderResponssertifikat(X509Certificate2 sertifikat)
        {
            throw new System.NotImplementedException();
        }

        public override bool ErGyldigResponssertifikat(X509Certificate2 sertifikat)
        {
            throw new System.NotImplementedException();
        }
    }
}
