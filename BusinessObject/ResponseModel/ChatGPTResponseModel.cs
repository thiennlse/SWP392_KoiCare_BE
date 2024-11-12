using BusinessObject.RequestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BusinessObject.ResponseModel
{
    public class ChatGPTResponseModel
    {
        public List<Choice> Choices { get; set; }
    }

    public class Choice
    {
        public Message Message { get; set; }
    }

    public class Message
    {
        public string Role { get; set; }
        public string Content { get; set; }
    }
}
