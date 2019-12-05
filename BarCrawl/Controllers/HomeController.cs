﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BarCrawl.Models;
using System.IO;
using System.Net;
using Newtonsoft.Json.Linq;
using System.Net.Http;

namespace BarCrawl.Controllers
{
    public class HomeController : Controller
    {

        public List<YelpModel> GetBars()
        {
            HttpWebRequest request = WebRequest.CreateHttp("https://api.yelp.com/v3/businesses/search?term=bars&location=grand rapids");
            request.Headers.Add("Authorization", "Bearer 5AZ1TMhzZzb52DbbAMkydLPjNRSURY3x-DtC2o7qDjNTa2n96PSxuLZMmQoBy3WtX5q4EWUh4KQWVG1GG_nq_x2YLEssXjh5WF5kYw8E_VPmyRVMRfDHLwOYM0bXXXYx");
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader rd = new StreamReader(response.GetResponseStream());
            string ApiText = rd.ReadToEnd();
            JToken tokens = JToken.Parse(ApiText);

            List<JToken> ts = tokens["businesses"].ToList();

            List<YelpModel> yelp = new List<YelpModel>();

            foreach (JToken t in ts)
            {
                YelpModel a = new YelpModel(t);
                yelp.Add(a);
            }

            return yelp;
        }
        public IActionResult Result()
        {
            List<YelpModel>bars = GetBars();
            return View(bars);
        }
        public List<YelpModel> Start()
        {
            List<YelpModel> bars = GetBars();
            LongAndLat a = new LongAndLat();
            decimal latitude = a.Latitude;
            decimal longitude = a.Longitude;

            HttpWebRequest request = WebRequest.CreateHttp($"https://api.yelp.com/v3/businesses/search?term=bars&location=grand rapids&radius=1000&longitude={longitude}&latitude={latitude}");
            request.Headers.Add("Authorization", "Bearer 5AZ1TMhzZzb52DbbAMkydLPjNRSURY3x-DtC2o7qDjNTa2n96PSxuLZMmQoBy3WtX5q4EWUh4KQWVG1GG_nq_x2YLEssXjh5WF5kYw8E_VPmyRVMRfDHLwOYM0bXXXYx");
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader rd = new StreamReader(response.GetResponseStream());
            string ApiText = rd.ReadToEnd();
            JToken tokens = JToken.Parse(ApiText);

            List<JToken> ts = tokens["businesses"].ToList();

            List<YelpModel> longlat = new List<YelpModel>();

            foreach (JToken t in ts)
            {
                YelpModel b = new YelpModel(t);
                longlat.Add(b);
            }

            return longlat;
           
        }

        public List<GoogleMapsModel> GoogleMapsAPI(string Start, string End, string Mode)

        {
            string key = "AIzaSyBnmEUMhQ - ijIZk1r3QUvbWkOwBQ2v2KcU";
            HttpWebRequest request = WebRequest.CreateHttp($"https://maps.googleapis.com/maps/api/directions/json?origin={Start}&destination={End}+&key={key}&mode={Mode}");
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            //This will convert the response to a string
            StreamReader rd = new StreamReader(response.GetResponseStream());
            string APIText = rd.ReadToEnd();
            JToken t = JToken.Parse(APIText);

            List<JToken> tt = t["routes"].ToList();

            List<GoogleMapsModel> Google = new List<GoogleMapsModel>();

            foreach (JToken f in tt)
            {
                GoogleMapsModel d = new GoogleMapsModel(f);
                Google.Add(d);
            }

            return Google;


        }

        public IActionResult RouteView(string Start, string End, string Mode)
        {
            List<GoogleMapsModel> Go = GoogleMapsAPI(Start, End, Mode);
            return View(Go);
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Search()
        {
            return View();
        }
       [HttpPost]
       public IActionResult Search(YelpModel a)
        {
            return RedirectToAction("Home", "Result");
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
