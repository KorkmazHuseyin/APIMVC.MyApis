using APIMVC.MyApis.DAL.Context;
using APIMVC.MyApis.DAL.Entitites;
using APIMVC.MyApis.Models;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIMVC.MyApis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KargoController : ControllerBase
    {
        MyContext _context;
        IMapper _mapper;
        public KargoController(MyContext context,IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }
        [HttpGet("")]
        public IActionResult Get()
        {
           var hede= _context.Shippers.ToList();
            return Ok(hede.Select(a=> new ShipperDTO() { 
            ShipperID=a.ShipperID,
            CompanyName=a.CompanyName,
            Phone=a.Phone
            }).ToList());
        }
        [HttpPost]
        [Route("~/api/addShip")]
        public IActionResult Post([FromBody]ShipperDTO dto)
        {            
                                              //Neye dönüştüreyim shippers e
            _context.Shippers.Add(_mapper.Map<Shippers>(dto));
            _context.SaveChanges();                     // Neyi dönüştireyim dto yu
            return Ok("Kayıt başarılı");
        }
    }
}
