using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CupidoBotV1.Data
{
    //C# Class Representing the Profile Stored into the SQL Database
    [Serializable]
    public class CupidoProfile
    {
        public string UserId { get; set; }
        public string ChannelId { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public int NormalPoints { get; set; }
        public int SuperModelPoints { get; set; }
        public int HairBald { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string picUrl { get; set; }
    }

}