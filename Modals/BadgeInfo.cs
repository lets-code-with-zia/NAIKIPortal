using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NAIKI.Modals
{
    public class BadgeInfo
    {
        public int Id { get; set; }
        public string BadgeName { get; set; }
        public string IconURL { get; set; }
        public int MileStoneCount { get; set; }
        public string CustomHTML { get; set; }
        public DateTime EarnedOn { get; set; }
        public string EarnedOnString { get; set; }
    }
}