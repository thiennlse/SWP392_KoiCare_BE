using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class RoleRepository : BaseRepository<Role> ,IRoleRepository
    {
        private readonly KoiCareDBContext _context;

        public RoleRepository(KoiCareDBContext context) : base(context) 
        {
            _context = context;
        }

    }
}
