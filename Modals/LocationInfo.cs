using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NAIKI.Modals
{
    public class LocationInfo
    {
        public int Id { get; set; }
        public string Cordinates { get; set; }
        public double MileRadius { get; set; }
        public DateTime LastUpdated { get; set; }
        public string LastUpdatedString { get; set; }
    }
}