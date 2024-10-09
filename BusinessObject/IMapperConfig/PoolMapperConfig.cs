using AutoMapper;
using BusinessObject.Models;
using BusinessObject.RequestModel;
using BusinessObject.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.IMapperConfig
{
    public class PoolMapperConfig
    {
        public static void CreateMap(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<Pool, PoolResponseModel>();
        }
    }
}
