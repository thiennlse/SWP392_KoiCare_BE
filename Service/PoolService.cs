﻿using BusinessObject.Models;
using BusinessObject.ResponseModel;
using Repository;
using Repository.Interface;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class PoolService : IPoolService
    {
        private readonly IPoolRepository _poolRepository;
        public PoolService(IPoolRepository poolRepository)
        {
            _poolRepository = poolRepository;
        }



        public async Task<List<Pool>> GetAllPoolAsync(int page, int pageSize, String? searchTerm)
        {
            return await _poolRepository.GetAllPooltAsync(page, pageSize, searchTerm);
        }

        public async Task<Pool> GetPoolById(int id)
        {
            return await _poolRepository.GetById(id);
        }

        public async Task AddNewPool(Pool newPool)
        {
            await _poolRepository.AddNewPool(newPool);
        }

        public async Task DeletePool(int id)
        { 
           await _poolRepository.DeletePool(id);
        }

        public async Task<Pool> UpdatePool(Pool pool)
        {
            return await _poolRepository.UpdatePool(pool);
        }
    }
}
