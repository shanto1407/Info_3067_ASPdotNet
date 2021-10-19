﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using ExercisesAPI.DAL;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using ExercisesAPI.DAL.DAO;

namespace ExercisesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataController : ControllerBase
    {
        AppDbContext _ctx;
        IWebHostEnvironment _env;
        public DataController(AppDbContext context, IWebHostEnvironment env)
        {
            _ctx = context;
            _env = env;
        }
        public async Task<ActionResult<String>> Index()
        {
            DataUtility util = new DataUtility(_ctx);
            string payload = "";
            var json = await getMenuItemJsonFromWebAsync();
            try
            {
                payload = (util.loadNutritionInfoFromWebToDb(json)) ? "tables loaded" : "problem loading tables";
            }
            catch (Exception ex)
            {
                payload = ex.Message;
            }
            return JsonSerializer.Serialize(payload);
        }

        [Route("loadcsv")]
        public ActionResult<String> LoadCsv()
        {
            string payload = "";
            StoreDAO dao = new StoreDAO(_ctx);
            bool storesLoaded = dao.LoadCSVFromFile(_env.WebRootPath);
            try
            {
                payload = storesLoaded ? "stores loaded successfully" : "problem loading store data";
            }
            catch (Exception ex)
            {
                payload = ex.Message;
            }
            return JsonSerializer.Serialize(payload);
        }

        private async Task<String> getMenuItemJsonFromWebAsync()
        {
            string url = "https://raw.githubusercontent.com/elauersen/info3067/master/mcdonalds.json";
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(url);
            var result = await response.Content.ReadAsStringAsync();
            return result;
        }

    }
}