using BusinessObject.RequestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interface
{
    public interface IChatGPTService {

        public Task MakeRequestAsync();
        public Task<string> SendMessageAsync(ChatGPTRequestModel request);
        public Task FixGrammarAsync(string textToFix);

        public Task<string> ProcessGrammarFix(string userInput, string model, double temperature, int maxTokens);
    }
}
