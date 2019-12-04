using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BarCrawlMine.Models;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;

namespace BarCrawlMine.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public List<Bar> GetBars(string location)
        {
            HttpWebRequest request = WebRequest.CreateHttp($"https://api.yelp.com/v3/businesses/search?term=bars&location={location}");
            request.Headers.Add("Authorization", "Bearer 5AZ1TMhzZzb52DbbAMkydLPjNRSURY3x-DtC2o7qDjNTa2n96PSxuLZMmQoBy3WtX5q4EWUh4KQWVG1GG_nq_x2YLEssXjh5WF5kYw8E_VPmyRVMRfDHLwOYM0bXXXYx");
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader rd = new StreamReader(response.GetResponseStream());
            string ApiText = rd.ReadToEnd();
            JToken tokens = JToken.Parse(ApiText);

            List<JToken> ts = tokens["businesses"].ToList();
            List<Bar> barList = new List<Bar>();

            foreach(JToken t in ts)
            {
                Bar b = new Bar(t);
                barList.Add(b);
            }

            return barList;
        }



        public IActionResult Search()
        {
            return View();
        }



        public IActionResult Result(string location)
        {
            List<Bar> bars = GetBars(location);
            return View(bars);
        }

        public IActionResult Privacy()
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
