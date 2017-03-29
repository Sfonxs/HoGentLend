using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HoGentLend.Models.Domain
{
    public interface IRepository<EntityType, IdType>
    {
        EntityType FindBy(IdType id);

        IQueryable<EntityType> FindAll();

        void Add(EntityType entity);

        void Delete(EntityType entity);

        void SaveChanges();
    }
}