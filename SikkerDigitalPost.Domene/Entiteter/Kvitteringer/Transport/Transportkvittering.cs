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
using System.Xml;
using SikkerDigitalPost.Domene.Exceptions;

namespace SikkerDigitalPost.Domene.Entiteter.Kvitteringer.Transport
{
    /// <summary>
    /// Abstrakt klasse for transportkvitteringer.
    /// </summary>
    public abstract class Transportkvittering : Kvittering
    {
        private readonly XmlDocument _document;
        private readonly XmlNamespaceManager _namespaceManager;

        /// <summary>
        /// Alle subklasser skal ha en ToString() som beskriver kvitteringen.
        /// </summary>
        public abstract override string ToString();
        
        protected Transportkvittering()
        { }

        protected Transportkvittering(XmlDocument document, XmlNamespaceManager namespaceManager)
            : base(document, namespaceManager)
        {
            try
            {
                _document = document;
                _namespaceManager = namespaceManager;
            }
            catch (Exception e)
            {
                throw new XmlParseException(
                   String.Format("Feil under bygging av {0} (av type Transportkvittering). Klarte ikke finne alle felter i xml."
                   , GetType()), e);
            }
        }
    }
}
