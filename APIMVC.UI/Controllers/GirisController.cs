using APIMVC.UI.ApiServices;
using APIMVC.UI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIMVC.UI.Controllers
{
    public class GirisController:Controller
    {
        private readonly TokenApiService _service;
        public GirisController(TokenApiService service)
        {
            _service = service;
        }




        public async Task<IActionResult> Index()
        {
            return View();
        }





        [HttpPost]
        public async Task<IActionResult> Index(LoginDTO dto)
        {
            var token = await _service.TokenAl(dto.UserName, dto.Password);

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Error", "Home");
            }
            else
            {
                // CookieBuilder yazılmalıydı.
                HttpContext.Session.SetString("mytoken", token);
                HttpContext.Session.SetString("myuser", dto.UserName);

                return View();
            }




        }


    }
}
