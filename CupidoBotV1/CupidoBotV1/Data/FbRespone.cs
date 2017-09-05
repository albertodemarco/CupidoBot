using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CupidoBotV1.Data
{
    //C# Representation Object of the facebook graph response when asking for profile data
    [Serializable]
    public class LastAdReferral
    {
        public string source { get; set; }
        public string type { get; set; }
        public string ad_id { get; set; }
    }
    [Serializable]
    public class FbResponse
    {
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string profile_pic { get; set; }
        public string locale { get; set; }
        public int timezone { get; set; }
        public string gender { get; set; }
        public LastAdReferral last_ad_referral { get; set; }
    }
}