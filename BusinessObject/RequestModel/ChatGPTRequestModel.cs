using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.RequestModel
{
    public class ChatGPTRequestModel
    {
        public string userInput { get; set; }

        public List<Message> Messages { get; set; } = new List<Message>();

        public string Model { get; set; } = "gpt-3.5-turbo";

        public double Temperature { get; set; } = 0.7;

        public int MaxTokens { get; set; } = 100;
    }


    public class Message
    {
        public string Role { get; set; }
        public string Content { get; set; }
    }
}