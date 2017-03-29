using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using HoGentLend.Models.DAL;
using HoGentLend.Models.Domain;
using HoGentLendTests.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace HoGentLendTests.Models.Domain.DAL
{
    [TestClass]
    public class MateriaalRepositoryTest
    {

        private MateriaalRepository repository;
        private DummyDataContext ctx;

        private Mock<HoGentLendContext> mockHogentLendContext;

        private const string FILTER_WERELDBOL = "Wereldbol";
        private const string FILTER_WERELDBOL_CAPS = "WERELDBOL";
        private const int ID_KLEUTERONDERWIJS = 1;
        private const int ID_WISKUNDE = 2;
        private const int ID_AARDRIJKSKUNDE = 3;

        [TestInitialize]
        public void SetupContext()
        {
            ctx = new DummyDataContext();
            var materialen = ctx.MateriaalList;

            var mockSet = new Mock<DbSet<Materiaal>>();
            mockSet.As<IQueryable<Materiaal>>().Setup(m => m.Provider).Returns(materialen.Provider);
            mockSet.As<IQueryable<Materiaal>>().Setup(m => m.Expression).Returns(materialen.Expression);
            mockSet.As<IQueryable<Materiaal>>().Setup(m => m.ElementType).Returns(materialen.ElementType);
         
            //mockSet.As<IQueryable<Materiaal>>().Setup(m => m.GetEnumerator()).Returns(0 => materialen.GetEnumerator());


            mockHogentLendContext = new Mock<HoGentLendContext>();
            mockHogentLendContext.Setup(m => m.Materialen).Returns(mockSet.Object);


            repository = new MateriaalRepository(mockHogentLendContext.Object);
        }

        public void FindByFilterString()
        {
            List<Materiaal> materials = repository.FindByFilter(FILTER_WERELDBOL, 0, 0).ToList();

            Assert.AreEqual(1, materials.Count);
            Assert.AreEqual(materials[0].Name, FILTER_WERELDBOL);
        }

        public void FindByFilterStringCaseInsensitive()
        {
            List<Materiaal> materials = repository.FindByFilter(FILTER_WERELDBOL_CAPS, 0, 0).ToList();

            Assert.AreEqual(1, materials.Count);
            Assert.AreEqual(materials[0].Name, FILTER_WERELDBOL);
        }

        public void FindByFilterDoelgroep()
        {
            List<Materiaal> materials = repository.FindByFilter("", ID_KLEUTERONDERWIJS, 0).ToList();

            Assert.AreEqual(2, materials.Count);
            Assert.AreEqual(materials[0].Name, "Wereldbol");
            Assert.AreEqual(materials[1].Name, "Rekenmachine");
        }

        public void FindByFilterLeergebied()
        {
            List<Materiaal> materials = repository.FindByFilter("", 0, ID_WISKUNDE).ToList();

            Assert.AreEqual(1, materials.Count);
            Assert.AreEqual(materials[0].Name, "Rekenmachine");
        }

        public void FindByFilterStringDoelgroepLeergebied()
        {
            List<Materiaal> materials = repository.FindByFilter(FILTER_WERELDBOL, ID_KLEUTERONDERWIJS, ID_AARDRIJKSKUNDE).ToList();

            Assert.AreEqual(1, materials.Count);
            Assert.AreEqual(materials[0].Name, FILTER_WERELDBOL);
        }

        public void FindByFilterNoResult()
        {
            List<Materiaal> materials = repository.FindByFilter(FILTER_WERELDBOL, ID_KLEUTERONDERWIJS, ID_WISKUNDE).ToList();

            Assert.AreEqual(0, materials.Count);
        }



    }
}
