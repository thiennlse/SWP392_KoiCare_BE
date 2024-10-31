using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.RequestModel
{
    public class ChatGPTRequestModel
    {
        public string Model { get; set; } = "gpt-3.5-turbo"; // You can change this to "gpt-4" or other models
        public List<Message> Messages { get; set; } = new List<Message>();

        public ChatGPTRequestModel(string userInput)
        {
            Messages.Add(new Message { Role = "system", Content = "You are a helpful assistant." }); // Optional system message
            Messages.Add(new Message { Role = "user", Content = userInput });
        }
    }

    public class Message
    {
        public string Role { get; set; } // "user" or "assistant"
        public string Content { get; set; }
    }
}
