using APIMVC.UI.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace APIMVC.UI.ApiServices
{
    public class KargoApiService
    {

        //  HttpClient Apilere referans asla vermiyoruz.Referanslı işlem Apilerle yapmıyoruz.O nedenle Oluşturduğumuz servisten Apileri kullanabilmek için HttpClient sınıfını kullanıyoruz.

        HttpClient _client;
        public KargoApiService(HttpClient client)
        {
            _client = client;
        }

        public async Task<List<ShipperDTO>> KargolariListele()
        {
            List<ShipperDTO> shipperDTOs = null;
            var donenCevap = await _client.GetAsync("api/Kargo");
            if (donenCevap.IsSuccessStatusCode)
            {
                shipperDTOs = JsonConvert.DeserializeObject<List<ShipperDTO>>(await donenCevap.Content.ReadAsStringAsync());
            }
            return shipperDTOs;
        }

        public async Task<string> ViewToAddShip(ShipperDTO dto)
        {
            var eklenecekIcerik = new StringContent(JsonConvert.SerializeObject(dto));
            eklenecekIcerik.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            string donenVeri = null;
            try
            {
                var donenPostDegeri = await _client.PostAsync("api/addShip", eklenecekIcerik);
                if (donenPostDegeri.IsSuccessStatusCode)
                {
                    donenVeri = await donenPostDegeri.Content.ReadAsStringAsync();
                }
            }
            catch (Exception ex)
            {
                donenVeri = "bir hta oluştu.";
            }
            return donenVeri;
        }

        // Token kullanarak kargo eklemesi yapmak için kullanılacak.
        public async Task<string> TokenShipAddAsync(ShipperDTO dto, string token = null)
        {
            if (token == null)                  // Tokensız geliyorsa!!!
            {
                return "token yok";

            }

            var hede = new StringContent(JsonConvert.SerializeObject(dto));                                 // Dto yu seriliaze ederek hede değişkenine                                                                                                atadık.
            hede.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");    // değişkenimizin header inin                                                                                                               contentype ına application/json diye                                                                                                        ekledik
            _client.DefaultRequestHeaders.Add("Authorization", " Bearer " + token); //  Request e token ekleme kodu. // Requestin header ine Authorization ve " Bearer " ekledik   ki autorize olmuş bearer olduğunu söyledik. beklermiş 
            var donendeger = await _client.PostAsync("api/addShip", hede);
            string veri = " ";
            if (donendeger.IsSuccessStatusCode)
            {
                veri = "veri token bilgisi okunarak eklendi";
            }
            return veri;
        }

    }
}
