using BusinessObject.Models;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Mscc.GenerativeAI;
using Repository.Interface;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public  class GeminiService : IGeminiService
    {

        private readonly IConfiguration _configuration;
        private readonly string _generativeApiKey; // Thay đổi để lưu trữ API Key
        
        public GeminiService(IConfiguration configuration)
        {
            _configuration = configuration;
            _generativeApiKey = _configuration["GenerativeAI:ApiKey"] ?? throw new Exception("Cannot find Generative AI API Key"); // Lấy API Key từ cấu hình
           
        }
        public async Task<string> AskQuestion(Pool pool)
        {
            var prompt = $"đánh giá chất lượng nước của hồ này với những thông số này:{pool.Water}, trả về theo thẻ html, không trả về <!DOCTYPE html> và <html> , no yapping";
            var model = new GenerativeModel();
            model.ApiKey = _generativeApiKey;
            var response = await model.GenerateContent(prompt);

            return response.ToString();
        }

        public async Task<string> AskQuestionFishFood(Fish fish)
        {
            int MonthAge = DateTime.Now.Month - fish.Dob.Month;
            var prompt = $"tính toán lượng thức ăn dựa vào 2 thông số cân nặng theo kg và tuổi theo tháng của cá Koi này :{fish.Weight},{MonthAge}, trả về theo thẻ html , không trả về <!DOCTYPE html> và <html> , no yapping";
            var model = new GenerativeModel();
            model.ApiKey = _generativeApiKey;
            var response = await model.GenerateContent(prompt);

            return response.ToString();
        }


        public Task<string> PDFToString(IFormFile file)
        {
            StringBuilder text = new StringBuilder();

            var tempFilePath = Path.GetTempFileName();
            using (var stream = new FileStream(tempFilePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            using (PdfReader reader = new PdfReader(tempFilePath))
            using (PdfDocument document = new PdfDocument(reader))
            {
                for (int i = 1; i <= document.GetNumberOfPages(); i++)
                {
                    var pageText = PdfTextExtractor.GetTextFromPage(document.GetPage(i));
                    text.AppendLine(pageText);
                }
            }
            File.Delete(tempFilePath);
            return Task.FromResult(text.ToString());
        }
    }
}

