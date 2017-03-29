using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HoGentLend.Models.Domain.HoGentApi
{
    public class OfflineHoGentApiLookupProvider : IHoGentApiLookupProvider
    {
        public HoGentApiLookupResult Lookup(string userId, string unhashedPassword)
        {
            if (userId == "student" && unhashedPassword == "student")
            {
                return new HoGentApiLookupResult()
                {
                    Faculteit = "FBO",
                    FirstName = "Offline",
                    LastName  = "Student",
                    Email = "offline.student@hogent.be",
                    Type = "student"
                };
            }
            if (userId == "lector" && unhashedPassword == "lector")
            {
                return new HoGentApiLookupResult()
                {
                    Faculteit = "FBO",
                    FirstName = "Offline",
                    LastName = "Lector",
                    Email = "offline.lector@hogent.be",
                    Type = "personeel"
                };
            }
            return new HoGentApiLookupResult();
        }
    }
}