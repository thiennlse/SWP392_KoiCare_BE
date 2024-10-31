using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interface
{
    public interface IChatGPTClient { 
        public Task<string> SendMessageAsync(string userInput);
    }
}
