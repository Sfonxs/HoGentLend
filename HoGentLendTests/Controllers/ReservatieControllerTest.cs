using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using HoGentLend.Controllers;
using HoGentLend.Models.Domain;
using HoGentLend.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace HoGentLendTests.Controllers
{
    [TestClass]
    public class ReservatieControllerTest
    {


        private DummyDataContext ctx;
        private ReservatieController reservatieController;


        private Mock<IMateriaalRepository> mockMateriaalRepository;
        private Mock<IGebruikerRepository> mockGebruikerRepository;
        private Mock<IReservatieRepository> mockReservatieRepository;
        private Mock<IConfigWrapper> mockConfigWrapper;

        private ReservatiePartModel rpm;
        private Materiaal m1, m2;
        private List<Groep> dgList;
        private Groep dg1;
        private List<Groep> lgListAardrijkskunde;
        private Groep lg2;

        private Reservatie r1, r2, r3;
        private ReservatieViewModel rvm;
        private ReservatieLijn rl1, rl2, rl3;
        private Gebruiker student, lector;

        [TestInitialize]
        public void SetupContext()
        {
            ctx = new DummyDataContext();

            rpm = new ReservatiePartModel();
            rpm.Amount = 5;
            rpm.MateriaalId = 342;

            dg1 = new Groep
            {
                IsLeerGebied = false,
                Name = "Kleuteronderwijs"
            };

            dgList = (new Groep[] { dg1 }).ToList();

            lg2 = new Groep
            {
                IsLeerGebied = true,
                Name = "Aardrijkskunde"
            };

            lgListAardrijkskunde = (new Groep[] { lg2 }).ToList();


            m1 = new Materiaal
            {
                Name = "Wereldbol",
                Amount = 10,
                Doelgroepen = dgList,
                Leergebieden = lgListAardrijkskunde,
                IsLendable = true,
            };

            mockMateriaalRepository = new Mock<IMateriaalRepository>();
            mockConfigWrapper = new Mock<IConfigWrapper>();

            //mockMateriaalRepository.Setup(m => m.FindAll()).Returns(ctx.MateriaalList);
            mockConfigWrapper.Setup(c => c.GetConfig()).Returns(new Config()
            {
                Indiendag = "vrijdag",
                Ophaaldag = "maandag",
                Indientijd = new DateTime(1, 1, 1, 17, 30, 0),
                Ophaaltijd = new DateTime(1, 1, 1, 10, 30, 0),
                LendingPeriod = 1
            });


            mockReservatieRepository = new Mock<IReservatieRepository>();
            mockMateriaalRepository = new Mock<IMateriaalRepository>();

            mockMateriaalRepository.Setup(m => m.FindBy(342)).Returns(m1);
            mockReservatieRepository.Setup(m => m.FindBy(342)).Returns(ctx.reservatie);

            reservatieController = new ReservatieController(mockReservatieRepository.Object,
                mockMateriaalRepository.Object, mockConfigWrapper.Object);

            /* Construct ReservatieViewModels */

            DateTime _11April2016 = new DateTime(2016, 4, 11);
            DateTime _15April2016 = new DateTime(2016, 4, 15);

            student = new Student("Offline", "Student", "offline.student@hogent.be");
            lector = new Lector("Offline", "Lector", "offline.lector@hogent.be");

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

            r1 = new Reservatie(student, _11April2016, _15April2016);
            r1.ReservatieLijnen = new List<ReservatieLijn>();
            rl1 = new ReservatieLijn(2, _11April2016, _15April2016, m2, r1);
            r1.ReservatieLijnen.Add(rl1);

            r2 = new Reservatie(lector, _11April2016, _15April2016)
            {
                Reservatiemoment = _11April2016
            };
            r2.ReservatieLijnen = new List<ReservatieLijn>();
            rl2 = (new ReservatieLijn(3, _11April2016, _15April2016, m2, r2));
            r2.ReservatieLijnen.Add(rl2);

            r3 = new Reservatie(student, _11April2016, _15April2016);
            r3.ReservatieLijnen = new List<ReservatieLijn>();
            rl3 = new ReservatieLijn(2, _11April2016, _15April2016, m1, r3);
            r3.ReservatieLijnen.Add(rl3);


            student.Reservaties.Add(r1);
            lector.Reservaties.Add(r2);
            student.Reservaties.Add(r3);

            m1.ReservatieLijnen.Add(rl3);
            m2.ReservatieLijnen.Add(rl1);
            m2.ReservatieLijnen.Add(rl2);

            rvm = new ReservatieViewModel(r1);

            mockReservatieRepository.Setup(r => r.FindBy(456)).Returns(r1);


        }

        [TestMethod]
        public void IndexReturnsZeroReservationsForRuben()
        {
            Gebruiker g = ctx.GebruikerList.First(u => u.Email.Equals("ruben@hogent.be"));

            // Act
            ViewResult result = reservatieController.Index(g) as ViewResult;
            ReservatieViewModel[] reservations = ((IEnumerable<ReservatieViewModel>)result.Model).ToArray();

            // Assert
            Assert.AreEqual(1, reservations.Length);
        }

        [TestMethod]
        public void AddSetsReservationsForRubenOnTwo()
        {


            Gebruiker g = ctx.GebruikerList.First(u => u.Email.Equals("ruben@hogent.be"));
            ViewResult result = reservatieController.Index(g) as ViewResult;
            List<ReservatiePartModel> rpms = new List<ReservatiePartModel>();
            //mockReservatieRepository.Setup(m => m.SaveChanges());
            rpms.Add(rpm);

            // Act

            reservatieController.Add(g, rpms, DateTime.Now.AddDays(12));

            //Assert
            Assert.AreEqual(2, g.Reservaties.Count);
            mockReservatieRepository.Verify(m => m.SaveChanges(), Times.Once);
            Assert.AreEqual("De reservatie  is toegevoegd aan uw verlanglijst.", result.TempData.Peek("msg"));

        }

        [TestMethod]
        public void RemoveReservationsFailsWrongId()
        {
            Gebruiker g = ctx.GebruikerList.First(u => u.Email.Equals("ruben@hogent.be"));

            ViewResult result = reservatieController.Index(g) as ViewResult;

            reservatieController.Remove(g, 341);

            Assert.AreEqual(1, g.Reservaties.Count);
            mockReservatieRepository.Verify(m => m.SaveChanges(), Times.Never);
            Assert.AreEqual("De reservatie is niet beschikbaar of mogelijk al verwijderd.", result.TempData.Peek("err"));
        }

        [TestMethod]
        public void RemoveReservationsRemovesRubensOnlyReservation()
        {
            Gebruiker g = ctx.GebruikerList.First(u => u.Email.Equals("ruben@hogent.be"));

            ViewResult result = reservatieController.Index(g) as ViewResult;

            List<ReservatiePartModel> rpms = new List<ReservatiePartModel>();
            rpms.Add(rpm);

            reservatieController.Remove(g, 342);

            Assert.AreEqual(0, g.Reservaties.Count);
            mockReservatieRepository.Verify(m => m.SaveChanges(), Times.Once);
            Assert.AreEqual("De reservatie is succesvol verwijderd.", result.TempData.Peek("msg"));
        }

        

        [TestMethod]
        public void DetailViewBagOphaalTijd()
        {
            Gebruiker g = ctx.GebruikerList.First(u => u.Email.Equals("ruben@hogent.be"));

            ViewResult result = reservatieController.Detail(g, 342) as ViewResult;

            var ophaalTijd = result.ViewBag.ophaalTijd;


            Assert.AreEqual("10:30", ophaalTijd);
        }

        [TestMethod]
        public void DetailViewBagIndienTijd()
        {
            Gebruiker g = ctx.GebruikerList.First(u => u.Email.Equals("ruben@hogent.be"));

            ViewResult result = reservatieController.Detail(g, 342) as ViewResult;

            var indienTijd = result.ViewBag.indienTijd;


            Assert.AreEqual("17:30", indienTijd);
        }


        [TestMethod]
        public void RemoveReservationLineSavesAndReturnsMessage()
        {
            reservatieController.RemoveReservationLine(student, 456, (int)rl1.Id);

            Assert.AreEqual(r1.ReservatieLijnen.Count, 0);
            mockReservatieRepository.Verify(m => m.SaveChanges(), Times.Once);
            Assert.IsTrue(reservatieController.TempData["msg"] != null);
        }

        [TestMethod]
        public void RemoveReservationNoSuchReservationLine()
        {
            reservatieController.RemoveReservationLine(student, 456, 117);

            Assert.AreEqual(r1.ReservatieLijnen.Count, 1);
            Assert.IsTrue(reservatieController.TempData["err"] != null);
        }

        [TestMethod]
        public void ConstructReservatieViewModelsWithConflicts()
        {
            reservatieController.ConstructReservatieViewModels(r1, rvm, student);
            Assert.IsNotNull(rvm.ReservatieLijnen);
            Assert.AreEqual(rvm.ReservatieLijnen[0].AantalSlechtsBeschikbaar, 1);
            Assert.IsTrue(rvm.Conflict);

        }

        [TestMethod]
        public void ConstructReservatieViewModelsWithNoConflicts()
        {
            reservatieController.ConstructReservatieViewModels(r3, rvm, student);
            Assert.IsNotNull(rvm.ReservatieLijnen);
            Assert.AreEqual(rvm.ReservatieLijnen[0].AantalSlechtsBeschikbaar, 0);
            Assert.IsFalse(rvm.Conflict);
        }

        [TestMethod]
        public void ConstructReservatieViewModelsWithConflictsAsLector()
        {
            reservatieController.ConstructReservatieViewModels(r2, rvm, lector);
            Assert.IsNotNull(rvm.ReservatieLijnen);
            Assert.AreEqual(rvm.ReservatieLijnen[0].AantalSlechtsBeschikbaar, 0);
            Assert.IsFalse(rvm.Conflict);
        }

    }
}
