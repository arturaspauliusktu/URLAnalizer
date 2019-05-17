using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCleaner
{
    class Program
    {
        static void Main(string[] args)
        {
            List<URLData> dataFromFile = new List<URLData>();
            ReadDataFromFile(dataFromFile);

            List<URLData> cleanedData = CleanUpData(dataFromFile);
            OutputToFile(cleanedData);

        }

        static public void ReadDataFromFile(List<URLData> dataFromFile)
        {
            bool firstLine = true;


            string[] lines = System.IO.File.ReadAllLines("vatxt.txt");
            foreach (string line in lines)
            {
                if (firstLine)
                {
                    firstLine = false;
                    continue;
                }

                string[] s = line.Split('\t');
                dataFromFile.Add(new URLData(int.Parse(s[1]), int.Parse(s[2]), int.Parse(s[3]), int.Parse(s[4]), int.Parse(s[5]), int.Parse(s[6]), int.Parse(s[7]), int.Parse(s[8]),
                    int.Parse(s[9]), int.Parse(s[10]), int.Parse(s[11])));
            }
        }

        static public List<URLData> CleanUpData(List<URLData> dataFromFile)
        {
            List<URLData> cleanedData = new List<URLData>();

            for (int i = 0; i < dataFromFile.Count - 1; i++)
            {
                for (int j = i + 1; j < dataFromFile.Count; j++)
                {
                    if (dataFromFile[j].Result != -10 && dataFromFile[i].Result != -10 && ( dataFromFile[i].Having_IPhaving_IP_Address == dataFromFile[j].Having_IPhaving_IP_Address
                        && dataFromFile[i].URL_Length == dataFromFile[j].URL_Length && dataFromFile[i].Having_At_Symbol == dataFromFile[j].Having_At_Symbol 
                        && dataFromFile[i].Double_slash_redirecting == dataFromFile[j].Double_slash_redirecting && dataFromFile[i].Prefix_Suffix == dataFromFile[j].Prefix_Suffix 
                        && dataFromFile[i].Having_Sub_Domain == dataFromFile[j].Having_Sub_Domain && dataFromFile[i].Shortining_Service == dataFromFile[j].Shortining_Service
                        && dataFromFile[i].Port == dataFromFile[j].Port && dataFromFile[i].Links_in_tags == dataFromFile[j].Links_in_tags && dataFromFile[i].Submitting_to_email == dataFromFile[j].Submitting_to_email
                        && dataFromFile[i].Result != dataFromFile[j].Result))
                    {
                        dataFromFile[j].Result = -10;
                    }
                }
            }

            foreach(URLData u in dataFromFile)
            {
                if (u.Result != -10)
                    cleanedData.Add(u);
            }
            return cleanedData;
        }

        static public void OutputToFile(List<URLData> cleanedData)
        {
            string title = "index	having_IPhaving_IP_Address	URLURL_Length	having_At_Symbol	double_slash_redirecting	Prefix_Suffix	having_Sub_Domain	Shortining_Service	port	Links_in_tags	Submitting_to_email	Result";
            using (StreamWriter file = new StreamWriter("cleanedData.txt"))
            {
                file.WriteLine(title);
                for (int i = 0; i < cleanedData.Count; i++)
                {
                    int a = i + 1; 
                    file.WriteLine(a + "\t" + cleanedData[i].Having_IPhaving_IP_Address + "\t" + cleanedData[i].URL_Length + "\t" + cleanedData[i].Having_At_Symbol + "\t" 
                        + cleanedData[i].Double_slash_redirecting + "\t" + cleanedData[i].Prefix_Suffix + "\t" + cleanedData[i].Having_Sub_Domain + "\t" + cleanedData[i].Shortining_Service
                        + "\t" + cleanedData[i].Port + "\t" + cleanedData[i].Links_in_tags + "\t" + cleanedData[i].Submitting_to_email + "\t" + cleanedData[i].Result);
                }
            }
        }

       
    }

    public class URLData
    {
        public int Having_IPhaving_IP_Address { get; set; }
        public int URL_Length { get; set; }
        public int Having_At_Symbol { get; set; }


        public int Double_slash_redirecting { get; set; }
        public int Prefix_Suffix { get; set; }
        public int Having_Sub_Domain { get; set; }
        public int Shortining_Service { get; set; }
        public int Port { get; set; }
        public int Links_in_tags { get; set; }
        public int Submitting_to_email { get; set; }
        public int Result { get; set; }


        public URLData(int Having_IPhaving_IP_Address, int URL_Length, int Having_At_Symbol, int Double_slash_redirecting, int Prefix_Suffix, int Having_Sub_Domain,
            int Shortining_Service, int Port, int Links_in_tags, int Submitting_to_email, int Result)
        {
            this.Having_IPhaving_IP_Address = Having_IPhaving_IP_Address;
            this.URL_Length = URL_Length;
            this.Having_At_Symbol = Having_At_Symbol;
            this.Double_slash_redirecting = Double_slash_redirecting;
            this.Prefix_Suffix = Prefix_Suffix;
            this.Having_Sub_Domain = Having_Sub_Domain;
            this.Shortining_Service = Shortining_Service;
            this.Port = Port;
            this.Links_in_tags = Links_in_tags;
            this.Submitting_to_email = Submitting_to_email;
            this.Result = Result;
        }
    }
}
