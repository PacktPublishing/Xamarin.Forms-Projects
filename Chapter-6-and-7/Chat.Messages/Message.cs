using System;

namespace Chat.Messages
{
    public class Message
    {
        public Type TypeInfo { get; set; }
        public string Id { get; set; }
        public string Username { get; set; }
        public DateTime Timestamp { get; set; }

        public Message() { }
        public Message(string username)
        {
            Id = Guid.NewGuid().ToString();
            TypeInfo = GetType();
            Username = username;
            Timestamp = DateTime.Now;
        }
    }
}
