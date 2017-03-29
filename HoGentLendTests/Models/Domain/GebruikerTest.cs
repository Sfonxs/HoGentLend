using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HoGentLend.Models.Domain;
using System.Collections.Generic;
using Moq;
using System.Linq;

namespace HoGentLendTests.Models.Domain
{
    [TestClass]
    public class GebruikerTest
    {
        private Gebruiker student, lector;
        private string firstName, lastName, email;
        private VerlangLijst wishList;
        private List<Reservatie> reservaties;

        private Materiaal material, materialNotLendable;

        private Reservatie reservatieStudent1, reservatieLector1, reservatieStudent2Opgehaald, reservatieStudent3;
        private ReservatieLijn reservatieLijnStudentR1, reservatieLijnStudentR3;

        private Mock<IReservatieRepository> mockReservatieRepository;
        private DateTime _13April2016;
        private DateTime _20April2016;
        private DateTime _21April2016;
        private DateTime _28April2016;
        private DateTime _1April2016;
        private DateTime _8April2016;
        private IQueryable<Reservatie> allReservations;
        private Materiaal m1;
        private Materiaal m2;
        private Materiaal m3;
        private Materiaal m4;
        private Materiaal m5;
        private Materiaal m6;

        [TestInitialize]
        public void setup()
        {
            this.firstName = "Firstname";
            this.lastName = "Lastname";
            this.email = "Firstname.lastname@hogent.be";
            this.wishList = new VerlangLijst();
            this.reservaties = new List<Reservatie>();
            this.student = new Student(firstName, lastName, email);
            this.lector = new Lector(firstName, lastName, email);
            this.mockReservatieRepository = new Mock<IReservatieRepository>();
            this.material = new Materiaal()
            {
                Amount = 1,
                Name = "Materiaal Naam",
                IsLendable = true
            };
            this.materialNotLendable = new Materiaal()
            {
                Amount = 1,
                Name = "Materiaal Naam",
                IsLendable = false
            };

            m1 = new Materiaal
            {
                Name = "Wereldbol",
                Amount = 10,
                IsLendable = true
            };
            m2 = new Materiaal
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
            m3 = new Materiaal
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
            m4 = new Materiaal
            {
                Name = "Voetbal",
                Description = "Voetballen voor in het lager onderwijs.",
                ArticleCode = "abc147",
                Price = 25.99,
                Amount = 15,
                IsLendable = false,
                Location = "GSCHB 4.021"
            };
            m5 = new Materiaal
            {
                Name = "Basketbal",
                Description = "De NBA Allstar biedt de perfecte oplossing op het vlak van duurzaamheid en spelprestaties. Zowel geschikt voor indoor als outdoor. Uitstekende grip!",
                ArticleCode = "abc258",
                Price = 25.99,
                Amount = 12,
                AmountNotAvailable = 3,
                IsLendable = true,
                Location = "GSCHB 4.021",
                PhotoBytes = null
            };
            m6 = new Materiaal
            {
                Name = "Voetbal",
                Description = "Voetballen voor in het lager onderwijs.",
                ArticleCode = "abc147",
                Price = 25.99,
                Amount = 15,
                AmountNotAvailable = 3,
                IsLendable = false,
                Location = "GSCHB 4.021"
            };

            _13April2016 = new DateTime(2016, 4, 13);
            _20April2016 = new DateTime(2016, 4, 20);

            _21April2016 = new DateTime(2016, 4, 21);
            _28April2016 = new DateTime(2016, 4, 28);

            _1April2016 = new DateTime(2016, 4, 1);
            _8April2016 = new DateTime(2016, 4, 8);
            
            reservatieStudent1 = new Reservatie(student, _13April2016, _20April2016);
            reservatieStudent1.ReservatieLijnen = new List<ReservatieLijn>();
            reservatieLijnStudentR1 = new ReservatieLijn(2, _13April2016, _20April2016, m1, reservatieStudent1);
            reservatieStudent1.ReservatieLijnen.Add(reservatieLijnStudentR1);
            reservatieStudent1.ReservatieLijnen.Add(new ReservatieLijn(3, _13April2016, _20April2016, m2, reservatieStudent1));
            reservatieStudent1.ReservatieLijnen.Add(new ReservatieLijn(4, _13April2016, _20April2016, m3, reservatieStudent1));

            reservatieStudent2Opgehaald = new Reservatie(student, _1April2016, _8April2016)
            {
                Opgehaald = true
            };
            reservatieStudent2Opgehaald.ReservatieLijnen = new List<ReservatieLijn>();
            reservatieStudent2Opgehaald.ReservatieLijnen.Add(new ReservatieLijn(2, _1April2016, _8April2016, m1, reservatieStudent2Opgehaald));
            reservatieStudent2Opgehaald.ReservatieLijnen.Add(new ReservatieLijn(3, _1April2016, _8April2016, m2, reservatieStudent2Opgehaald));
            reservatieStudent2Opgehaald.ReservatieLijnen.Add(new ReservatieLijn(4, _1April2016, _8April2016, m3, reservatieStudent2Opgehaald));

            reservatieStudent3 = new Reservatie(student, _13April2016, _20April2016);
            reservatieStudent3.ReservatieLijnen = new List<ReservatieLijn>();
            reservatieLijnStudentR3 = new ReservatieLijn(2, _13April2016, _20April2016, m1, reservatieStudent3);
            reservatieStudent3.ReservatieLijnen.Add(reservatieLijnStudentR3);

            reservatieLector1 = new Reservatie(lector, _21April2016, _28April2016);
            reservatieLector1.ReservatieLijnen = new List<ReservatieLijn>();
            reservatieLector1.ReservatieLijnen.Add(new ReservatieLijn(2, _21April2016, _28April2016, m4, reservatieLector1));
            reservatieLector1.ReservatieLijnen.Add(new ReservatieLijn(3, _21April2016, _28April2016, m5, reservatieLector1));
            reservatieLector1.ReservatieLijnen.Add(new ReservatieLijn(4, _21April2016, _28April2016, m3, reservatieLector1));

            student.Reservaties.Add(reservatieStudent1);
            student.Reservaties.Add(reservatieStudent2Opgehaald);
            student.Reservaties.Add(reservatieStudent3);
            lector.Reservaties.Add(reservatieLector1);

            List<Reservatie> listAllReservations = new List<Reservatie>();
            listAllReservations.AddRange(student.Reservaties);
            listAllReservations.AddRange(lector.Reservaties);

            allReservations = listAllReservations.AsQueryable();

        }

        [TestMethod]
        public void TestFirstNameVerplicht()
        {
            new Student(null, lastName, email);
        }
        [TestMethod]
        public void TestLastNameVerplicht()
        {
            new Student(firstName, null, email);
        }
        [TestMethod]
        public void TestEmailVerplicht()
        {
            new Student(firstName, lastName, null);
        }
        [TestMethod]
        public void TestWishListVerplicht()
        {
            new Student(firstName, lastName, email, null, reservaties);
        }
        [TestMethod]
        public void TestReservatiesVerplicht()
        {
            new Lector(firstName, lastName, email, wishList, null);
        }

        [TestMethod]
        public void TestStudentCanNotSeeAllMaterials()
        {
            Assert.IsFalse(student.CanSeeAllMaterials());
        }
        [TestMethod]
        public void TestLectorCanSeeAllMaterials()
        {
            Assert.IsTrue(lector.CanSeeAllMaterials());
        }
        [TestMethod]
        public void TestAddToWishListAddsMaterialToWishList()
        {
            student.AddToWishList(material);
            Assert.AreEqual(1, student.WishList.Materials.Count);
            lector.AddToWishList(material);
            Assert.AreEqual(1, lector.WishList.Materials.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestStudentAddToWishListMateriaalVerplicht()
        {
            student.AddToWishList(null);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestLectorAddToWishListMateriaalVerplicht()
        {
            lector.AddToWishList(null);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestStudentAddToWishListMateriaalMustBeLendable()
        {
            student.AddToWishList(materialNotLendable);
        }
        [TestMethod]
        public void TestStudentAddToWishListMateriaalCanBeUnlendable()
        {
            lector.AddToWishList(materialNotLendable);
            Assert.AreEqual(1, lector.WishList.Materials.Count);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestStudentRemoveFromWishListMateriaalIsVerplicht()
        {
            student.RemoveFromWishList(null);
        }

        [TestMethod]
        public void TestStudentRemoveFromWishListMateriaalIsRemoved()
        {
            student.AddToWishList(material);
            student.RemoveFromWishList(material);
            Assert.AreEqual(0, lector.WishList.Materials.Count);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestRemoveReservationReservationIsVerplicht()
        {
            student.RemoveReservation(null);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestRemoveReservationReservationMustBeAdded()
        {
            student.RemoveReservation(reservatieLector1);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestRemoveReservationReservationMagNietOpgehaaldZijn()
        {
            student.RemoveReservation(reservatieStudent2Opgehaald);
        }
        [TestMethod]
        public void TestRemoveReservationDidRemoveTheReservation()
        {
            var beforeCount = student.Reservaties.Count;
            student.RemoveReservation(reservatieStudent1);
            Assert.AreEqual(beforeCount - 1, student.Reservaties.Count);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestRemoveReservationLineReservationLineIsVerplicht()
        {
            student.RemoveReservationLijn(null, mockReservatieRepository.Object);
        }
        [TestMethod]
        public void TestRemoveReservationLineDeletesReservationLine()
        {
            var beforeCount = reservatieStudent1.ReservatieLijnen.Count;
            student.RemoveReservationLijn(reservatieLijnStudentR1, mockReservatieRepository.Object);
            Assert.AreEqual(beforeCount - 1, reservatieStudent1.ReservatieLijnen.Count);
        }
        [TestMethod]
        public void TestRemoveReservationLineAlsoDeletedReservationIfLastLine()
        {
            var beforeCount = student.Reservaties.Count;
            student.RemoveReservationLijn(reservatieLijnStudentR3, mockReservatieRepository.Object);
            Assert.AreEqual(beforeCount - 1, student.Reservaties.Count);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestRemoveReservationLijnDeGebruikerBevatDeReservatieNiet()
        {
            student.RemoveReservationLijn(reservatieLector1.ReservatieLijnen.ElementAt(0), mockReservatieRepository.Object);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestRemoveReservationLijnReservatieLijnNietMeerInResevatie()
        {
            reservatieStudent1.ReservatieLijnen.Remove(reservatieLijnStudentR1);
            student.RemoveReservationLijn(reservatieLijnStudentR1, mockReservatieRepository.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestAddReservationTeReserverenMaterialenVerplicht()
        {
            student.AddReservation(null, _13April2016, _20April2016, _1April2016);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestAddReservationTeReserverenMaterialenNietLeeg()
        {
            student.AddReservation(new Dictionary<Materiaal, int>(), _13April2016, _20April2016, _1April2016);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestAddReservationMateriaalNietGenoegBeschikbaarDoorAndereReservaties()
        {
            // m1 4 keer gereserveerd, maar 10 beschikbaar
            student.AddReservation(CreateDic(m1, 7), _13April2016, _20April2016, _1April2016);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestAddReservationMateriaalNietGenoegBeschikbaar()
        {
            // m6 niet gereserveerd, maar 15 beschikbaar
            student.AddReservation(CreateDic(m6, 16), _13April2016, _20April2016, _1April2016);
        }
        [TestMethod]
        public void TestAddReservationKonTochToegevoegdWordenAlsLectorOmStudentTeOverlappen()
        {
            // m1 4 keer gereserveerd door studenten, maar 10 beschikbaar
            int beforeCount = lector.Reservaties.Count;
            lector.AddReservation(CreateDic(m1, 10), _13April2016, _20April2016, _1April2016);
            Assert.AreEqual(beforeCount + 1, lector.Reservaties.Count);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestAddReservationLectoKanLectorNietOverlappen()
        {
            // m4 2 keer gereserveerd door lector, maar 15 beschikbaar
            lector.AddReservation(CreateDic(m4, 13), _21April2016, _28April2016, _1April2016);
        }

        [TestMethod]
        public void TestAddReservationLuktInWeekZonderOverlappingen()
        {
            int beforeCount = lector.Reservaties.Count;
            lector.AddReservation(CreateDic(m4, 15), _13April2016, _21April2016, _1April2016);
            Assert.AreEqual(beforeCount + 1, lector.Reservaties.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestAddReservationNoLijnen()
        {
            lector.AddReservation(CreateDic(), _21April2016, _28April2016, _1April2016);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestAddReservationOphaalDatumBeforeToday()
        {
            lector.AddReservation(CreateDic(m4, 15), _13April2016, _21April2016, _28April2016);
        }

        private Dictionary<Materiaal, int> CreateDic(params Object[] objs)
        {
            var dic = new Dictionary<Materiaal, int>();
            for(int i = 0; i < objs.Length; i++)
            {
                dic.Add(objs[i] as Materiaal, Convert.ToInt32(objs[++i]));
            }
            return dic;
        }
    }
}
