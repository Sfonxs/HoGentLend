using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HoGentLend.Models.Domain
{
    public interface IMateriaalRepository : IRepository<Materiaal, int>
    {

        IEnumerable<Materiaal> FindByFilter(String filter, int doelgroepId, int leergebiedId);
    }
}