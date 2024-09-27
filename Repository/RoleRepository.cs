using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class RoleRepository : IRoleRepository
    {
        private readonly KoiCareDBContext _context;

        public RoleRepository(KoiCareDBContext context)
        {
            _context = context;
        }

        public async Task<Role> GetRoleById(int id)
        {
            return await _context.Roles.FirstOrDefaultAsync( r => r.Id.Equals(id));
        }
    }
}
