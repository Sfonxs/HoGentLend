using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoGentLend.Models.Domain
{
    public interface IGroepRepository : IRepository<Groep, int>
    {
        IQueryable<Groep> FindAllDoelGroepen();
        IQueryable<Groep> FindAllLeerGebieden();

    }
}
