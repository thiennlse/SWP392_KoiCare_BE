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
            optionsBuilder.UseSqlServer("Server=tcp:koicare-db.database.windows.net,1433;Initial Catalog=KoiCareDB;Persist Security Info=False;User ID=koicaredb;Password=0965015422Th;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");

            return new KoiCareDBContext(optionsBuilder.Options);
        }
    }

}
