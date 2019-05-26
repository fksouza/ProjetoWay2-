using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace FootballLeague.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public IActionResult Index(string ano)
        {

            if (ano != null)
            {
                //Declaração da lista
                JsonResult league = OnGet(ano);

                ViewBag.list = league.Value;
            }

            return View();
        }

        [HttpPost]
        public IActionResult Search(string ano)
        {

            //Declaração da lista
            JsonResult league = OnGet(ano);

            return RedirectToAction(nameof(Index));
        }

        public JsonResult OnGet(string ano)
        {
            try
            {
                using (var client = new HttpClient())
                {

                    string baseURL = "http://api.football-data.org/v2/competitions/";

                    string key = "16f08e0b47af4d9ea712832b82add290";

                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Add("X-Auth-Token", key);
                    client.DefaultRequestHeaders.Accept.Add(
                        new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage response = client.GetAsync(baseURL + ano + "/standings").Result;

                    response.EnsureSuccessStatusCode();
                    string conteudo =
                        response.Content.ReadAsStringAsync().Result;

                    var resultado = JsonConvert.DeserializeObject(conteudo);

                    return Json(resultado);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }

    }
}