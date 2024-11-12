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
        public Task<string> ProcessGrammarFix(string userInput, string model = "gpt-3.5-turbo", double temperature = 0.7, int maxTokens = 200);
    }
}
