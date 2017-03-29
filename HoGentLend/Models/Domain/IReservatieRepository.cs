using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoGentLend.Models.Domain
{
   public interface IReservatieRepository : IRepository<Reservatie, int>
    {
        void RemoveReservationLine(ReservatieLijn rl);
    }
}
