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
            optionsBuilder.UseSqlServer("Password=KoiCaredb123@;Persist Security Info=True;User ID=sa;Initial Catalog=koicaredb;Data Source=localhost,1444");

            return new KoiCareDBContext(optionsBuilder.Options);
        }
    }

}
