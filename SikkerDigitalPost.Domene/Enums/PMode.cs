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
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Post;

namespace Difi.SikkerDigitalPost.Klient.Domene.Enums
{
    internal enum PMode
    {
        FormidleDigitalPost,
        FormidleFysiskPost,
        KvitteringsForespoersel
    }

    internal static class PModeHelper
    {
        private const string FormidleDigitalPostReferanse = "http://begrep.difi.no/SikkerDigitalPost/1.0/transportlag/Meldingsutveksling/FormidleDigitalPostForsendelse";
        private const string FormidleFysiskPostReferanse = "http://begrep.difi.no/SikkerDigitalPost/1.0/transportlag/Meldingsutveksling/FormidleFysiskPostForsendelse";

        internal static string EnumToRef(PMode pMode)
        {
            switch (pMode)
            {
                case PMode.FormidleDigitalPost:
                    return FormidleDigitalPostReferanse;
                case PMode.FormidleFysiskPost:
                    return FormidleFysiskPostReferanse;
                case PMode.KvitteringsForespoersel:
                    return FormidleDigitalPostReferanse;
                default:
                    throw new ArgumentOutOfRangeException("pMode", pMode.ToString(), "Fant ingen referanse for angitt pMode");
            }
        }

        internal static PMode FromPostInfo(PostInfo postInfo)
        {
            var type = postInfo.GetType();
            
            if(type == typeof(FysiskPostInfo))
                return PMode.FormidleFysiskPost;

            if (type == typeof (DigitalPostInfo))
                return PMode.FormidleDigitalPost;

            throw new ArgumentOutOfRangeException("postInfo", type, "PostInfo har feil type.");

        }
    }
}