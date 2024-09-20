using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface IMemberService
    {
        Task<List<Member>> GetAllMember();
        Task<Member> GetMemberById(int id);
        Task<Member> Login(string email, string password);
        Task Register(Member member);
        Task<Member> UpdateMember(Member member);
    }
}
