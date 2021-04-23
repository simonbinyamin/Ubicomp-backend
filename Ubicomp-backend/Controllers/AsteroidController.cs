using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Ubicomp_backend.Env;

[ApiController]
[Route("[controller]")]
public class AsteroidController
{
    private HttpClient client;
    private ApiKeys _api;


    public AsteroidController()
    {
        _api = new ApiKeys();
        client = new HttpClient();
    }

    [Route("GetAll")]
    public async Task<string> GetAll()
    {
        string today = DateTime.Now.ToString("yyyy-MM-dd");
        string yesterday = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
        string resultContent = "";
        string url = _api.GetNearByObjects + "?start_date=" + yesterday + "&end_date=" + today + "&api_key=" + _api.key;

        HttpResponseMessage response = await client.GetAsync(url);
        if (response.IsSuccessStatusCode)
        {
            resultContent = await response.Content.ReadAsStringAsync();
        }

        var json = JObject.Parse(resultContent);

        var nearearth = json?["near_earth_objects"]?[today] ?? json?["near_earth_objects"]?[yesterday]
                        ?? "";

        foreach (var section in nearearth)
        {
            section.First.Remove();
        }

        return nearearth.ToString();
    }
}
