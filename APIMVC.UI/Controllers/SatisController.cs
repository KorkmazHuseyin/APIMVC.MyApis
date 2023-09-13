using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIMVC.UI.Controllers
{
    public class SatisController:Controller
    {






        public IActionResult Index()
        {

            return View();
        }


        //[HttpPost]
        //public async Task<IActionResult> Index(RegisterDTO dto)
        //{
        //    //apiye bağlan register methodunu calistir. 
        //    var deger = await _tokenService.KullaniciKaydet(dto);
        //    return View();
        //}



    }
}
