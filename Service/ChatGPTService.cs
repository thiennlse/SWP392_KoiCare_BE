using BusinessObject.RequestModel;
using BusinessObject.ResponseModel;
using Newtonsoft.Json;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class ChatGPTService : IChatGPTService
    {
        private readonly HttpClient _httpClient;

        public ChatGPTService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task MakeRequestAsync()
        {
            using var client = new HttpClient();

            // Add the Bearer token to the Authorization header
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "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");

            var response = await client.GetAsync("https://localhost:7017/api/ChatGPT/fixGrammar");
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

        public async Task FixGrammarAsync(string textToFix)
        {
            using var client = new HttpClient();

            // Set the authorization header (replace "x-api-key" or "Authorization" with correct header based on API requirement)
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "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");

            // Define the request endpoint and payload (if your API accepts a JSON payload)
            var requestUri = "https://localhost:7017/api/ChatGPT/fixGrammar";
            var requestBody = new
            {
                prompt = textToFix
            };

            // Convert request body to JSON
            var content = new StringContent(JsonConvert.SerializeObject(requestBody), System.Text.Encoding.UTF8, "application/json");

            // Send the POST request
            HttpResponseMessage response;
            try
            {
                response = await client.PostAsync(requestUri, content);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while sending the request: {ex.Message}");
                return;
            }

            // Handle the response
            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Response received: " + responseData);
            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode}, {await response.Content.ReadAsStringAsync()}");
            }
        }
        public async Task<string> ProcessGrammarFix(string userInput, string model, double temperature, int maxTokens)
        {
            var requestData = new
            {
                model = model,
                messages = new[] { new { role = "user", content = userInput } },
                temperature = temperature,
                max_tokens = maxTokens
            };

            var response = await _httpClient.PostAsJsonAsync("https://api.openai.com/v1/chat/completions", requestData);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ChatGPTResponseModel>();
                return result?.Choices?.FirstOrDefault()?.Message?.Content ?? "No response from the model.";
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                throw new Exception($"OpenAI API error: {errorMessage}");
            }
        }
    }
}
