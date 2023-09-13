using APIMVC.MyApis.DAL.AuthDAL.Interfaces;
using APIMVC.MyApis.DAL.Entitites;
using APIMVC.MyApis.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace APIMVC.MyApis.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        //DI
        IAuthDAL _authDAL;
        IConfiguration _conf;
        public AuthController(IAuthDAL authDAL, IConfiguration conf)
        {
            _authDAL = authDAL;
            _conf = conf;
        }
        // Kullanıcının kaydını yapıyoruz burda
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO dto) // RegisterDTO oluşturduk. Username ve passwod istiyoruz çünki
        {
            if (!await _authDAL.UserExists(dto.UserName))
            {
                ModelState.AddModelError("not valid", "zaten varsın"); // Kişini kullanıcızaten kayıtlı diye uyarı veriyoruz burda
            }

            if (!ModelState.IsValid)  // Kişi kayıtlı olabilir ama datada istediğim bilgi olmaya bilir. Passwordu yanlıştır mesela
            {
                return BadRequest();
            }
            var kisi = await _authDAL.Register(new User() { UserName = dto.UserName }, dto.Password); // Kişi yoktu kayıt burda gerçekleşti.

            return StatusCode(201);

        }






        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO dto)// Login işlemler burda kontrol ediliyor. 
        {
            var bulunanUser = await _authDAL.Login(dto.UserName, dto.Password);
            if (bulunanUser == null)
            {
                //return null;
                return BadRequest();   //   Kişiden Username ve Password istendi. Eğer böyle bir kişi yoksa BadRequest döndü.
            }
            else
            {
                //Kişi varmış bulduk. şimdi onun Kullanıcı adı ve Password ü ile token oluşturmaya geldi sıra

                // SecurityTokenDescriptor  sınıfı ile nasıl bir token oluşturulacağının bilgilerini oluşturuyoruz.
                var desc = new SecurityTokenDescriptor()
                {
                    Expires = DateTime.Now.AddDays(1),                                      /// Token ın geçerlilik süresi
                    Subject = new System.Security.Claims.ClaimsIdentity(new Claim[] {      // Konu kısmında kişiyi özelleştiren
                        new Claim(ClaimTypes.NameIdentifier,bulunanUser.UserID.ToString()),//ne varsa yazabiliriz. Doğal olarak
                        new Claim(ClaimTypes.Name,bulunanUser.UserName)                    //kişiye özgü güvenlik seviyesi                                                                               yüksek bir tokun oluşmasını sağlarız.                                                                        Ne kadar çok bilgi o kadar iyidir.
                        //new Claim("tel","05065656323"),//
                    }),
                    //Kişinin en özel bilgisi Password ü. Aşağıda o passwordün ne şekilde olacağı yani nasıl bir güvenlik algoritması ile kriptolanacağı nı kodluyoruz. Birtane AppSettings te Hüsamettinimiz vardı. secret key olarak kullancağımız aslında password e o key i ekliyoruz aşağıdaki satırlar ama nasıl. Sha512 algoritması ile.

                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_conf.GetSection("AppSettings:Token").Value)), SecurityAlgorithms.HmacSha512Signature)
                };

                // token oluşturabilmek içn AspNetCore Authentication.JwtBearer nudget ini indirmek gerekiyor. İndirince JwtSecurityTokenHandler sınıfını kullanabiliyor. Bu sınıf yardımı ile Token oluşturabiliyoruz.

                var tokenHandler = new JwtSecurityTokenHandler();                       // Newledik
                var token = tokenHandler.CreateToken(desc);                          // Yukarıda yazdığımız desc ile tuttuğumuz kişiye                                                                                             özel hale getirilmiş bilgiler ile CreatToken                                                                                              metodunu kullanarak token oluşturuyoruz.
                var donulecekTokenDegeri = tokenHandler.WriteToken(token);               // oluşturduğumuz token ı da geriye dönülecekdeğer                                                                                            olarak değişkende tutup return edebiliyoruz.
                return Ok(donulecekTokenDegeri);

            }

        }

    }
}
