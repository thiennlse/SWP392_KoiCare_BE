using BusinessObject.RequestModel;
using BusinessObject.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class ChatGPTClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public ChatGPTClient(string apiKey)
        {
            _apiKey = apiKey;
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
        }

        public async Task<string> SendMessageAsync(string userInput)
        {
            // Create a request model with user input
            var request = new ChatGPTRequestModel(userInput);

            // Send the request to OpenAI API
            var response = await _httpClient.PostAsJsonAsync("https://api.openai.com/v1/chat/completions", request);

            if (response.IsSuccessStatusCode)
            {
                // Deserialize the response into ChatGPTResponse model
                var result = await response.Content.ReadFromJsonAsync<ChatGPTResponseModel>();
                return result?.Choices[0].Message.Content ?? "No response from ChatGPT.";
            }
            else
            {
                // Handle error response (for example, show the error code)
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"OpenAI API call failed with status: {response.StatusCode}, Details: {errorContent}");
            }
        }
    }
