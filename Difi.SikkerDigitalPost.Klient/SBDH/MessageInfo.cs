namespace Difi.SikkerDigitalPost.Klient.SBDH
{
    public class MessageInfo
    {
        private string messageType { get; set; }
        private string receiverOrgNumber { get; set; }
        private string senderOrgNumber { get; set; }
        private string conversationId { get; set; }
        private string messageId { get; set; }

        public MessageInfo(string messageType, string receiverOrgNumber, string senderOrgNumber, string conversationId, string messageId)
        {
            this.messageType = messageType;
            this.receiverOrgNumber = receiverOrgNumber;
            this.senderOrgNumber = senderOrgNumber;
            this.conversationId = conversationId;
            this.messageId = messageId;
        }
    }
}
