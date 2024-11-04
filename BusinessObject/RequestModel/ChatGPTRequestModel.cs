using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.RequestModel
{
    public class ChatGPTRequestModel
    {
        public string userInput { get; set; } // The user’s message
        public string Model { get; set; } = "gpt-3.5-turbo"; // Specify the model, defaulting to GPT-3.5-turbo

    }

    public class Message
    {
        public string Role { get; set; } // "user" or "assistant"
        public string Content { get; set; }
    }
}
