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
        public double Temperature { get; set; } = 0.7; // Controls randomness of the output
        public int MaxTokens { get; set; } = 100; // Maximum length of response
    }

    public class Message
    {
        public string Role { get; set; } // "user" or "assistant"
        public string Content { get; set; }
    }
}
