using BusinessObject.Models;
using BusinessObject.RequestModel;
using BusinessObject.ResponseModel;
using iText.Layout.Borders;
using iText.Layout.Element;
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
        private readonly IWaterRepository _waterRepository;
        private readonly IProductRepository _productRepository;
        private readonly IFishRepository _fishRepository;
        public PoolService(IPoolRepository poolRepository, IHttpContextAccessor contextAccessor, IMemberRepository memberRepository, IWaterRepository waterRepository, IProductRepository productRepository, IFishRepository fishRepository)
        {
            _poolRepository = poolRepository;
            _contextAccessor = contextAccessor;
            _memberRepository = memberRepository;
            _waterRepository = waterRepository;
            _productRepository = productRepository;
            _fishRepository = fishRepository;   
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
        public async Task<List<Fish>> GetFishByPoolId(int poolId)
        {
            return await _poolRepository.GetFishByPoolId(poolId);
        }


        // Volumne of pool
        public async Task<double> CalCulateVolumeOfPool(int poolId)
        {
            Pool _pool = await _poolRepository.GetById(poolId);
            double volumeCubicMeters = 0;
            double volume = 0;
           
            if (_pool != null)
            {
                volumeCubicMeters = _pool.Size * _pool.Depth;
                volume = volumeCubicMeters;
            }
           return volume; // lit
        }

        // find total fish on pool
        public async Task<int> TotalFishInPool(int poolId)
        {
            List<Fish> poolFish = await _poolRepository.GetFishByPoolId(poolId); 
            int numberOfKoi = 0;
            if (poolFish != null)
            {
                numberOfKoi = poolFish.Count;
            }       
            return numberOfKoi;
        }


        // calculate standard of salt
        public async Task<double> CalculateSaltOfPool(double waterSalt, double volumOfWater)
        {
            double standardOfSalt = volumOfWater * 0.2 / 100;// kg
            return Math.Round(standardOfSalt,5);

        }

        // Calculate standard of No3
        public async Task<double> CalculateNo3OfPool(double waterNo3)
        {
            double standardOfNo3 = waterNo3 * 0.2 * 0.16;
            return Math.Round(standardOfNo3,5);//g
        }

        //Calculate standard of No2
        public async Task<double> CalculateNo2OfPool(int poolId, double volumeOfwater)
        {
            int totalFish = await TotalFishInPool(poolId);
            double Nh3 = totalFish * 0.2 * 24;
            double Nh3gas = Nh3  / volumeOfwater; // mg/l
            double standardOfNo2 = Nh3gas * 0.33;
            return Math.Round(standardOfNo2, 5);
        }

        //Calculate standard of Po4
        public async Task<double> CalculatePO4OfPool(int poolId,  double VolumeOfpool) 
        {
            int totalFish = await TotalFishInPool(poolId);
            double numberPo4OnDay = totalFish * 0.5;
            double standardOfPo4 = numberPo4OnDay *10/ VolumeOfpool;// mg
            return Math.Round(standardOfPo4, 5);
        }
        // Calcultae Standard of O2
        public async Task<double> CalculateO2OfPool(int poolId, double VolumneOfPool)
        {
            int totalFish = await TotalFishInPool(poolId);
            double numberOfO2OnDay = totalFish * 0.1 * 24; // g
            double standardOfO2 = numberOfO2OnDay /VolumneOfPool;
            return Math.Round(standardOfO2, 5);

        }

        public async Task<WaterElementResponseModel> CheckWaterElementInPool(int PoolId)
        {
            Pool pool = await _poolRepository.GetById(PoolId);
            double volumOfWater = await CalCulateVolumeOfPool(PoolId);
            List<int> products = new List<int>();
            WaterElementResponseModel waterElementResponseModel = new WaterElementResponseModel();
            if (pool != null)
            {
                Waters waters = await _waterRepository.GetById(pool.WaterId);

                if (waters != null)
                {
                    // Call calculate standard of salt 
                    double standardOfSalt = await CalculateSaltOfPool(waters.Salt, volumOfWater);
                    // Call calculate standard of No3
                    double standardOfNo3 = await CalculateNo3OfPool(waters.No3);
                    // Call calculate standard of No2
                    double standardOfNo2 = await CalculateNo2OfPool(PoolId, volumOfWater);
                    // Call calculate standard of po4
                    double standardOfPo4 = await CalculatePO4OfPool(PoolId, volumOfWater);
                    // Call calculate standard of o2
                    double standardOfO2 = await CalculateO2OfPool(PoolId, volumOfWater);

                    // Check temperature
                    if (waters.Temperature < 6 || waters.Temperature > 32)
                    {
                        waterElementResponseModel.StandardTemperature = waters.Temperature;
                        products.Add(13);
                    }

                    // Check salt
                    if (waters.Salt != standardOfSalt)
                    {   
                        waterElementResponseModel.StandardSalt = standardOfSalt;
                       products.Add(12);
                    }

                    // Check pH
                    if (waters.Ph < 6.5 || waters.Ph > 8.5)
                    {
                        waterElementResponseModel.StandardPH = waters.Ph;
                       products.Add(10);
                    }

                    // Check O2
                    if (waters.O2 != standardOfO2)
                    {
                        waterElementResponseModel.StandardO2 = standardOfO2;
                        products.Add(9);
                    }

                    // Check No2
                    if (waters.No2 != standardOfNo2)
                    {
                        waterElementResponseModel.StandardNo2 = standardOfNo2;
                       products.Add(8);
                    }

                    // Check No3
                    if (waters.No3 != standardOfNo3)
                    {
                        waterElementResponseModel.StandardNo3 = standardOfNo3;
                        products.Add(7);
                    }

                    // Check Po4
                    if (waters.Po4 != standardOfPo4)
                    {
                        waterElementResponseModel.StandardPo4 = standardOfPo4;
                        products.Add(11);
                    }

                    waterElementResponseModel.listProductId = products;
                }
            }
            return waterElementResponseModel;
        }
    }
}
