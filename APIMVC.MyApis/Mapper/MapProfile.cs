using APIMVC.MyApis.DAL.Entitites;
using APIMVC.MyApis.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIMVC.MyApis.Mapper
{
    public class MapProfile:Profile
    {
        public MapProfile()
        {
            CreateMap<Shippers, ShipperDTO>();
            CreateMap<ShipperDTO, Shippers>();
          
        }
    }
}
