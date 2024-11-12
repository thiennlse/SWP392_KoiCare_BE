using BusinessObject.RequestModel;
using BusinessObject.ResponseModel;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class ChatGPTService : IChatGPTService
    {
        private readonly IConfiguration _configuration;
        private readonly string _chatGPTApiKey;
        

        public ChatGPTService(IConfiguration configuration)
        {
            _configuration = configuration;
            _chatGPTApiKey = _configuration["ChatGPT:ApiKey"] ?? throw new Exception("Cannot find ChatGPT API Key");
            
        }

        public async Task MakeRequestAsync()
        {
            try
            {
                var requestUrl = "https://localhost:7017/api/ChatGPT/fixGrammar"; // Replace with your actual URL
                var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _chatGPTApiKey);

                using var client = new HttpClient();
                var response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(result);
                }
                else
                {
                    Console.WriteLine($"Error: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Request failed: {ex.Message}");
            }
        }


        public async Task<string> ProcessGrammarFix(string userInput, string model = "gpt-3.5-turbo", double temperature = 0.7, int maxTokens = 200)
        {
            try
            {
                var requestData = new
                {
                    model = model,
                    messages = new[] { new { role = "user", content = userInput } },
                    temperature = temperature,
                    max_tokens = maxTokens
                };

                var requestJson = JsonConvert.SerializeObject(requestData);

                var requestUrl = "https://api.openai.com/v1/chat/completions"; // OpenAI API endpoint
                var request = new HttpRequestMessage(HttpMethod.Post, requestUrl)
                {
                    Content = new StringContent(requestJson, Encoding.UTF8, "application/json")
                };

                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _chatGPTApiKey);

                using var client = new HttpClient();
                var response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    var chatResponse = JsonConvert.DeserializeObject<ChatGPTResponseModel>(result);
                    return chatResponse?.Choices?.FirstOrDefault()?.Message?.Content ?? "No response from the model.";
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    throw new Exception($"OpenAI API error: {errorMessage}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw;
            }
        }
    }
}
