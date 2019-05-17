using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using URLAnalizer.Models;

namespace URLAnalizer.Controllers
{
    public class HomeController : Controller
    {

        public IActionResult Index()
        {
            Result result = new Result();

            return View(result);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
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


       //--------------------------------------------------------MUSU KODAS----------------------------------------------------------------------------

        public void ReadDataFromFile(List<URLData> DataFromFile)
        {
            bool firstLine = true;


            string[] lines = System.IO.File.ReadAllLines("~/../Data/dataset.txt");
            foreach(string line in lines)
            {
                if (firstLine)
                {
                    firstLine = false;
                    continue;
                }

                string[] s = line.Split('\t');
                DataFromFile.Add(new URLData(int.Parse(s[1]), int.Parse(s[2]), int.Parse(s[3]), int.Parse(s[4]), int.Parse(s[5]), int.Parse(s[6]), int.Parse(s[7]), int.Parse(s[8]),
                    int.Parse(s[9]), int.Parse(s[10]), int.Parse(s[11])));
            }
        }

        [HttpPost]
        public ActionResult Index(string URL)
        {
            string u = URL;
            int prediction = 1;

            List<URLData> DataFromFile = new List<URLData>();

            if (DataFromFile.Count == 0)
                ReadDataFromFile(DataFromFile);

            //URL = page url from text field
            //Do prediction here

            Result result = new Result(u, prediction);
            return View(result);
        }


        public ActionResult AddToFile(string URL, int prediction)
        {
            //prob read file again

            //check if it dosent exist already

            //if not add to file

            Result result = new Result(URL, prediction);
            return View("Index", result);
        }

    }
}
