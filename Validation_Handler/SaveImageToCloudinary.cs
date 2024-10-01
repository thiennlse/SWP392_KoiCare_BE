using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using dotenv.net;
using Microsoft.AspNetCore.Http;

namespace Validation_Handler
{
    public class SaveImageToCloudinary
    {
        public static string SaveImage(IFormFile file)
        {
            DotEnv.Load(options: new DotEnvOptions(probeForEnv: true));
            Cloudinary cloudinary = new Cloudinary(Environment.GetEnvironmentVariable("CLOUDINARY_URL"));
            cloudinary.Api.Secure = true;
            var myImage = cloudinary.Api.UrlImgUp.Transform(new Transformation().Width(50));
            var myUrl = myImage.BuildUrl(file.Name);
            var myImageTag = myImage.BuildImageTag(file.FileName);
            return myUrl;
        }
    }
}
