﻿using BusinessObject.Models;
using BusinessObject.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interface
{
    public interface IMemberRepository : IBaseRepository<Member>
    {
        Task<List<MemberResponseModel>> GetAllMember();
        Task<Member> Login(string email, string password);
        Task Register(Member member);
        Task<Member> UpdateMember(Member member);

        Task<bool> ExistedEmail(string email);
    }
}
