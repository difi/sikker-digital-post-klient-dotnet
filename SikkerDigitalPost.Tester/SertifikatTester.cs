/** 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *         http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Security.Cryptography.X509Certificates;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SikkerDigitalPost.Tester
{
    [TestClass]
    public class SertifikatTester
    {
        private static X509Store _store;
        private static X509Certificate2 _certificate;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            _store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            _store.Open(OpenFlags.ReadOnly);

            try
            {
                _certificate = _store.Certificates[0];

            }
            catch
            {
                Assert.Fail("Klarte ikke å finne noen sertifikater til å gjøre tester på. Dette er nok fordi du ikke har noen sertifikater i CurrentUser.My.");
            }
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            _store.Close();
        }

        [TestMethod]
        public void TestLowercaseThumbprint()
        {
            //string arr = String.Format("StoreCert: [{0}]", _certificate.Thumbprint);
            //foreach (var cert in _store.Certificates)
            //{
            //    arr += String.Format(" [{0}],", cert.Thumbprint);
            //}
            //throw new Exception(arr);

            var lowercaseThumbprint = _certificate.Thumbprint;
            var certFound = _store.Certificates.Find(X509FindType.FindByThumbprint, lowercaseThumbprint, true)[0];

            Assert.IsTrue(certFound.Equals(_certificate), "Sertifikat funnet med thumbprint matcher ikke referansesertifikat");
        }

        [TestMethod]
        public void TestRandomSpacingThumbprint()
        {
            var randomSpacingThumb = String.Empty;

            var random = new Random();
            for (var i = 0; i < _certificate.Thumbprint.Length; i++)
            {
                if (i % random.Next(1, 5) == 0 && i > 0)
                    randomSpacingThumb += " ";

                randomSpacingThumb += _certificate.Thumbprint[i];
            }
            
            var certificate = _store.Certificates.Find(X509FindType.FindByThumbprint, randomSpacingThumb, true)[0];
            Assert.AreEqual(_certificate, certificate);
            
        }
    }
}
