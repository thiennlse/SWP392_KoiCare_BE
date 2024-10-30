using BusinessObject.Models;
using BusinessObject.ResponseModel;
using Repository.Interface;
using Service.Interface;

namespace Service
{
    public class MemberService : IMemberService
    {
        private readonly IMemberRepository _memberRepository;

        public MemberService(IMemberRepository memberRepository)
        {
            _memberRepository = memberRepository;
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
    }
}
