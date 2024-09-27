using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Validation_Handler;
using static System.Net.Mime.MediaTypeNames;

namespace Repository
{
    public class MemberRepository : IMemberRepository
    {
        private readonly KoiCareDBContext _context;

        public MemberRepository(KoiCareDBContext context)
        {
            _context = context;
        }

        List<Member> _members;

        public async Task<List<Member>> GetAllMember()
        {
            return await _context.Members.Include(m => m.Role).ToListAsync();
        }
        public async Task<Member> GetMemberById( int id)
        {
            return await _context.Members.Include(m => m.Role)
                .SingleOrDefaultAsync(m => m.Id.Equals(id));
        }
        public async Task<Member> Login(string email , string password)
        {
            return await _context.Members.Include(m => m.Role)
                .FirstOrDefaultAsync(m => m.Email.Equals(email) && m.Password.Equals(password));
        }
        public async Task Register(Member member)
        {
            member.Password = HashPasswordValidation.HashPasswordToSha256(member.Password);
            _context.Members.Add(member);
            await _context.SaveChangesAsync();

        }
        public async Task<Member> UpdateMember(Member member)
        {
            _context.Entry(member).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return member;
        }
        public async Task<bool> ExistedEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return false; // Hoặc có thể throw một exception tùy thuộc vào yêu cầu của bạn
            }

            return await _context.Members.AnyAsync(m => m.Email.Equals(email));
        }
    }
}
