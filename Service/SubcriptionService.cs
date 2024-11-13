using BusinessObject.Models;
using BusinessObject.RequestModel;
using Repository.Interface;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class SubcriptionService : ISubcriptionService
    {

        private readonly ISubcriptionRepository _subcriptionRepository;

        public SubcriptionService(ISubcriptionRepository subcriptionRepository)
        {
            _subcriptionRepository = subcriptionRepository;
        }

        public async Task Add(SubcriptionRequest request)
        {
            Subcriptions subcriptions = new Subcriptions
            {
                Duration = request.Duration,
                Name = request.Name,
                Price = request.Price,
            };
            _subcriptionRepository.add(subcriptions);
        }

        public async Task DeleteById(int id)
        {
            var subcriptions = await _subcriptionRepository.GetById(id);
            _subcriptionRepository.delete(subcriptions);
        }

        public async Task<List<Subcriptions>> GetAll()
        {
            return await _subcriptionRepository.GetAll();
        }

        public async Task<Subcriptions> GetById(int id)
        {
            return await _subcriptionRepository.GetById(id);
        }

        public async Task Update(int id, SubcriptionRequest request)
        {
            var subcriptions = await _subcriptionRepository.GetById(id);
            subcriptions.Duration = request.Duration;
            subcriptions.Name = request.Name;
            subcriptions.Price = request.Price;

            _subcriptionRepository.update(subcriptions);
        }
    }
}
