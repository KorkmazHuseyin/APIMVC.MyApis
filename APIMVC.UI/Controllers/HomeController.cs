using APIMVC.UI.ApiServices;
using APIMVC.UI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace APIMVC.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        KargoApiService _service;
        TokenApiService _tokenService;
        public HomeController(ILogger<HomeController> logger, KargoApiService service, TokenApiService tokenService)
        {
            _logger = logger;
            _service = service;
            _tokenService = tokenService;
        }



        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Index02()
        {
            var deger = _service.KargolariListele();
            return View(deger);
        }




        /// <summary>
        /// 1.adım  : Önce kullanıcı kayıt sayfasına gideceğiz.
        /// </summary>
        /// <returns></returns>
        /// 

        public IActionResult Index03KullaniciKayit()
        {
            return View();
        }


        /// <summary>
        /// 2.adım   Kullanıcı kayıt sayfasından aldığımız dto bilgileri ile kaydı gerçekleştirmek üzere metodları kullanıyoruz. 
        /// </summary>
        /// <returns></returns>
        /// 

        [HttpPost]
        public async Task<IActionResult> Index03KullaniciKayit(RegisterDTO dto)
        {
            //apiye bağlan register methodunu calistir. 
            var deger = await _tokenService.KullaniciKaydet(dto);
            return View(deger);
        }







        /// <summary>
        ///  Login için 1. adım ///  Login sayfasına giderek sayfayı görüntülüyoruz.
        /// </summary>
        /// <returns></returns>
        public IActionResult IndexLogin()
        {
            if (Request.Cookies["giris"] != null)
            {
                string kaydedilmişCookieBilgisi = Request.Cookies["token"];
            }
            return View(new LoginDTO());
        }



        // View sayfasından gelen, daha önce kaydedilmiş olan username ve Password bilgisi girildikten sonra TokenAl metodu ile giriş yapan kullanıcıya Token alınır.
        [HttpPost]
        public async Task<IActionResult> IndexLogin(LoginDTO dto)
        {
            //tokenal methodunu çalıştır..

            string uretilmisTokenDegeri = await _tokenService.TokenAl(dto.UserName, dto.Password);


            //cookie de tut.

            CookieOptions mycookie = new CookieOptions();
            mycookie.Domain = "giris";
            mycookie.Expires = DateTime.Now.AddDays(1);
            Response.Cookies.Append("token", uretilmisTokenDegeri);



            return View();
        }

        /// <summary>
        /// 3.adım
        /// </summary>

        public async Task<IActionResult> Index03PostWithToken()
        {

            return View();
        }

        // public async Task<IActionResult> Index03PostWithToken1() => View(new ShipperDTO); /// Bunların hepsi Get Actionu için farklı bir yazım şekli
        //public async Task<IActionResult> Index03PostWithToken1() => View();/// Bunların hepsi Get Actionu için farklı bir yazım şekli
        // public async Task<IActionResult> Index03PostWithToken1() => RedirectToAction();/// Bunların hepsi Get Actionu için farklı bir yazım şekli



        [HttpPost]
        public async Task<IActionResult> Index03PostWithToken([Bind("CompanyName,Phone")] ShipperDTO dto)
        {
            if (ModelState.IsValid)
            {
                // apiye dtoyu gönder
                //if (Request.Cookies["Token"]!=null)
                //{
                TempData["servermesaj"] = await _service.TokenShipAddAsync(dto, Request.Cookies["token"]);
                //}
            }
            return RedirectToAction("Index03PostWithToken");
        }


        /// <summary>
        /// Kullanıcı sisteme giriş yapar,
        /// urunleri listeler,
        /// urunlerin birine basıp detay görüntüler,
        /// ürünü beğenirse o üründen istediği adet kadarını sepete atar,
        /// istediğinde sepeti kaydeder,
        /// 
        /// sepeti kaydetme ise Borthwindde yeni bir tablo tutulmalıdır. secure olarak kayt edilmeli. Bu sefer token bilgisi session da tutulmalıdır.
        /// 
        /// Order Details a 10249 Id li fata üzerinden tüm sepet kaydedilmelidir. 
        /// 
        /// Sepet= ID User Id tarih
        /// Sepet Detay =>ıd Sepet Id product ID adet
        /// 
        /// 
        /// 
        /// </summary>
        /// <returns></returns>





















        //~/api/addShip
        [HttpGet]
        public IActionResult Index03Pos()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index03Post([Bind("CompanyName,Phone")] ShipperDTO dto)
        {
            if (ModelState.IsValid)
            {
                //apiye bu dtoyu gonder
                string apidenGelenEnSonCevap = await _service.ViewToAddShip(dto);
            }
            return View("Index03PostWithToken");
        }










        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
