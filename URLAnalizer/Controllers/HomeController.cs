using System;
using System.Collections;
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

        public void ReadDataFromIntegers(List<int[]> DataFromFile)
        {
            bool firstLine = true;


            string[] lines = System.IO.File.ReadAllLines("~/../Data/cleanedData.txt");
            foreach(string line in lines)
            {
                if (firstLine)
                {
                    firstLine = false;
                    continue;
                }

                string[] s = line.Split('\t');

                DataFromFile.Add(new int[]{ int.Parse(s[1]), int.Parse(s[2]), int.Parse(s[3]), int.Parse(s[4]), int.Parse(s[5]), int.Parse(s[6]), int.Parse(s[7]), int.Parse(s[8]),
                    int.Parse(s[9]), int.Parse(s[10]), int.Parse(s[11]) });
            }
        }

        public void ReadDataFromURL(Dictionary<string, string> URLs)
        {
            bool firstLine = true;
            string[] lines = System.IO.File.ReadAllLines("~/../Data/URLs.txt");
            foreach (string line in lines)
            {
                if (firstLine)
                {
                    firstLine = false;
                    continue;
                }

                string[] s = line.Split('\t');
                if (!URLs.ContainsKey(s[0]))
                    URLs.Add(s[0], s[1]);

            }
        }

        [HttpPost]
        public ActionResult Index(string URL)
        {
            Dictionary<string, string> URLs = new Dictionary<string, string>();
            ReadDataFromURL(URLs);

            if (URLs.ContainsKey(URL))
            {
                if (URLs[URL] == "bad")
                    return View(new Result(URL, -1));
                else
                    return View(new Result(URL, 1));
            }
            else
            {
                List<double> indexes = PhishingIndexes(URL);

                int prd = SVMpredicate(indexes);
                
                return View(new Result(URL, prd));
            }
        }

        private int SVMpredicate( List<double> x)
        {
            List<double> w = new List<double> { 3.00711980577991, 1.93769457425321, 1.92729411813324, 0.880188320237666, 7.93449186648432, 5.46749683263913, -0.280493231161658, -1.70262591713867, 0.850931284532477, 6.46162051417157, -11.8214927946623 };
            double dotProduct = x.Zip(w, (d1, d2) => d1 * d2).Sum();
            if ( dotProduct < 0)
            {
                return 1;
            }
            else
            {
                return -1;
            }
        }

        public List<double> PhishingIndexes(string URL)
        {
            List<double> indexes = new List<double>();

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
                indexes.Add(1);
            else
                indexes.Add(-1);

            if (newURL.Length >= 75)                                                            // 2 URL_Length 
                indexes.Add(1);
            else
                indexes.Add(-1);

            if (newURL.Contains('@'))                                                           // 3 Having_At_Symbol
                indexes.Add(1);
            else
                indexes.Add(-1);

            string path = uriURL.AbsolutePath;                                                  // 4 Double_slash_redirecting
            string str = "";
            if (path.Length >= 2)
                str = path.Substring(0, 2);
            if (str == "//")
                indexes.Add(1);
            else
                indexes.Add(-1);

            if (uriURL.Authority.Contains('-'))                                                 // 5 Prefix_Suffix
                indexes.Add(1);
            else
                indexes.Add(-1);

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
                indexes.Add(1);
            else
                indexes.Add(-1);

            if (uriURL.Authority == "bit.ly")                                                   // 7 Shortining_Service (SITAS VIETOJ Domain_registeration_length)
                indexes.Add(1);
            else
                indexes.Add(-1);

            int[] goodPorts = { 21, 22, 23, 80, 443, 445, 1433, 1521, 3306, 3389 };             // legit ports
            if (goodPorts.Contains(uriURL.Port) == false)                                       // 8 Port
                indexes.Add(1);
            else
                indexes.Add(-1);

            if (newURL.Contains("mail()") || newURL.Contains("mailto:"))                        // 9 Submitting_to_email (VIETOJ SFH)
                indexes.Add(1);
            else
                indexes.Add(-1);

            if (uriURL.Authority.Contains("https"))                                             // 10 HTTPS_token (VIETOJ Links_in_tags)
                indexes.Add(1);
            else
                indexes.Add(-1);


            indexes.Add(-1);                                                                    // Arturas liepe sita pridet kazkam

            return indexes;
        }

        public ActionResult AddToFile(string URL, int prediction)
        {
            Dictionary<string, string> URLs = new Dictionary<string, string>();
            ReadDataFromURL(URLs);

            if (!URLs.ContainsKey(URL))
            {
                if (prediction == -1)
                {
                    using (FileStream aFile = new FileStream("~/../Data/URLs.txt", FileMode.Append, FileAccess.Write))
                    using (StreamWriter sw = new StreamWriter(aFile))
                    {
                        sw.WriteLine(URL + "\t" + "bad");
                    }
                }
                else
                {
                    using (FileStream aFile = new FileStream("~/../Data/URLs.txt", FileMode.Append, FileAccess.Write))
                    using (StreamWriter sw = new StreamWriter(aFile))
                    {
                        sw.WriteLine(URL + "\t" + "good");
                    }
                }
            }

            //List<int[]> DataFromFile = new List<int[]>();
            //ReadDataFromIntegers(DataFromFile);

            List<double> indexes = PhishingIndexes(URL);

            using (FileStream aFile = new FileStream("~/../Data/cleanedData.txt", FileMode.Append, FileAccess.Write))
            using (StreamWriter sw = new StreamWriter(aFile))
            {
                sw.Write("9999999" + "\t");
                for (int i = 0; i < indexes.Count - 1; i++)
                    sw.Write(indexes[i] + "\t");

                sw.WriteLine(prediction);
            }

            Result result = new Result(URL, prediction);
            return View("Index", result);
        }

    }
}
