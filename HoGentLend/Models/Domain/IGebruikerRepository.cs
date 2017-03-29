using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HoGentLend.Models.Domain
{
    public interface IGebruikerRepository : IRepository<Gebruiker, int>
    {
        Gebruiker FindByEmail(string email);
    }
}