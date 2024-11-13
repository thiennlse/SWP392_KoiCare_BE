using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Interface;

namespace Repository
{
    public class SubcriptionRepository : BaseRepository<Subcriptions>, ISubcriptionRepository
    {
        public SubcriptionRepository(KoiCareDBContext context) : base(context)
        {
        }

        public async Task<List<Subcriptions>> GetAll()
        {
            return await _dbSet.AsNoTracking().ToListAsync();
        }
    }
}
