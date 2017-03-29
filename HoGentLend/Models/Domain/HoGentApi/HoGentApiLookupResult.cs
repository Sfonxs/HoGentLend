using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HoGentLend.Models.Domain.HoGentApi
{
    public class HoGentApiLookupResult
    {
        public string Faculteit { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
        public string Base64Foto { get; set; }
        public string Type { get; set; }

        public bool IsCompleetResult()
        {
            return IsValidString(LastName)
                && IsValidString(FirstName)
                && IsValidString(Email)
                && IsValidString(Type);
        }

        private bool IsValidString(string str)
        {
            return str != null && str.Length > 0;
        }
    }
}