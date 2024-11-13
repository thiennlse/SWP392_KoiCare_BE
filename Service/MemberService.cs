using BusinessObject.Models;
using BusinessObject.ResponseModel;
using Microsoft.AspNetCore.Http;
using Repository.Interface;
using Service.Interface;
using System.Security.Claims;

namespace Service
{
    public class MemberService : IMemberService
    {
        private readonly IMemberRepository _memberRepository;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ISubcriptionRepository _subcriptionRepository;
        public MemberService(IMemberRepository memberRepository, IHttpContextAccessor contextAccessor, ISubcriptionRepository subcriptionRepository)
        {
            _memberRepository = memberRepository;
            _contextAccessor = contextAccessor;
            _subcriptionRepository = subcriptionRepository;
        }

        public async Task<Member> ExistedEmail(string email)
        {
            return await _memberRepository.ExistedEmail(email);
        }

        public async Task<List<MemberResponseModel>> GetAllMember()
        {
            return await _memberRepository.GetAllMember();
        }

        public async Task<Member> GetMemberById(int id)
        {
            return await _memberRepository.GetById(id);
        }

        public async Task<Member> Login(string email, string password)
        {
            return await _memberRepository.Login(email, password);
        }

        public async Task Register(Member member)
        {
            await _memberRepository.Register(member);
        }

        public async Task<Member> UpdateMember(Member member)
        {
            return await _memberRepository.UpdateMember(member);
        }

        public async Task<Member> CreateMemberByGoogleAccount(string accountEmail, string accountName)
        {
            return await _memberRepository.CreateMemberByGoogleAccount(accountEmail, accountName);
        }

        public async Task AddSubcriptionForMember(int planId)
        {
            var user = _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userid = int.Parse(user);
            var subcription = await _subcriptionRepository.GetById(planId);
            var member = await _memberRepository.GetById(userid);
            var existSubcription = member.UserSubcriptions?
                .Where(s => s.EndDate > DateTime.Now)
                .ToList();
            if (!existSubcription.Any())
            {
                var newSubcription = new UserSubcriptions
                {
                    StartDate = DateTime.Now.ToUniversalTime(),
                    EndDate = DateTime.Now.AddMonths(subcription.Duration).ToUniversalTime(),
                    UserId = userid,
                    SubcriptionId = planId
                };
                member.UserSubcriptions.Add(newSubcription);
                await _memberRepository.UpdateMember(member);
            }
            else
            {
                throw new Exception("Already have subcription");
            }
        }
    }
}
