using BusinessObject.Models;
using BusinessObject.ResponseModel;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Mscc.GenerativeAI;
using Repository.Interface;
using System.Text;

namespace Service
{
    public class GeminiService : IGeminiService
    {

        private readonly IConfiguration _configuration;
        private readonly string _generativeApiKey;
        private readonly IProductRepository _productRepository;
        private readonly IWaterRepository _waterRepository;

        public GeminiService(IConfiguration configuration, IProductRepository productRepository, IWaterRepository waterRepository)
        {
            _configuration = configuration;
            _generativeApiKey = _configuration["GenerativeAI:ApiKey"] ?? throw new Exception("Cannot find Generative AI API Key"); // Lấy API Key từ cấu hình
            _productRepository = productRepository;
            _waterRepository = waterRepository;
        }
        public async Task<string> AskQuestion(PoolResponseModel pool)
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
            var prompt = $"Đánh giá và tính toán lượng thức ăn cho cá Koi này dựa trên hai thông số: {fish.Weight} gam và {MonthAge} tháng tuổi. Trả về kết quả dưới dạng HTML nhưng không bao gồm thẻ <!DOCTYPE html> và <html>. Trả lời trực tiếp, không thêm lời bình luận phụ.";
            var model = new GenerativeModel();
            model.ApiKey = _generativeApiKey;
            var response = await model.GenerateContent(prompt);

            return response.ToString();
        }

        public async Task<string> AskQuestionCaculatSalt(PoolResponseModel pool)
        {
            List<Product> products = await _productRepository.GetAllProduct();
            Waters waters = await _waterRepository.GetById(pool.WaterId);
            var saltOfpool = waters.Salt;
            var prompt = $"tính toán lượng muối hợp lí cho nước này với size{pool.Size} cm2 ,Depth{pool.Depth} cm, và so sánh với salt {saltOfpool} gram trong hồ, đồng thời tính các chỉ số po4 {waters.Po4} ," +
                $"chỉ số ph {waters.Ph} , chỉ số no2 {waters.No2}, chỉ số no3 {waters.No3}, chỉ số o2 {waters.O2}, và nhiệt độ của nước {waters.Temperature} " +
                $" trả về kết quả  theo thẻ html , không trả về <!DOCTYPE html> và <html> , no yapping";
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

