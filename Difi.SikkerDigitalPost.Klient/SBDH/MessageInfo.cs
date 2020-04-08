namespace Difi.SikkerDigitalPost.Klient.SBDH
{
    public class MessageInfo
    {
        private readonly string messageType;
        private readonly string receiverOrgNumber;
        private readonly string senderOrgNumber;
        private readonly string conversationId;
        private readonly string messageId;

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
