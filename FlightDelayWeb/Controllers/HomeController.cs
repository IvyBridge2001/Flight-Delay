using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using FlightDelayWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using python_test.Models;

namespace python_test.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            Graphs graph = new Graphs();
            ViewBag.Label1 = "01/01";
            ViewBag.Label2 = "01/02";
            ViewBag.Label3 = "01/03";
            ViewBag.Label4 = "01/04";
            ViewBag.Label5 = "01/05";
            ViewBag.Label6 = "01/06";
            ViewBag.Label7 = "01/07";

            ViewBag.Prob1 = 5.9;
            ViewBag.Prob2 = 8.9;
            ViewBag.Prob3 = 8.9;
            ViewBag.Prob4 = 8.9;
            ViewBag.Prob5 = 9.9;
            ViewBag.Prob6 = 8.9;
            ViewBag.Prob7 = 3.9;            //graph.Labels = "['a','b','c','d','e','f','g']";
            //graph.Labels = "[1,2,3,4,5,6,7]";
            graph.Date = DateTime.UtcNow;
            graph.Date = DateTime.UtcNow;
            graph.Time = "16";
            return View(graph);
        }

        [HttpPost]
        public async Task<IActionResult> Index(Graphs graph)
        {
            string dateFormated = graph.Date.ToString("ddMMyyyy");
            dateFormated = dateFormated.Replace("/", "");
            ViewBag.Label1 = "01/01";
            ViewBag.Label2 = "01/02";
            ViewBag.Label3 = "01/03";
            ViewBag.Label4 = "01/04";
            ViewBag.Label5 = "01/05";
            ViewBag.Label6 = "01/06";
            ViewBag.Label7 = "01/07";
            ViewBag.Prob1 = 8.9;
            ViewBag.Prob2 = 8.9;
            ViewBag.Prob3 = 8.9;
            ViewBag.Prob4 = 8.9;
            ViewBag.Prob5 = 8.9;
            ViewBag.Prob6 = 8.9;
            ViewBag.Prob7 = 8.9;
            try
            {
                using (var httpClient = new HttpClient())
                {
                    using (var response = await httpClient.GetAsync("http://127.0.0.1:5000/probability/" + dateFormated + "/" + graph.Time + "/" + graph.Origin.ToString() + "/" + graph.Destination.ToString()))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        var reservationList = JsonConvert.DeserializeObject<jsonResponse>(apiResponse);
                        graph.Probabilities = reservationList.Probabilities;
                        graph.Labels = reservationList.Labels;
                        ViewBag.Label1 = graph.Labels[0];
                        ViewBag.Label2 = graph.Labels[1];
                        ViewBag.Label3 = graph.Labels[2];
                        ViewBag.Label4 = graph.Labels[3];
                        ViewBag.Label5 = graph.Labels[4];
                        ViewBag.Label6 = graph.Labels[5];
                        ViewBag.Label7 = graph.Labels[6];
                        ViewBag.Prob1 = graph.Probabilities[0];
                        ViewBag.Prob2 = graph.Probabilities[1];
                        ViewBag.Prob3 = graph.Probabilities[2];
                        ViewBag.Prob4 = graph.Probabilities[3];
                        ViewBag.Prob5 = graph.Probabilities[4];
                        ViewBag.Prob6 = graph.Probabilities[5];
                        ViewBag.Prob7 = graph.Probabilities[6];
                    }
                }

            }
            catch (Exception e)
            {
                var Error1 = e;
            }
            return View(graph);
        }

        //[HttpPatch]
        //[Route("Home/Index/{date}/{time}/{dest}/{orig}")]
        //public async Task<IActionResult> Index(string date, string time, string dest, string orig)
        //{
        //    //DateTime date = Convert.ToDateTime(day + "/" + month + "/" + year + " "+ time + ":00:00");
        //    //string strDate = date.ToString();
        //    Graphs graph = new Graphs();
        //    try
        //    {
        //        using (var httpClient = new HttpClient())
        //        {
        //            using (var response = await httpClient.GetAsync("http://127.0.0.1:5000/probability/" +date+"/" + time + "/" + dest + "/" + orig))
        //            {
        //                string apiResponse = await response.Content.ReadAsStringAsync();
        //                var reservationList = JsonConvert.DeserializeObject<float[]>(apiResponse);
        //                graph.Probabilities = reservationList;
        //                graph.Labels = new string[] { "a", "b", "c", "d", "e", "f" };
        //            }
        //        }

        //    }
        //    catch (Exception e)
        //    {
        //        var Error1 = e;
        //    }
        //    return View(graph);
        //}
        public IActionResult Componentes()
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
