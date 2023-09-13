using APIMVC.UI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace APIMVC.UI.ApiServices
{
    public class TokenApiService
    {
        HttpClient _client;
        public TokenApiService(HttpClient client)
        {
            _client = client;
        }


        //Kullanıcı giriş yapıyor. Girmiş olduğu kullanıcı adı ve şifresi ile token alması sağlanıyor.Bu metodla

        public async Task<string> TokenAl(string kullaniciAdi, string sifre)
        {
            LoginDTO dto = new LoginDTO() { Password = sifre, UserName = kullaniciAdi };        // içeri Kullanıcı adı ve şifresi veriliyor.Login dto                                                                                        tipnde alıyorum.

            StringContent mycontent = new StringContent(JsonConvert.SerializeObject(dto));      // aldığım dto yu class olduğu için json a serilize                                                                                       etmem gerekiyor.Serilize ederek json tipinde my                                                                                        content e yüklüyorum

            mycontent.Headers.ContentType = new MediaTypeHeaderValue("application/json");       // Mycontentin header lerına contentType ının                                                                                               applicatin/json tipinde olduğunu sbelirtiyoruz.

            var apininGondermisOlduguDeger = await _client.PostAsync("api/auth/login", mycontent);   // api/auth/login  bu routten gelen birinin                                                                                                verdiği bilgilere gore bir token değeri                                                                                                 oluşuyor. Oluşan token değerini de bir                                                                                                  değişkene atıyoruz.

            string token = "";
            if (apininGondermisOlduguDeger.IsSuccessStatusCode)         ///  Apinin göndermiş olduğu Statuscode true mu diye bakıyoruz.yani takone                                                                  başarılı bir şekilde oluşmuş mu? ona bakıyoruz.
            {
                token = await apininGondermisOlduguDeger.Content.ReadAsStringAsync(); ///apininGondermisOlduguDeger in content alanında Apinin göndermiş olduğu token değeri var string olarak oku benim için diyoruz ve token(değişkene bu ismi verdik. İsim önemli değil.) diye isimlendirdiğimiz değişkene atıyoruz. 
            }
            return token;
        }



        // Kullanıcı kaydı gerçekelştiriyoruz. bu metodla
        public async Task<string> KullaniciKaydet(RegisterDTO dto)
        {
            StringContent mycontent = new StringContent(JsonConvert.SerializeObject(dto));            // Register dto dan gelen username ve password                                                                                              bilgisi serialize edilerek mycontent                                                                                                      değişkenine yüklendi.

            mycontent.Headers.ContentType = new MediaTypeHeaderValue("application/json");               // Mycontent in headerslerine applicatin/json                                                                                               tipinde contentype eklendi

            var apininGondermisOlduguDeger = await _client.PostAsync("api/auth/register", mycontent);   // api/auth/register rotundan gelen mycontent                                                                                           verisi ile postAsync metodu yardımı ile api                                                                                             ye bağlanıp kaydı gerçekleştiriyoruz.                                                                                                  Gerçekleşen kaydı da                                                                                                                     apininGondermisOlduguDeger  isimli bir                                                                                                  değişekene atıyoruz. (İsim önemli değil                                                                                                 her ismi verebilirdik.) 

            if (apininGondermisOlduguDeger.IsSuccessStatusCode)                         // Kayıt başarılı bir şekilde gerçekleştiyse statuscode true                                                                                dönecektir. 
            {
                //201 status kodunun gelip gelmediğini de kontrol edebiliriz. 
                return "giriş yapıldı..";
            }
            return "";                                                                  // Kayıt gerçekleşmediyse birşey dönemeyecek.Boş dönecek daha                                                                                doğrusu
        }



    }
}
