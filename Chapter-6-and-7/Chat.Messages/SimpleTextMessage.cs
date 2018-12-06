namespace Chat.Messages
{
    public class SimpleTextMessage : Message
    {
        public SimpleTextMessage() { }
        public SimpleTextMessage(string username) : base(username) { }
        public string Text { get; set; }
    }
}
