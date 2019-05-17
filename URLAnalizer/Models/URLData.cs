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
        public int Domain_registeration_length { get; set; }
        public int Port { get; set; }
        public int Links_in_tags { get; set; }
        public int SFH { get; set; }
        public int Result { get; set; }


        public URLData(int Having_IPhaving_IP_Address, int URL_Length, int Having_At_Symbol, int Double_slash_redirecting, int Prefix_Suffix, int Having_Sub_Domain,
            int Domain_registeration_length, int Port, int Links_in_tags, int SFH, int Result)
        {
            this.Having_IPhaving_IP_Address = Having_IPhaving_IP_Address;
            this.URL_Length = URL_Length;
            this.Having_At_Symbol = Having_At_Symbol;
            this.Double_slash_redirecting = Double_slash_redirecting;
            this.Prefix_Suffix = Prefix_Suffix;
            this.Having_Sub_Domain = Having_Sub_Domain;
            this.Domain_registeration_length = Domain_registeration_length;
            this.Port = Port;
            this.Links_in_tags = Links_in_tags;
            this.SFH = SFH;
            this.Result = Result;
        }
    }
}