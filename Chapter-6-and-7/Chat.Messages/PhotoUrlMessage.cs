using System;
using System.Collections.Generic;
using System.Text;

namespace Chat.Messages
{
    public class PhotoUrlMessage : Message
    {
        public PhotoUrlMessage() { }
        public PhotoUrlMessage(string username) : base(username) { }

        public string Url { get; set; }
    }
}
