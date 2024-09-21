using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Models
{
    public class KoiCareDBContextFactory : IDesignTimeDbContextFactory<KoiCareDBContext>
    {
        public KoiCareDBContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<KoiCareDBContext>();
            optionsBuilder.UseSqlServer("Server=LAPTOP-G7BTCLNQ;Database=KoiCareDB;Uid=koiCareSystem;Pwd=12345;TrustServerCertificate=True;");

            return new KoiCareDBContext(optionsBuilder.Options);
        }
    }

}
