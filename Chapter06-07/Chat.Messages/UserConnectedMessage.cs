namespace Chat.Messages
{
    public class UserConnectedMessage : Message
    {
        public UserConnectedMessage() { }
        public UserConnectedMessage(string username) : base(username) { }
    }
}
