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

namespace SikkerDigitalPost.Domene.Entiteter.Interface
{
    internal abstract class PMode
    {        
    // ROLES
	public static readonly string RolleMeldingsformidler = "urn:sdp:meldingsformidler";
	public static readonly string RolleAvsender = "urn:sdp:avsender";
	public static readonly string RollePostkasse = "urn:sdp:postkasseleverandør";

	// PARTY IDs
	public static readonly string PartyIdType = "urn:oasis:names:tc:ebcore:partyid-type:iso6523:9908";

	// COLLABORATION INFO
	public static readonly string FormidlingAgreementRefOld = "http://begrep.difi.no/SikkerDigitalPost/Meldingsutveksling/FormidleDigitalPostForsendelse";
	public static readonly string FormidlingAgreementRef = "http://begrep.difi.no/SikkerDigitalPost/1.0/transportlag/Meldingsutveksling/FormidleDigitalPostForsendelse";
	public static readonly string FlyttAgreementRef = "http://begrep.difi.no/SikkerDigitalPost/1.0/transportlag/Meldingsutveksling/FlyttetDigitalPost";
	public static readonly string Service = "SDP";

    }
}
