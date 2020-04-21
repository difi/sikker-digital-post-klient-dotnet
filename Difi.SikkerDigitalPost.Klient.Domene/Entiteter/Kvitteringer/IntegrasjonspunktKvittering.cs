using System;
using System.Text.Json.Serialization;
using Difi.SikkerDigitalPost.Klient.Domene.Extensions;

namespace Difi.SikkerDigitalPost.Klient.Domene.Entiteter.Kvitteringer
{
    public class IntegrasjonspunktKvittering
    {
        public long id { get; set; }
        
        // [JsonConverter(typeof(DateTimeConverter))]
        public DateTime lastUpdated { get; set; }
        
        [JsonConverter(typeof(JsonStringEnumMemberConverter))]
        public IntegrasjonspunktKvitteringType status { get; set; }
        
        public string description { get; set; }
        
        public string rawReceipt { get; set; }
        
        public Guid conversationId { get; set; }
        
        public Guid messageId { get; set; }
        
        public long convId { get; set; }
        
        public IntegrasjonspunktKvittering()
        {
            
        }

        public IntegrasjonspunktKvittering(long id, DateTime lastUpdated, IntegrasjonspunktKvitteringType status, string description, string rawReceipt, Guid conversationId, Guid messageId, long convId)
        {
            this.id = id;
            this.lastUpdated = lastUpdated;
            this.status = status;
            this.description = description;
            this.rawReceipt = rawReceipt;
            this.conversationId = conversationId;
            this.messageId = messageId;
            this.convId = convId;
        }
    }

    public enum IntegrasjonspunktKvitteringType
    {
        OPPRETTET,
        SENDT,
        MOTTATT,
        LEVERT,
        LEST,
        FEIL,
        ANNET,
        INNKOMMENDE_MOTTATT,
        INNKOMMENDE_LEVERT,
        LEVETID_UTLOPT
    }
}
