using AutoMapper;
using BusinessObject.Models;
using BusinessObject.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.IMapperConfig
{
    public class MappingProfile :Profile
    {
        public MappingProfile()
        {
            CreateMap<Member, MemberResponseModel>();
            CreateMap<Blog, BlogResponseModel>();
            CreateMap<Fish, FishResponseModel>();
            CreateMap<Food, FoodResponseModel>();
        }
    }
}
