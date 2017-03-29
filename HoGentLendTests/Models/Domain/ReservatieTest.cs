using HoGentLend.Models.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoGentLendTests.Models.Domain
{
    [TestClass]
    public class ReservatieTest
    {

        private Reservatie reservatie;
        private Gebruiker lener;
        private DateTime ophaalMoment;
        private DateTime indienMoment;
        private int maandag;
        private int maandag2;
        private int vrijdag;
        private int eenweek;
        private int tweeweken;



        [TestInitialize]
        public void setup()
        {
            this.lener = new Student("Lener", "De Lener", "lener@email.com");

            this.ophaalMoment = new DateTime(2016, 4, 1);
            this.indienMoment = new DateTime(2016, 4, 8);

            this.reservatie = new Reservatie(lener, ophaalMoment, indienMoment);
            this.lener.Reservaties.Add(reservatie);

            this.maandag = 1;
            this.maandag2 = 1;
            this.vrijdag = 5;
            this.eenweek = 1;
            this.tweeweken = 2;
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestLenerVerplicht()
        {
            new Reservatie(null, ophaalMoment, indienMoment);
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestOphaalMomentVerplicht()
        {
            new Reservatie(lener, null, indienMoment);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestIndienMomentVerplicht()
        {
            new Reservatie(lener, ophaalMoment, null);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestIndienMomentNaOphaalMomentVerplicht()
        {
            new Reservatie(lener, indienMoment, ophaalMoment);
        }

        [TestMethod]
        public void TestOpgehaaldIsFalseBijNieuweReservatie()
        {
            Assert.IsFalse(reservatie.Opgehaald);
        }

        [TestMethod]
        public void TestReservatieMomentIsNowBijNieuweReservatie()
        {
            DateTime before = DateTime.Now;
            Reservatie r = new Reservatie(lener, ophaalMoment, indienMoment);
            DateTime after = DateTime.Now;
            Assert.IsTrue(before <= r.Reservatiemoment && r.Reservatiemoment >= after);
        }

        [TestMethod]
        public void TestAddReservatieLijnAddReservationLineAddReservationLine()
        {
            reservatie.AddReservationLine(new Materiaal()
            {
                Name = "Een Materiaal",
                Amount = 1,
            }, 1, ophaalMoment, indienMoment);
            Assert.AreEqual(1, reservatie.ReservatieLijnen.Count);
        }

        [TestMethod]
        public void CalculateAmountMondayToFridaySameWeekReturns4()
        {
            int verschil = Reservatie.CalculateAmountDaysOphaalDatumFromIndienDatum(vrijdag, eenweek, maandag);

            Assert.AreEqual(4, verschil);
        }

        [TestMethod]
        public void CalculateAmountMondayToFridayNotSameWeekReturns11()
        {
            int verschil = Reservatie.CalculateAmountDaysOphaalDatumFromIndienDatum(vrijdag, tweeweken, maandag);

            Assert.AreEqual(11, verschil);
        }

        [TestMethod]
        public void CalculateAmountMondayToMondayNextWeekReturns7()
        {
            int verschil = Reservatie.CalculateAmountDaysOphaalDatumFromIndienDatum(maandag2, eenweek, maandag);

            Assert.AreEqual(7, verschil);
        }

        [TestMethod]
        public void CalculateAmountMondayToMondayTwoWeeksReturns14()
        {
            int verschil = Reservatie.CalculateAmountDaysOphaalDatumFromIndienDatum(maandag2, tweeweken, maandag);

            Assert.AreEqual(14, verschil);
        }
    }
}
