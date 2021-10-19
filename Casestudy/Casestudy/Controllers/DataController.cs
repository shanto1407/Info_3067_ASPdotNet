using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using Casestudy.DAL;
using System.Text.Json;

namespace Casestudy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataController : ControllerBase
    {
        AppDbContext _ctx;
        public DataController(AppDbContext context) // injected here
        {
            _ctx = context;
        }
        public async Task<ActionResult<String>> Index()
        {
            DataUtility util = new DataUtility(_ctx);
            string payload = "";
            var json = await getProductsItemAsync();
            try
            {
                payload = (util.loadProductFromWebToDb(json)) ? "tables loaded" : "problem loading tables";
            }
            catch (Exception ex)
            {
                payload = ex.Message;
            }
            return JsonSerializer.Serialize(payload);
        }
        private async Task<String> getProductsItemAsync()
        {
            string url = "https://raw.githubusercontent.com/shanto1407/info3067/master/myProducts.json";
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(url);
            var result = await response.Content.ReadAsStringAsync();
            return result;
        }


    }
}