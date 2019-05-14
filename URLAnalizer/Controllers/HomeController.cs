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
        public List<URLData> DataFromFile = new List<URLData>();


        public IActionResult Index()
        {
            if (DataFromFile.Count == 0)
                ReadDataFromFile();

            foreach(URLData d in DataFromFile)
                Console.WriteLine("Reultatas" + d.Result);


            return View();
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

        public void ReadDataFromFile()
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



    }
}
