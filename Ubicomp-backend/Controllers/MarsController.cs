using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Ubicomp_backend.APIModels;
using Ubicomp_backend.Env;


namespace Ubicomp_backend.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class MarsController
    {    

        private HttpClient client;
        private ApiKeys _api;


        public MarsController()
        {
            _api = new ApiKeys();
            client = new HttpClient();
        }
    

        public async Task<solLocation[]> getLocationes()
        {
            string resultContentLoc = "";
            
            HttpResponseMessage locationresponse = await client.GetAsync(_api.PositionApi);

            if (locationresponse.IsSuccessStatusCode)
            {
                resultContentLoc = await locationresponse.Content.ReadAsStringAsync();
            }

            var json = JObject.Parse(resultContentLoc);
            var feat = json?["features"]?? "";
            return feat.ToObject<solLocation[]>();

        }

        public async Task<solTemperature[]> GetWeather()
        {

            string resultContentWeath = "";
            
            HttpResponseMessage weatherresponse = await client.GetAsync(_api.WeatherApi);

            if (weatherresponse.IsSuccessStatusCode)
            {
                resultContentWeath =  await weatherresponse.Content.ReadAsStringAsync();
            }

            var json = JObject.Parse(resultContentWeath);
            var feat = json?["soles"]?? "";
            return feat.ToObject<solTemperature[]>();

        }



        [Route("GetAll")]
        public async Task<List<solTemperature>> GetAll()
        {

            var locationes = await getLocationes();
            var weather = await GetWeather();

            var soles = (
                    from w in weather
                    select new solTemperature {
                        terrestrial_date = w.terrestrial_date,
                        sol = w.sol,
                        min_temp = w.min_temp,	
                        max_temp = w.max_temp,
                        pressure = w.pressure,	
                        solLocation = (from l in locationes where l.properties.sol==w.sol select l)
                        .FirstOrDefault()
                        }).ToList();

            return soles;

        }




        [Route("GetMarsImage/{date}")]
        public async Task<dynamic> GetMarsImage(string date)
        {

            string resultContentWeath = "";
            string url = _api.GetPhoto + "?earth_date=" + date + "&api_key=" + _api.key + "&camera=NAVCAM";
            
            HttpResponseMessage weatherresponse = await client.GetAsync(url);

            if (weatherresponse.IsSuccessStatusCode)
            {
                resultContentWeath =  await weatherresponse.Content.ReadAsStringAsync();
            }

            var json = JObject.Parse(resultContentWeath);
  
  
            var feat = json?["photos"] ?? "";
            var img = feat[0]?["img_src"] ?? ""; 

            return img.ToObject<string>();

        }






}

}