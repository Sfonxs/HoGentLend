using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using HoGentLend.Models.Domain;

namespace HoGentLend.Models.DAL
{
    public class MateriaalRepository : Repository<Materiaal, int>, IMateriaalRepository
    {
        private HoGentLendContext ctx;
        //private DbSet<Materiaal> materialen;

        public MateriaalRepository(HoGentLendContext ctx) : base(ctx.Materialen, ctx)
        {
        }

        public override Materiaal FindBy(int id)
        {
            return
                dbSet.Include(m => m.ReservatieLijnen)
                    .Include(m => m.ReservatieLijnen.Select(rl => rl.Reservatie.Lener))
                    .SingleOrDefault(m => m.Id == id);
        }

        public IEnumerable<Materiaal> FindByFilter(String filter, int doelgroepId, int leergebiedId)
        {
            IQueryable<Materiaal> materialen;

            if (String.IsNullOrEmpty(filter) && doelgroepId == 0 && leergebiedId == 0)
            {
                materialen = FindAll();
            }

            else
            {
                filter = filter.ToLower();
                materialen = FindAll().
                    Where(m =>
                        (m.Name.ToLower().Contains(filter)) ||
                        (m.ArticleCode.ToLower().Contains(filter)) ||
                        (m.Firma.Email.ToLower().Contains(filter)) ||
                        (m.Firma.Name.ToLower().Contains(filter)) ||
                        (m.Location.ToLower().Contains(filter))|| 
                        (m.Description.ToLower().Contains(filter))
                    );

                if (doelgroepId != 0)
                {
                    materialen = materialen.Where(m => m.Doelgroepen.Any(d => d.Id == doelgroepId));
                }
                if (leergebiedId != 0)
                {
                    materialen = materialen.Where(m => m.Leergebieden.Any(d => d.Id == leergebiedId));
                }
            }

            var mat = materialen.Include(m => m.Firma);
            try
            {
                mat = mat
                   .Include(m => m.Doelgroepen)
                   .Include(m => m.Leergebieden);

            }
            catch (ArgumentNullException e)
            {
                return mat;
            }
            return mat
                    .OrderBy(m => m.Name)
                   .ToList();
        }

    }
}