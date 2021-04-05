using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Ubicomp_backend.Env;

namespace Ubicomp_backend.Controllers
{



    [ApiController]
    [Route("[controller]")]
    public class AsteroidController
    {
        static HttpClient client;
        private ApiKeys _api;


        public AsteroidController()
        {
            _api = new ApiKeys();
            client = new HttpClient();
        }
       
        [Route("GetAll")]
        public async Task<object> GetAll(){
  
    
            string resultContent="";
            string url = _api.GetNearByObjects + "?start_date="+ DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd")+"&end_date="+ DateTime.Now.ToString("yyyy-MM-dd")+"&api_key="+_api.key;
            HttpResponseMessage response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                resultContent= await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            }
            return resultContent = resultContent.Replace(_api.key, "");
        }

    }


}