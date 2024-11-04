using BusinessObject.RequestModel;
using BusinessObject.ResponseModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class ChatGPTService
    {
        private readonly HttpClient _httpClient;

        public ChatGPTService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> SendMessageAsync(ChatGPTRequestModel request)
        {
            var openAIRequest = new
            {
                model = request.Model,
                messages = new[] { new { role = "user", content = request.userInput } },
                max_tokens = request.MaxTokens,
                temperature = request.Temperature
            };

            var jsonContent = JsonConvert.SerializeObject(openAIRequest);

            // Create StringContent with JSON format
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            // Set up the API call with authorization header
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "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");

            // Send the request and await the response
            var response = await _httpClient.PostAsync("https://api.openai.com/v1/chat/completions", content);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Error: {response.ReasonPhrase}");
            }

            // Read and return the response content as a string
            var responseString = await response.Content.ReadAsStringAsync();
            return responseString; // Ensure a value is returned
        }
    }
}
