using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using HoGentLend.Models.Domain;

namespace HoGentLend.Models.DAL
{
    public class HoGentLendContext : IdentityDbContext<ApplicationUser>
    {
        public HoGentLendContext() : base("HoGentLend")
        { }

        // DbSets
        public virtual DbSet<Materiaal> Materialen { get; set; }
        public virtual DbSet<Groep> Groepen { get; set; }
        public virtual DbSet<Gebruiker> Gebruikers { get; set; }
        public virtual DbSet<Reservatie> Reservaties { get; set; }
        public virtual DbSet<Config> Configs { get; set; } 

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Configurations.AddFromAssembly(Assembly.GetExecutingAssembly());
            //modelBuilder.Entity<IdentityUser>().Ignore(u => u.EmailConfirmed);
            //modelBuilder.Entity<IdentityUser>().Ignore(u => u.PhoneNumber);
            //modelBuilder.Entity<IdentityUser>().Ignore(u => u.PhoneNumberConfirmed);
            //modelBuilder.Entity<IdentityUser>().Ignore(u => u.TwoFactorEnabled);
            //modelBuilder.Entity<IdentityUser>().Ignore(u => u.LockoutEndDateUtc);
            //modelBuilder.Entity<IdentityUser>().Ignore(u => u.LockoutEnabled);
            //modelBuilder.Entity<IdentityUser>().Ignore(u => u.AccessFailedCount);
        }

        public static HoGentLendContext Create()
        {
            return DependencyResolver.Current.GetService<HoGentLendContext>();
        }

        static HoGentLendContext()
        {
            Console.WriteLine("setting the initializer");
            Database.SetInitializer<HoGentLendContext>(new HoGentLendDbInitializer());
        }

        public static void Init() { Create().Database.Initialize(true); }
    }

}