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

            List<int> indexes = PhishingIndexes(URL);
            //URL = page url from text field
            //Do prediction here

            Result result = new Result(u, prediction);
            return View(result);
        }

        public List<int> PhishingIndexes(string URL)
        {
            List<int> indexes = new List<int>();

            string newURL = "";
            Uri uriURL;
            if (Uri.IsWellFormedUriString(URL, UriKind.Absolute) == false)                      // adds https:// if necessary
            {
                newURL = "https://" + URL;
                uriURL = new Uri(newURL);
            }
            else
            {
                newURL = URL;
                uriURL = new Uri(URL);
            }

            string[] parts = uriURL.Authority.Split('.');                                       // splits domain into parts to check if its an IP
            int digitCount = 0;
            if (parts.Length >= 4)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (System.Text.RegularExpressions.Regex.IsMatch(parts[i], @"^\d+$"))       // if is a digit
                        digitCount++;
                }
            }

            if (digitCount == 4)                                                                // 1 Having_IPhaving_IP_Address
                indexes.Add(-1);
            else
                indexes.Add(1);

            if (newURL.Length >= 75)                                                            // 2 URL_Length 
                indexes.Add(-1);
            else
                indexes.Add(1);

            if (newURL.Contains('@'))                                                           // 3 Having_At_Symbol
                indexes.Add(-1);
            else
                indexes.Add(1);

            string path = uriURL.AbsolutePath;                                                  // 4 Double_slash_redirecting
            string str = "";
            if (path.Length >= 2)
                str = path.Substring(0, 2);
            if (str == "//")
                indexes.Add(-1);
            else
                indexes.Add(1);

            if (uriURL.Authority.Contains('-'))                                                 // 5 Prefix_Suffix
                indexes.Add(-1);
            else
                indexes.Add(1);

            var host = new System.Uri(newURL).Host;
            int index = host.LastIndexOf('.'), last = 3;
            while (index > 0 && index >= last - 3)                                              // gets subdomain 
            {
                last = index;
                index = host.LastIndexOf('.', last - 1);
            }
            var domain = host.Substring(index + 1);
            int dotCount = 0;
            for (int i = 0; i < domain.Length; i++)
            {
                if (domain[i] == '.')
                    dotCount++;
            }
            if (dotCount > 2)                                                                   // 6 Having_Sub_Domain 
                indexes.Add(-1);
            else
                indexes.Add(1);

            if (uriURL.Authority == "bit.ly")                                                   // 7 Shortining_Service (SITAS VIETOJ Domain_registeration_length)
                indexes.Add(-1);
            else
                indexes.Add(1);

            int[] goodPorts = { 21, 22, 23, 80, 443, 445, 1433, 1521, 3306, 3389 };             // legit ports
            if (goodPorts.Contains(uriURL.Port) == false)                                       // 8 Port
                indexes.Add(-1);
            else
                indexes.Add(1);

            if (newURL.Contains("mail()") || newURL.Contains("mailto:"))                        // 9 Submitting_to_email (VIETOJ SFH)
                indexes.Add(-1);
            else
                indexes.Add(1);

            if (uriURL.Authority.Contains("https"))                                             // 10 HTTPS_token (VIETOJ Links_in_tags)
                indexes.Add(-1);
            else
                indexes.Add(1);


            indexes.Add(-1);                                                                    // Arturas liepe sita pridet kazkam

            return indexes;
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
