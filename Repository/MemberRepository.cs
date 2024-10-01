using AutoMapper;
using BusinessObject.IMapperConfig;
using BusinessObject.Models;
using BusinessObject.ResponseModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Validation_Handler;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.Extensions.Options; 

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

        public async Task<List<MemberResponseModel>> GetAllMember()
        {
            List<Member> members = await _context.Members.Include(m => m.Role).ToListAsync();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile());
            });
            var mapper = config.CreateMapper();

            List<MemberResponseModel> _member = members.Select(mem => mapper.Map<Member, MemberResponseModel>(mem)).ToList();

            return _member;
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
                return false;
            }

            return await _context.Members.AnyAsync(m => m.Email.Equals(email));
        }



    }
}
