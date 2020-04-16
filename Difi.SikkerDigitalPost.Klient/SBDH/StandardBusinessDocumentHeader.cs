using System;
using System.Collections.Generic;
using System.Linq;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter;
using Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Aktører;

namespace Difi.SikkerDigitalPost.Klient.SBDH
{
    public class StandardBusinessDocumentHeader
    {
        public string headerVersion { get;  }  = "1.0";

        public List<Sender> sender
        {
            get { return _sender; }
            set { _sender = value; }
        }
        
        private List<Sender> _sender = new List<Sender>();

        public List<Receiver> receiver
        {
            get { return _receiver; }
            set { _receiver = value; }
        }
        
        private List<Receiver> _receiver = new List<Receiver>();
        public DocumentIdentification documentIdentification { get; set; }
        public BusinessScope businessScope { get; set; }


        public StandardBusinessDocumentHeader AddSender(Sender sender)
        {
            this.sender.Add(sender);
            return this;
        }

        public StandardBusinessDocumentHeader AddReceiver(Receiver receiver)
        {
            this.receiver.Add(receiver);
            return this;
        }
        
        public StandardBusinessDocumentHeader SetBusinessScope(BusinessScope businessScope)
        {
            this.businessScope = businessScope;
            return this;
        }
        
        public StandardBusinessDocumentHeader SetDocumentIdentification(DocumentIdentification documentIdentification) {
            this.documentIdentification = documentIdentification;
            return this;
        }

        public class Builder
        {
            private static readonly string TYPE_VERSION = "1.0";

            private string mottaker { get; set; }
            private string sender { get; set; }
            private string onBehalfOf { get; set; }
            private string conversationId { get; set; }
            private string messageId { get; set; }
            private string documentType { get; set; }
            private string standard { get; set; }
            private Process.ProcessType process { get; set; }
            private DateTime creationDateAndTime { get; set; }

            public Builder WithTo(Organisasjonsnummer organisasjonsnummer)
            {
                mottaker = organisasjonsnummer.WithCountryCode;
                return this;
            }

            public Builder WithTo(DigitalPostMottaker mottaker) {
                this.mottaker = mottaker.Personidentifikator;
                return this;
            }

            public Builder WithTo(String mottaker) {
                this.mottaker = mottaker;
                return this;
            }
            
            public Builder WithFrom(Databehandler sender) {
                this.sender = sender.Organisasjonsnummer.Verdi;
                return this;
            }
            
            public Builder WithFrom(Organisasjonsnummer sender) {
                this.sender = sender.Verdi;
                return this;
            }
            
            public Builder WithOnBehalfOf(Organisasjonsnummer onBehalfOf) {
                this.onBehalfOf = onBehalfOf.Verdi;
                return this;
            }
            
            public Builder WithStandard(string type) {
                standard = type;
                return this;
            }
            
            public Builder WithType(String type) {
                documentType = type;
                return this;
            }
            
            public Builder WithRelatedToConversationId(String conversationId) {
                this.conversationId = conversationId;
                return this;
            }

            public Builder WithRelatedToMessageId(String messageId) {
                this.messageId = messageId;
                return this;
            }

            public Builder WithCreationDateAndTime(DateTime creationDateAndTime) {
                this.creationDateAndTime = creationDateAndTime;
                return this;
            }

            public Builder WithProcess(Process.ProcessType processType)
            {
                this.process = processType;
                return this;
            }
            
            public StandardBusinessDocumentHeader Build() {
                StandardBusinessDocumentHeader standardBusinessDocumentHeader = new StandardBusinessDocumentHeader();

                return standardBusinessDocumentHeader
                    .AddReceiver(CreateReciever(mottaker))
                    .AddSender(CreateSender(sender, onBehalfOf))
                    .SetBusinessScope(CreateBusinessScope(FromConversationId(conversationId)))
                    .SetDocumentIdentification(CreateDocumentIdentification(messageId, documentType, standard, creationDateAndTime));
            }
            
            private Receiver CreateReciever(String mottaker) {
                PartnerIdentification identification = new PartnerIdentification();
                
                identification.value = mottaker;
                identification.authority = Organisasjonsnummer.ISO6523_ACTORID;

                return new Receiver(identification);
            }

            private Sender CreateSender(string avsender, string onBehalfOf) {
                PartnerIdentification identification = new PartnerIdentification();
                String value = Organisasjonsnummer.COUNTRY_CODE_ORGANIZATION_NUMBER_NORWAY + ":" + avsender;
                
                if(onBehalfOf != null) {
                    value += ":" + onBehalfOf;
                }
                
                identification.value = value;
                identification.authority = Organisasjonsnummer.ISO6523_ACTORID;
                
                return new Sender(identification);
            }
            
            private DocumentIdentification CreateDocumentIdentification(string messageId, string documentType, string standard, DateTime creationDateAndTime) {
                if (documentType == null) {
                    throw new ArgumentException("documentType must be set");
                }

                DocumentIdentification documentIdentification = new DocumentIdentification();
                documentIdentification.standard = standard;
                documentIdentification.type = documentType;
                documentIdentification.typeVersion = TYPE_VERSION;
                documentIdentification.instanceIdentifier = messageId;
                documentIdentification.creationDateAndTime = creationDateAndTime;
                
                return documentIdentification;
            }
            
            private static BusinessScope CreateBusinessScope(params Scope[] scopes) {
                BusinessScope businessScope = new BusinessScope();
                businessScope.scope = scopes.ToList();
                return businessScope;
            }

            private Scope FromConversationId(String conversationId) {
                Scope scope = CreateDefaultScope();
                scope.type = ScopeType.ConversationId.ToString();
                scope.instanceIdentifier = conversationId;
                return scope;
            }
            
            private Scope CreateDefaultScope() {
                if (process == null) {
                    throw new ArgumentException("Process must be set");
                }

                Scope scope = new Scope();
                scope.identifier = Process.GetEnumDescription(process);
                
                return scope;
            }
        }
    }
}
