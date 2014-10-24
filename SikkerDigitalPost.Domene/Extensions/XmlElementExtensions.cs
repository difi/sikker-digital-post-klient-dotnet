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

namespace SikkerDigitalPost.Domene.Extensions
{
    internal static class XmlElementExtensions
    {
        public static XmlElement AppendChildElement(this XmlElement parent, string childname, string namespaceUri, XmlDocument document)
        {
            var child = document.CreateElement(childname, namespaceUri);
            parent.AppendChild(child);
            return child;
        }

        public static XmlElement AppendChildElement(this XmlElement parent, string childname, string prefix, string namespaceUri, XmlDocument document)
        {
            var child = document.CreateElement(prefix, childname, namespaceUri);
            parent.AppendChild(child);
            return child;
        }

        public static XmlElement AppendChild(this XmlElement element, string localName, string namespaceUri, string value = null)
        {
            var newElement = element.OwnerDocument.CreateElement(null, localName, namespaceUri);
            element.AppendChild(newElement);
            if (value != null)
                newElement.InnerText = value;

            return newElement;
        }

    }
}
