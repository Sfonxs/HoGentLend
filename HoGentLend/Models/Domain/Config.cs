using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HoGentLend.Models.Domain
{
    public class Config
    {
        public long Id { get; set; }
        public int LendingPeriod { get; set; }
        public String Indiendag { get; set; }
        public String Ophaaldag { get; set; }
        public DateTime Indientijd { get; set; }
        public DateTime Ophaaltijd { get; set; }
    }
}