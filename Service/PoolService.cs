using BusinessObject.Models;
using BusinessObject.RequestModel;
using BusinessObject.ResponseModel;
using Microsoft.AspNetCore.Http;
using Repository;
using Repository.Interface;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Service
{
    public class PoolService : IPoolService
    {
        private readonly IPoolRepository _poolRepository;
        private readonly IMemberRepository _memberRepository;
        private readonly IHttpContextAccessor _contextAccessor;
        public PoolService(IPoolRepository poolRepository, IHttpContextAccessor contextAccessor, IMemberRepository memberRepository)
        {
            _poolRepository = poolRepository;
            _contextAccessor = contextAccessor;
            _memberRepository = memberRepository;
        }



        public async Task<List<Pool>> GetAllPoolAsync(int page, int pageSize, String? searchTerm)
        {
            return await _poolRepository.GetAllPooltAsync(page, pageSize, searchTerm);
        }

        public async Task<Pool> GetPoolById(int id)
        {
            return await _poolRepository.GetById(id);
        }

        public async Task AddNewPool(PoolRequestModel newPool)
        {
            Pool pool = MapToPool(newPool);
            await _poolRepository.AddNewPool(pool);
        }

        public async Task DeletePool(int id)
        {
            await _poolRepository.DeletePool(id);
        }

        public async Task UpdatePool(int id, PoolRequestModel request)
        {
            var existPool = await _poolRepository.GetById(id);
            Pool pool = MapToPool(request);
            UpdatePoolProperty(existPool, pool);
            await _poolRepository.UpdatePool(existPool);
        }

        private void UpdatePoolProperty(Pool pool, Pool newpool)
        {
            pool.Name = newpool.Name;
            pool.Description = newpool.Description;
            pool.Size = newpool.Size;
            pool.Depth = newpool.Depth;
        }

        private Pool MapToPool(PoolRequestModel request)
        {
            var currUser = _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int currUserId = int.Parse(currUser);
            return new Pool
            {
                Name = request.Name,
                Size = request.Size,
                Depth = request.Depth,
                MemberId = currUserId,
                Description = request.Description,
                Water = new Waters
                {
                    No2 = 0,
                    No3 = 0,
                    O2 = 0,
                    Salt = 0,
                    Ph = 0,
                    Po4 = 0,
                    Temperature = 0,
                }
            };
        }

        public async Task<Double> CalCulateSaltPoolNeed(int poolId)
        {
            Pool _pool = await _poolRepository.GetById(poolId);
            double volumeCubicMeters = 0;
            double volume = 0;
            double saltForPool = 0;
            if (_pool != null)
            {
                volumeCubicMeters = _pool.Size * _pool.Depth;
                volume = volumeCubicMeters * 1;
                saltForPool = 0.001 * volume;
            }
            return Math.Round(saltForPool, 2);
        }
    }
}
