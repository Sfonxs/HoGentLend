using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HoGentLend.Models.Domain.HoGentApi
{
    public interface IHoGentApiLookupProvider
    {
        HoGentApiLookupResult Lookup(string userId, string password);
    }
}