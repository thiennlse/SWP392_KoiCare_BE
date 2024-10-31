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
        public int Index { get; set; }
        public Message Message { get; set; }
        public string FinishReason { get; set; }
    }
}
