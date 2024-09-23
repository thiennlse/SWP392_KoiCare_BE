using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class FishRepository
    {
        private readonly KoiCareDBContext _context;
        
        public FishRepository( KoiCareDBContext context)
        {
            _context = context;
            
        }

        List<Fish> FishList { get; set; }

        public async Task<List<Fish>> GetAllFish() 
        {
        
        }

      

    }
}
