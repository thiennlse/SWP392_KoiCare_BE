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
        public string Message { get; set; } // The ChatGPT response text
        public bool Success { get; set; } // Status of the API call
        public string ErrorMessage { get; set; } // Error message, if any
    }

    public class Choice
    {
        public int Index { get; set; }
        public Message Message { get; set; }
        public string FinishReason { get; set; }
    }
}
