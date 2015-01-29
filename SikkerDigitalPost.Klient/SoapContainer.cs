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
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using SikkerDigitalPost.Domene.Entiteter.Interface;
using SikkerDigitalPost.Domene.Exceptions;
using SikkerDigitalPost.Klient.Utilities;

namespace SikkerDigitalPost.Klient
{
    internal class SoapContainer
    {
        private readonly string _boundary;

        public string Action { get; set; }
        public string ContentLocation { get; set; }
        public IList<ISoapVedlegg> Vedlegg { get; set; }
        public ISoapVedlegg Envelope { get; set; }

        public SoapContainer()
        {
            _boundary = Guid.NewGuid().ToString();
            Vedlegg = new List<ISoapVedlegg>();
        }

        public void SendOld(HttpWebRequest request)
        {
            if (Envelope == null)
                throw new SendException("Kan ikke sende en Soap-melding uten en envelope.");

            if (!string.IsNullOrWhiteSpace(Action))
                request.Headers.Add("SOAPAction", Action);

            request.ContentType = string.Format("Multipart/Related; boundary=\"{0}\"; type=\"application/soap+xml\"; start=\"<{1}>\"", _boundary, Envelope.ContentId);
            request.Method = "POST";
            request.Accept = "*/*";

            using (var stream = new StreamWriter(request.GetRequestStream()))
            {
                WriteAttachment(stream, Envelope, true);

                foreach (var item in Vedlegg)
                {
                    WriteAttachment(stream, item, false);
                }

                stream.Write("\r\n--" + _boundary + "--");
                stream.Flush();
            }
        }

        public void Send(HttpWebRequest request)
        {
            if (Envelope == null)
                throw new SendException("Kan ikke sende en Soap-melding uten en envelope.");

            if (!string.IsNullOrWhiteSpace(Action))
                request.Headers.Add("SOAPAction", Action);

            request.ContentType = string.Format("Multipart/Related; boundary=\"{0}\"; type=\"application/soap+xml\"; start=\"<{1}>\"", _boundary, Envelope.ContentId);
            request.Method = "POST";
            request.Accept = "*/*";

            //Åpner memorystream for mellomlagring.
            using (var mem = new MemoryStream())
            {
                //Hiv inn i mem
                using (var stream = new StreamWriter(mem))
                {
                    WriteAttachment(stream, Envelope, true);

                    foreach (var item in Vedlegg)
                    {
                        WriteAttachment(stream, item, false);
                    }
                   
                    stream.Write("\r\n--" + _boundary + "--");
                    stream.Flush();

                    mem.Position = 0;

                    //Skriv mem til fil
                    var soapPath = FileUtility.AbsolutePath("SENDT", "SOAP-UTF8.xml");
                    string data = null;
                    using (var fileWriter = new StreamWriter(soapPath))
                    {
                        data = Encoding.UTF8.GetString(mem.ToArray());
                        fileWriter.Write(data);
                    }
                    
                    Debug.WriteLine(data);
                    
                    //Send data til MF
                    using (var reqStream = request.GetRequestStream())
                    {
                        var bytes = mem.ToArray();
                        reqStream.Write(bytes, 0, bytes.Length);
                        reqStream.Flush();
                    }

                } //End Streamwriter SOAP-data

            } //End memorystream
       
        }

        private void WriteAttachment(StreamWriter stream, ISoapVedlegg attachment, bool isFirst)
        {
            if (!isFirst)
                stream.Write("\r\n\r\n");

            stream.WriteLine("--" + _boundary);
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
