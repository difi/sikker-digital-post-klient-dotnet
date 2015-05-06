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

using System.IO;

namespace Difi.SikkerDigitalPost.Klient.Domene
{
    /// <summary>
    /// Represents the &lt;reference&gt; element of an Xml Signature with http://www.w3.org/2001/04/xmlenc#sha256 as the digest method.
    /// </summary>
    internal class Sha256Reference : System.Security.Cryptography.Xml.Reference
    {
        public Sha256Reference()
        {
            SetDigest();
        }

        public Sha256Reference(Stream stream): base(stream)
        {
            SetDigest();
        }

        public Sha256Reference(string uri): base(uri)
        {
            SetDigest();
        }

        public Sha256Reference(byte[] bytes): base(new MemoryStream(bytes))
        {
           SetDigest();
        }

        private void SetDigest()
        {
            DigestMethod = "http://www.w3.org/2001/04/xmlenc#sha256";
        }
    }
}
