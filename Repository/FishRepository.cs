﻿using AutoMapper;
using BusinessObject.IMapperConfig;
using BusinessObject.Models;
using BusinessObject.ResponseModel;
using Microsoft.EntityFrameworkCore;
using Repository.Interface;
namespace Repository
{
    public class FishRepository : BaseRepository<Fish> ,IFishRepository
    {
        private readonly KoiCareDBContext _context;

        public FishRepository(KoiCareDBContext context) : base(context) 
        {
            _context = context;

        }

        private List<Fish> fishtList;

        public async Task<List<FishResponseModel>> GetAllFish()
        {
            List<Fish> fishs = await _context.Fishes.ToListAsync();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile());
            });
            var mapper = config.CreateMapper();
            List<FishResponseModel> _fishs = fishs.Select(f => mapper.Map<Fish, FishResponseModel>(f)).ToList();

            return _fishs;
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
            return fish;
        }


    }
}
