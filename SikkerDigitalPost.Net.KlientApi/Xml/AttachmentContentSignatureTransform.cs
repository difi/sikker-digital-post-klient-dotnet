using System;
using System.IO;
using System.Security.Cryptography.Xml;
using System.Xml;

namespace SikkerDigitalPost.Net.Klient.Xml
{
    /// <summary>
    /// Represents the attachment content signature transform as defined in the Web Services Security SOAP Message with Attachments (SwA) Profile for use with binary attachments. 
    /// </summary>
    internal class AttachmentContentSignatureTransform : Transform
    {
        private Type[] _inputTypes = new Type[2] {
            typeof(Stream),
            typeof(byte[])
        };

        private Type[] _outputTypes = new Type[1] {
            typeof (Stream)
        };

        private Stream result = null;

        /// <summary>
        /// Initializes a new instance of the AttachmentContentSignatureTransform class.
        /// </summary>
        public AttachmentContentSignatureTransform()
        {
            this.Algorithm = "http://docs.oasis-open.org/wss/oasis-wss-SwAProfile-1.1#Attachment-Content-Signature-Transform";
                        
        }

        protected override XmlNodeList GetInnerXml()
        {
            return null;
        }

        public override object GetOutput(Type type)
        {
            return GetOutput();
        }

        public override object GetOutput()
        {
            return ((Object)result);
        }
        
        /// <summary>
        /// Returns a list of the valid input types for this transformer. Valid types are Stream and byte[].
        /// </summary>
        public override Type[] InputTypes
        {
            get { return _inputTypes; }
        }

        public override void LoadInnerXml(XmlNodeList nodeList)
        {
        }

        /// <summary>
        /// Loads the data for transformation. Valid types are Stream or byte[].
        /// </summary>
        /// <param name="obj"></param>
        public override void LoadInput(object obj)
        {
            if (obj is Stream)
            {
                result = new MemoryStream();
                ((Stream)obj).CopyTo(result);
                if (((Stream)obj).CanSeek)
                    ((Stream)obj).Position = 0;

                result.Position = 0;
            }
            else
            {
                result = new MemoryStream((byte[])obj);
            }
        }

        public override Type[] OutputTypes
        {
            get { return _outputTypes; }
        }
    }
}
