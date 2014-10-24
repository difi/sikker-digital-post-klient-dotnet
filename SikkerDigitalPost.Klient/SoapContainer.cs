using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using SikkerDigitalPost.Domene.Entiteter.Interface;
using SikkerDigitalPost.Domene.Exceptions;

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

        public void Send(HttpWebRequest request)
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
