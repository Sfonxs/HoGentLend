using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HoGentLend.Models.Domain;

namespace HoGentLendTests.Controllers
{
    class DummyDataContext
    {
        public IQueryable<Gebruiker> GebruikerList { get; set; }
        public IQueryable<Materiaal> MateriaalList { get; set; }
        public IQueryable<Groep> DoelgroepList { get; set; }
        public IQueryable<Groep> LeergebiedList { get; set; }


        public Reservatie reservatie { get; set; }

        public Config Config { get; set; }

        public DummyDataContext()
        {
            /* Config */
            DateTime _10u30 = new DateTime(2016, 4, 13).AddHours(10).AddMinutes(30);
            DateTime _17u00 = new DateTime(2016, 4, 13).AddHours(17);

            Config = new Config()
            {
                Indiendag = "maandag",
                Ophaaldag = "vrijdag",
                LendingPeriod = 1,
                Indientijd = _10u30,
                Ophaaltijd = _17u00
            };

            /* Doelgroepen */
            Groep dg1 = new Groep
            {
                IsLeerGebied = false,
                Name = "Kleuteronderwijs"
            };

            List<Groep> dgList = (new Groep[] { dg1 }).ToList();
            DoelgroepList = dgList.AsQueryable();

            /* Leergebieden */
            Groep lg1 = new Groep
            {
                IsLeerGebied = true,
                Name = "Wiskunde"
            };

            Groep lg2 = new Groep
            {
                IsLeerGebied = true,
                Name = "Aardrijkskunde"
            };

            List<Groep> lgList = (new Groep[] { lg1, lg2 }).ToList();
            LeergebiedList = lgList.AsQueryable();

            List<Groep> lgListWiskunde = (new Groep[] { lg1 }).ToList();
            List<Groep> lgListAardrijkskunde = (new Groep[] { lg2 }).ToList();

            /* Materials */
            Materiaal m1 = new Materiaal
            {
                Id = 1,
                Name = "Wereldbol",
                Amount = 10,
                Doelgroepen = dgList,
                Leergebieden = lgListAardrijkskunde,
                IsLendable = true,
            };

            Materiaal m2 = new Materiaal
            {
                Id = 2,
                Name = "Rekenmachine",
                Amount = 2,
                Doelgroepen = dgList,
                Leergebieden = lgListWiskunde,
                IsLendable = true,
            };

            Materiaal m3 = new Materiaal
            {
                Id = 3,
                Name = "Basketbal",
                Amount = 3,
                IsLendable = false
            };



            IList<Materiaal> mList = (new Materiaal[] { m1, m2 }).ToList(); /* all lendable materials */

            List<Materiaal> mList2 = (new Materiaal[] { m1, m2, m3 }).ToList(); /* all materials */
            MateriaalList = mList2.AsQueryable();

            /* Wishlists */
            VerlangLijst l1 = new VerlangLijst
            {
                Materials = mList
            };

            VerlangLijst l2 = new VerlangLijst
            {
                Materials = new List<Materiaal>()
            };


            /* Users */
            Gebruiker g1 = new Student("Ruben", "Vermeulen", "ruben@hogent.be", l1, new List<Reservatie>());

            Gebruiker g2 = new Student("Sven", "Dedeene", "sven@hogent.be", l2, new List<Reservatie>());

            Gebruiker g3 = new Lector("Xander", "Berkein", "lector@hogent.be", l2, new List<Reservatie>());

            GebruikerList = (new Gebruiker[] { g1, g2, g3 }).ToList().AsQueryable();

            /* Reservaties */
            Reservatie rv = new Reservatie(g1, DateTime.Now, DateTime.Now.AddDays(5));

            reservatie = rv;
            g1.Reservaties.Add(rv);

            /* Reservatie lijnen */
            ReservatieLijn rvl = new ReservatieLijn(5, DateTime.Now, DateTime.Now.AddDays(5), m1, rv);

            m1.ReservatieLijnen = new List<ReservatieLijn>() {rvl};
        }
    }
}
