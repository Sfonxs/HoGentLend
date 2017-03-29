using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using HoGentLend.Models.Domain;

namespace HoGentLend.Models.DAL
{
    public class GroepRepository : Repository<Groep, int>, IGroepRepository
    {

        public GroepRepository(HoGentLendContext ctx) : base(ctx.Groepen, ctx)
        {}

        public IQueryable<Groep> FindAllDoelGroepen()
        {
            return FindAll().Where(g => ! g.IsLeerGebied);
        }

        public IQueryable<Groep> FindAllLeerGebieden()
        {
            return FindAll().Where(g => g.IsLeerGebied);
        }
 
    }
}