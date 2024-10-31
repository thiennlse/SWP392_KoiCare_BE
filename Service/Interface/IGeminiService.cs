using BusinessObject.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interface
{
    public interface IGeminiService
    {
        public Task<string> AskQuestion(Pool pool);

        public Task<string> PDFToString(IFormFile file);

        public Task<string> AskQuestionFishFood(Fish fish);

        public  Task<string> AskQuestionCaculatSalt(Pool pool);
    }
}
