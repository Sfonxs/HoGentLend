using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HoGentLend.Models.Domain;
using System.Linq;
using System.Web;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using HoGentLend.Models.Domain;

namespace HoGentLend.Models.DAL
{
    public class ReservatieRepository : Repository<Reservatie, int>, IReservatieRepository
    {
        public ReservatieRepository(HoGentLendContext ctx) : base(ctx.Reservaties, ctx)
        {
            
        }

        public override IQueryable<Reservatie> FindAll()
        {
            return
                dbSet.Include(r => r.ReservatieLijnen)
                    .Include(r => r.Lener)
                    .Include(r => r.ReservatieLijnen.Select(rl => rl.Materiaal));
        }

        public override Reservatie FindBy(int id)
        {
            return FindAll()
              .SingleOrDefault(x => x.Id == id);
        }

        public void RemoveReservationLine(ReservatieLijn rl)
        {
            DbContext.Entry(rl).State = EntityState.Deleted;
        }
    }
}