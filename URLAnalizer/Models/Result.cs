using System;

namespace URLAnalizer.Models
{
    public class Result
    {
        public string URL { get; set; }
        public int Prediction { get; set; }

        public Result() { }

        public Result(string URL, int Prediction)
        {
            this.URL = URL;
            this.Prediction = Prediction;
        }
    }
}