﻿using AutoMapper;
using BusinessObject.IMapperConfig;
using BusinessObject.Models;
using BusinessObject.ResponseModel;
using Microsoft.EntityFrameworkCore;
using Repository.Interface;
namespace Repository
{
    public class FishRepository : BaseRepository<Fish>, IFishRepository
    {
        private readonly KoiCareDBContext _context;

        public FishRepository(KoiCareDBContext context) : base(context)
        {
            _context = context;

        }

        private List<Fish> fishtList;

        public async Task<List<Fish>> GetAllFishAsync(int page, int pageSize, string? searchTerm)
        {
            var query = GetQueryable();
            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(f => f.Name.Contains(searchTerm) || f.Origin.Contains(searchTerm));
            }

            var fishs = await query.Include(f => f.FishProperties.OrderByDescending(fp => fp.Date)).Skip((page - 1) * pageSize)
                                       .Take(pageSize)
                                       .ToListAsync();
            return fishs.ToList();
        }



        public async Task AddNewFish(Fish fish)
        {
            _context.Fishes.AddAsync(fish);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteFish(int id)
        {
            var _fish = await GetById(id);
            if (_fish != null)
            {
                _context.Fishes.Remove(_fish);
                await _context.SaveChangesAsync();
            }

        }

        public async Task<Fish> UpdateById(Fish fish)
        {
            _context.Entry(fish).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            await _context.Entry(fish)
             .Collection(f => f.FishProperties)
             .LoadAsync();

            return fish;
        }

        public async Task<Fish> GetFishByIdGetFishProperties(int fishId)
        {
            return await _context.Fishes
                .Include(f => f.FishProperties.OrderByDescending(fp => fp.Date))
                .FirstOrDefaultAsync(f => f.Id == fishId);
        }

        public async Task<FishProperties> GetFishPropertiesForCalculateByFishId(int fishId)
        {
            return (await _context.Fishes
            .Include(f => f.FishProperties.OrderByDescending(fp => fp.Date))
            .FirstAsync(f => f.Id == fishId))
            .FishProperties.First();
        }

        public async Task<Fish> GetLastPropertiesOnDay(int fishId)
        {
            var fish = await _context.Fishes
                .Where(f => f.Id == fishId)
                .Include(f => f.FishProperties.OrderByDescending(fp => fp.Date))
                .FirstOrDefaultAsync();

            if (fish == null || fish.FishProperties == null)
                return null;

            // Lấy FishProperties cuối cùng trong mỗi ngày
            fish.FishProperties = fish.FishProperties
                .GroupBy(fp => fp.Date.Date) // Nhóm theo ngày (chỉ lấy phần Date)
                .Select(g => g.OrderByDescending(fp => fp.Date).First()) // Lấy FishProperty cuối cùng trong ngày
                .ToList();

            return fish;
        }



    }
}
