using HoGentLend.Models.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace HoGentLend.Models.DAL
{
    public class Repository<EntityType, IdType> : IRepository<EntityType, IdType>
        where EntityType : class
    {
        protected DbSet<EntityType> dbSet;
        private DbContext dbContext;

        protected DbSet<EntityType> DbSet { get { return dbSet; } }
        protected DbContext DbContext { get { return dbContext; } }

        public Repository(DbSet<EntityType> dbSet, DbContext dbContext)
        {
            this.dbSet = dbSet;
            this.dbContext = dbContext;
        }

        public void Add(EntityType entity)
        {
            dbSet.Add(entity);
        }

        public void Delete(EntityType entity)
        {
            dbSet.Remove(entity);
        }

        public virtual IQueryable<EntityType> FindAll()
        {
            return dbSet;
        }

        public virtual EntityType FindBy(IdType id)
        {
            return dbSet.Find(id);
        }

        public void SaveChanges()
        {
            dbContext.SaveChanges();
        }
    }
}