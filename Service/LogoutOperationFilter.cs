using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class LogoutOperationFilter : IOperationFilter
    {  
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.OperationId == "Logout")
            {
                operation.Responses.Add("200", new OpenApiResponse { Description = "Logged out successfully" });
            }
        }
    }
}
