using System.Security.Cryptography;

namespace Difi.SikkerDigitalPost.Klient.Security
{
    /// <remarks>
    ///     From:
    ///     http://stackoverflow.com/questions/17258800/c-sharp-support-for-rsa-sha-256-signing-for-individual-xml-elements
    ///     Usage: CryptoConfig.AddAlgorithm(typeof(RsaPkCs1Sha256SignatureDescription),
    ///     @"http://www.w3.org/2001/04/xmldsig-more#rsa-sha256");
    /// </remarks>
    public class RsaPkCs1Sha256SignatureDescription : SignatureDescription
    {
        public RsaPkCs1Sha256SignatureDescription()
        {
            KeyAlgorithm = "System.Security.Cryptography.RSACryptoServiceProvider";
            DigestAlgorithm = "System.Security.Cryptography.SHA256Managed";
            FormatterAlgorithm = "System.Security.Cryptography.RSAPKCS1SignatureFormatter";
            DeformatterAlgorithm = "System.Security.Cryptography.RSAPKCS1SignatureDeformatter";
        }

        public override AsymmetricSignatureDeformatter CreateDeformatter(AsymmetricAlgorithm key)
        {
            var asymmetricSignatureDeformatter = (AsymmetricSignatureDeformatter) CryptoConfig.CreateFromName(DeformatterAlgorithm);
            asymmetricSignatureDeformatter.SetKey(key);
            asymmetricSignatureDeformatter.SetHashAlgorithm("SHA256");
            return asymmetricSignatureDeformatter;
        }
    }
}