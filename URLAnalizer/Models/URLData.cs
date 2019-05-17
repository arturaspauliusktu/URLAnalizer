using System;

namespace URLAnalizer.Models
{
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
        public int Submitting_to_email { get; set; }
        public int HTTPS_token { get; set; }
        public int Result { get; set; }


        public URLData(int Having_IPhaving_IP_Address, int URL_Length, int Having_At_Symbol, int Shortining_Service, int Prefix_Suffix, int Having_Sub_Domain,
            int Domain_registeration_length, int Port, int Submitting_to_email, int HTTPS_token, int Result)
        {
            this.Having_IPhaving_IP_Address = Having_IPhaving_IP_Address;
            this.URL_Length = URL_Length;
            this.Having_At_Symbol = Having_At_Symbol;
            this.Double_slash_redirecting = Double_slash_redirecting;
            this.Prefix_Suffix = Prefix_Suffix;
            this.Having_Sub_Domain = Having_Sub_Domain;
            this.Shortining_Service = Shortining_Service;
            this.Port = Port;
            this.Submitting_to_email = Submitting_to_email;
            this.HTTPS_token = HTTPS_token;
            this.Result = Result;
        }
    }
}