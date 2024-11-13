using AutoMapper;
using BusinessObject.IMapperConfig;
using BusinessObject.Models;
using BusinessObject.ResponseModel;
using Microsoft.EntityFrameworkCore;
using Repository.Interface;
using System.Text;
using System.Security.Cryptography;

namespace Repository
{
    public class MemberRepository : BaseRepository<Member>, IMemberRepository
    {
        private readonly KoiCareDBContext _context;

        public MemberRepository(KoiCareDBContext context) : base(context)
        {
            _context = context;
        }

        List<Member> _members;

        public async Task<List<MemberResponseModel>> GetAllMember()
        {
            List<Member> members = await _context.Members.Include(m => m.Role)
                .Include(m => m.UserSubcriptions)
                .ToListAsync();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile());
            });
            var mapper = config.CreateMapper();

            List<MemberResponseModel> _member = members.Select(mem => mapper.Map<Member, MemberResponseModel>(mem)).ToList();

            return _member;
        }

        public async Task<Member> Login(string email, string password)
        {
            password = HashPasswordToSha256(password);
            return await _context.Members
                .Include(m => m.Role)
                .Include(m => m.UserSubcriptions)
                .FirstOrDefaultAsync(m => m.Email.Equals(email) && m.Password.Equals(password));
        }
        public async Task Register(Member member)
        {
            member.Password = HashPasswordToSha256(member.Password);
            _context.Members.Add(member);
            await _context.SaveChangesAsync();

        }
        public async Task<Member> UpdateMember(Member member)
        {
            _context.Entry(member).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return member;
        }
        public async Task<Member> ExistedEmail(string email)
        {
            return await _context.Members
                .Include(m => m.Role)
                .Include(m => m.UserSubcriptions)
                .FirstOrDefaultAsync(m => m.Email.Equals(email));
        }

        public string HashPasswordToSha256(string password)
        {
            using var sha256 = SHA256.Create();
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            var sb = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                sb.Append(bytes[i].ToString("x2"));
            }
            return sb.ToString();
        }

        public async Task<Member> CreateMemberByGoogleAccount(string accountEmail, string accountName)
        {
            Member member = new Member();
            member.Email = accountEmail;
            member.FullName = accountName;
            member.RoleId = 4;
            member.Password = "1";
            _context.Members.Add(member);
            await _context.SaveChangesAsync();
            return member;
        }
    }
}
