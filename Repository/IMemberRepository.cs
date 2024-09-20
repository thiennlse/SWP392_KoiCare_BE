using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface IMemberRepository
    {
        Task<List<Member>> GetAllMember();
        Task<Member> GetMemberById(int id);
        Task<Member> Login(string email, string password);
        Task Register(Member member);
        Task<Member> UpdateMember(Member member);
    }
}
