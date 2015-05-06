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

using System.Xml;
using Difi.SikkerDigitalPost.Klient.Envelope.Abstract;

namespace Difi.SikkerDigitalPost.Klient.Envelope
{
    internal class UtenInnholdBody : EnvelopeXmlPart
    {
        public UtenInnholdBody(EnvelopeSettings settings, XmlDocument context) : base(settings, context)
        {
        }

        public override XmlNode Xml()
        {
            XmlElement body = Context.CreateElement("env", "Body", Navnerom.SoapEnvelopeEnv12);
            body.SetAttribute("xmlns:wsu", Navnerom.WssecurityUtility10);
            body.SetAttribute("Id", Navnerom.WssecurityUtility10, Settings.GuidHandler.BodyId);
            return body;
        }
    }
}
