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
using System.Collections.Generic;
using System.IO;
using System.Net;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Interface;
using Difi.SikkerDigitalPost.Klient.Domene.Exceptions;

namespace Difi.SikkerDigitalPost.Klient
{
    internal class SoapContainer
    {
        public readonly string Boundary;

        public string Action { get; set; }
        public string ContentLocation { get; set; }
        public IList<ISoapVedlegg> Vedlegg { get; set; }
        public ISoapVedlegg Envelope { get; set; }
        public byte[] SisteBytesSendt { get; private set; }

        public SoapContainer()
        {
            Boundary = Guid.NewGuid().ToString();
            Vedlegg = new List<ISoapVedlegg>();
        }

        public void Send(HttpWebRequest request)
        {
            if (Envelope == null)
                throw new SendException("Kan ikke sende en Soap-melding uten en envelope.");

            if (!string.IsNullOrWhiteSpace(Action))
                request.Headers.Add("SOAPAction", Action);

            request.ContentType = string.Format("Multipart/Related; boundary=\"{0}\"; type=\"application/soap+xml\"; start=\"<{1}>\"", Boundary, Envelope.ContentId);
            request.Method = "POST";
            request.Accept = "*/*";

            using (var cachedResponseStream = new MemoryStream())
            {
                using (var cachedResponseWriter = new StreamWriter(cachedResponseStream))
                {
                    SkrivVedlegg(cachedResponseWriter, Envelope, isFirst: true);

                    foreach (var item in Vedlegg)
                    {
                        SkrivVedlegg(cachedResponseWriter, item, isFirst: false);
                    }
                   
                    cachedResponseWriter.Write("\r\n--" + Boundary + "--");
                    cachedResponseWriter.Flush();

                    var cachedResponseBytes = cachedResponseStream.ToArray();
                    
                    using (var reqStream = request.GetRequestStream())
                    {
                        reqStream.Write(cachedResponseBytes, 0, cachedResponseBytes.Length);
                    }
                    
                    SisteBytesSendt = cachedResponseBytes;
                }
            }
        }

        private void SkrivVedlegg(StreamWriter stream, ISoapVedlegg attachment, bool isFirst)
        {
            if (!isFirst)
                stream.Write("\r\n\r\n");

            stream.WriteLine("--" + Boundary);
            if (!string.IsNullOrWhiteSpace(attachment.Innholdstype))
                stream.WriteLine("Content-Type: " + attachment.Innholdstype);
            if (!string.IsNullOrWhiteSpace(attachment.TransferEncoding))
                stream.WriteLine("Content-Transfer-Encoding: " + attachment.TransferEncoding);
            if (!string.IsNullOrWhiteSpace(attachment.ContentId))
                stream.WriteLine("Content-ID: <" + attachment.ContentId + ">");

            stream.WriteLine();

            stream.Flush();

            stream.BaseStream.Write(attachment.Bytes, 0, attachment.Bytes.Length);
        }
    }
}
