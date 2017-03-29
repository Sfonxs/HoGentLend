using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HoGentLend.Models.Domain;
using HoGentLendTests.Controllers;

namespace HoGentLendTests.Models.Domain
{

    [TestClass]
    public class ReservatieLijnTest
    {

        private DateTime ophaalMoment;
        private DateTime indienMoment;
        private Materiaal materiaal;
        private int amount;

        private Gebruiker student, student2, lector, lector2;
        private Materiaal m1, m2, m3;
        private Reservatie reservatie, rv, rv2, rvOverlapLector, rvOverlapStudent, rvOverlapLector2;
        private ReservatieLijn rvl1, rvl2, rvl3, rvlOverlapLectorMetRvl1, rvlOverlapStudentMetRvl3, 
            rvlOverlapLector1, rvlOverlapLector2;


        [TestInitialize]
        public void setup()
        {
            /* Data */
            DateTime _11April2016 = new DateTime(2016, 4, 11);
            DateTime _15April2016 = new DateTime(2016, 4, 15);

            DateTime _1April2016 = new DateTime(2016, 4, 1);
            DateTime _8April2016 = new DateTime(2016, 4, 8);

            ophaalMoment = _11April2016;
            indienMoment = _15April2016;

            student = new Student("Offline", "Student", "offline.student@hogent.be");
            student2 = new Student("Tweede", "Student", "tweede.student@hogent.be");
            lector = new Lector("Offline", "Lector", "offline.lector@hogent.be");
            lector2 = new Lector("Tweede", "Lector", "tweede.lector@hogent.be");

            this.reservatie = new Reservatie(student, ophaalMoment, indienMoment);
            student.Reservaties.Add(reservatie);
            this.amount = 1;
            this.materiaal = new Materiaal()
            {
                Name = "Een Materiaal",
                Amount = 1,
            };

            /* Materialen */

            m1 = new Materiaal
            {
                Name = "Rekenmachine",
                Description = "Reken er op los met deze grafische rekenmachine.",
                ArticleCode = "abc456",
                Price = 19.99,
                Amount = 4,
                AmountNotAvailable = 0,
                IsLendable = true,
                Location = "GSCHB 4.021"
            };
            m2 = new Materiaal
            {
                Name = "Kleurpotloden",
                Description = "Alle kleuren van de regenboog.",
                ArticleCode = "abc789",
                Price = 29.99,
                Amount = 10,
                AmountNotAvailable = 0,
                IsLendable = true,
                Location = "GSCHB 4.021"
            };
            m3 = new Materiaal
            {
                Name = "Voetbal",
                Description = "Voetballen voor in het lager onderwijs.",
                ArticleCode = "abc147",
                Price = 25.99,
                Amount = 15,
                AmountNotAvailable = 0,
                IsLendable = true,
                Location = "GSCHB 4.021"
            };




            /* Reservaties */
            rv = new Reservatie(student, _11April2016, _15April2016);
            rv.ReservatieLijnen = new List<ReservatieLijn>();
            rvl1 = new ReservatieLijn(2, _11April2016, _15April2016, m1, rv);
            rv.ReservatieLijnen.Add(rvl1);
            rvl2 = new ReservatieLijn(3, _11April2016, _15April2016, m2, rv);
            rv.ReservatieLijnen.Add(rvl2);
            rvl3 = new ReservatieLijn(5, _11April2016, _15April2016, m3, rv);
            rv.ReservatieLijnen.Add(rvl3);

            rv2 = new Reservatie(student2, _1April2016, _8April2016);
            rv2.ReservatieLijnen = new List<ReservatieLijn>();
            rv2.ReservatieLijnen.Add(new ReservatieLijn(2, _1April2016, _8April2016, m1, rv2));
            rv2.ReservatieLijnen.Add(new ReservatieLijn(3, _1April2016, _8April2016, m2, rv2));

            rvOverlapStudent = new Reservatie(student2, _11April2016, _15April2016)
            {
                Reservatiemoment = _8April2016
            };
            rvOverlapStudent.ReservatieLijnen = new List<ReservatieLijn>();
            rvlOverlapStudentMetRvl3 = new ReservatieLijn(14, _11April2016, _15April2016, m3, rvOverlapStudent);
            rvOverlapStudent.ReservatieLijnen.Add(rvlOverlapStudentMetRvl3);


            /* Overlappende Reservatie Lector */
            rvOverlapLector = new Reservatie(lector, _11April2016, _15April2016);
            rvOverlapLector.ReservatieLijnen = new List<ReservatieLijn>();
            rvlOverlapLectorMetRvl1 = (new ReservatieLijn(3, _11April2016, _15April2016, m1, rvOverlapLector));
            rvOverlapLector.ReservatieLijnen.Add(rvlOverlapLectorMetRvl1);

            rvlOverlapLector1 = new ReservatieLijn(5, _1April2016, _8April2016, m3, rvOverlapLector);
            rvOverlapLector.ReservatieLijnen.Add(rvlOverlapLector1);


            /* Overlappende Reservatie Overlappende Lector */
            rvOverlapLector2 = new Reservatie(lector2, _1April2016, _8April2016)
            {
                Reservatiemoment = _8April2016
            };
            rvOverlapLector2.ReservatieLijnen = new List<ReservatieLijn>();
            rvlOverlapLector2 = new ReservatieLijn(11, _1April2016, _8April2016, m3, rvOverlapLector2);
            rvOverlapLector2.ReservatieLijnen.Add(rvlOverlapLector2);

            reservatie = rv;
            student.Reservaties.Add(rv);
            student2.Reservaties.Add(rv2);
            student2.Reservaties.Add(rvOverlapStudent);
            lector.Reservaties.Add(rvOverlapLector);
            lector2.Reservaties.Add(rvOverlapLector2);

            m1.ReservatieLijnen.Add(rvl1);
            m1.ReservatieLijnen.Add(rvlOverlapLectorMetRvl1);
            m2.ReservatieLijnen.Add(rvl2);
            m3.ReservatieLijnen.Add(rvl3);
            m3.ReservatieLijnen.Add(rvlOverlapStudentMetRvl3);
            m3.ReservatieLijnen.Add(rvlOverlapLector2);
            m3.ReservatieLijnen.Add(rvlOverlapLector1);

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestAmountMagNietKleinerDanNulZijn()
        {
            new ReservatieLijn(-1, ophaalMoment, indienMoment, materiaal, reservatie);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestAmountMagNietNulZijn()
        {
            new ReservatieLijn(0, ophaalMoment, indienMoment, materiaal, reservatie);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestOphaalMomentVerplicht()
        {
            new ReservatieLijn(amount, null, indienMoment, materiaal, reservatie);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestIndienMomentVerplicht()
        {
            new ReservatieLijn(amount, ophaalMoment, null, materiaal, reservatie);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestMateriaalVerplicht()
        {
            new ReservatieLijn(amount, ophaalMoment, indienMoment, null, reservatie);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestReservatieVerplicht()
        {
            new ReservatieLijn(amount, ophaalMoment, indienMoment, materiaal, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestIndienMomentNaOphaalMomentVerplicht()
        {
            new ReservatieLijn(amount, indienMoment, ophaalMoment, materiaal, reservatie);
        }

        [TestMethod]
        public void FindConflictsNoConflicts()
        {
            Assert.AreEqual(0, rvl2.FindConflicts(false));
        }

        [TestMethod]
        public void FindConflictsConflictWithLector()
        {
            Assert.AreEqual(1, rvl1.FindConflicts(false));
        }

        [TestMethod]
        public void FindConflictsByLector()
        {
            Assert.AreEqual(0, rvlOverlapLectorMetRvl1.FindConflicts(true));
        }

        [TestMethod]
        public void FindConflictsConflictBetweenTwoStudentsLaterReservation()
        {
            Assert.AreEqual(1, rvl3.FindConflicts(false));
        }

        [TestMethod]
        public void FindConflictsConflictBetweenTwoStudentsEarlierReservation()
        {
            Assert.AreEqual(0, rvlOverlapStudentMetRvl3.FindConflicts(false));
        }

        [TestMethod]
        public void FindConflictsConflictBetweenTwoLectorsLaterReservation()
        {
            Assert.AreEqual(4, rvlOverlapLector1.FindConflicts(true));
        }

        [TestMethod]
        public void FindConflictsConflictBetweenTwoLectorsEarlierReservation()
        {
            Assert.AreEqual(0, rvlOverlapLector2.FindConflicts(true));
        }

    }
}
