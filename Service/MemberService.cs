﻿using BusinessObject.Models;
using BusinessObject.ResponseModel;
using Repository.Interface;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class MemberService : IMemberService
    {
        private readonly IMemberRepository _memberRepository;

        public MemberService(IMemberRepository memberRepository)
        {
            _memberRepository = memberRepository;
        }

        public async Task<bool> ExistedEmail(string email)
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
    }
}
