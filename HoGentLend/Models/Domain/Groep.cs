using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HoGentLend.Models.Domain
{
    public class Groep
    {
        public long Id { get; private set; }

        public string Name { get; set; }
        public bool IsLeerGebied { get; set; }
    }
}