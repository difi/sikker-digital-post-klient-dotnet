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

namespace Difi.SikkerDigitalPost.Klient.Utilities
{
    internal class GuidUtility
    {
        public readonly string StandardBusinessDocumentHeaderId = Guid.NewGuid().ToString();
        
        public readonly string BodyId = "soapBody";

        public readonly string EbMessagingId = String.Format("id-{0}", Guid.NewGuid());
        
        public readonly string BinarySecurityTokenId = String.Format("X509-{0}", Guid.NewGuid());
        
        public readonly string TimestampId = String.Format("TS-{0}", Guid.NewGuid());
        
        public readonly string DokumentpakkeId = String.Format("{0}@meldingsformidler.sdp.difi.no", Guid.NewGuid());
    }
}
