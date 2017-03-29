using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using HoGentLend.Controllers;
using HoGentLend.Models.DAL;
using HoGentLend.Models.Domain;
using HoGentLend.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace HoGentLendTests.Controllers
{
    [TestClass]
    public class CatalogusControllerTest
    {

        private CatalogusController controller;
        private DummyDataContext ctx;

        private Mock<IMateriaalRepository> mockMateriaalRepository;
        private Mock<IGroepRepository> mockGroepRepository;
        private Mock<IReservatieRepository> mockReservatieRepository;
        private Mock<IConfigWrapper> mockConfigWrapper;

        private Gebruiker student;
        private Gebruiker lector;
        private const string FILTER_WERELDBOL = "Wereldbol";
        private const int ID_KLEUTERONDERWIJS = 1;
        private const int ID_WISKUNDE = 2;
        private const int ID_AARDRIJKSKUNDE = 3;


        [TestInitialize]
        public void SetupContext()
        {
            ctx = new DummyDataContext();

            mockMateriaalRepository = new Mock<IMateriaalRepository>();
            mockGroepRepository = new Mock<IGroepRepository>();
            mockReservatieRepository = new Mock<IReservatieRepository>();
            mockConfigWrapper = new Mock<IConfigWrapper>();

            student = ctx.GebruikerList.First(u => u.Email.Equals("ruben@hogent.be"));
            lector = ctx.GebruikerList.First(u => u.Email.Equals("lector@hogent.be"));

            /* de logica hierachter wordt getest in de MateriaalRepositoryTest */
            mockMateriaalRepository
                .Setup(m => m.FindByFilter("", 0, 0))
                .Returns(ctx.MateriaalList);

            mockMateriaalRepository
                .Setup(m => m.FindByFilter("Wereldbol", 0, 0))
                .Returns(ctx.MateriaalList.Where(mat => mat.Name.Equals(FILTER_WERELDBOL)));

            mockMateriaalRepository
                .Setup(m => m.FindByFilter("", ID_KLEUTERONDERWIJS, ID_WISKUNDE))
                .Returns(ctx.MateriaalList
                    .Where(mat => mat.Name.Equals("Rekenmachine")));

            mockMateriaalRepository
                .Setup(m => m.FindBy(1))
                .Returns(ctx.MateriaalList.FirstOrDefault(mat => mat.Id.Equals(1)));

            mockMateriaalRepository
                .Setup(m => m.FindBy(2))
                .Returns(ctx.MateriaalList.FirstOrDefault(mat => mat.Id.Equals(2)));

            mockMateriaalRepository
                .Setup(m => m.FindBy(3))
                .Returns(ctx.MateriaalList.FirstOrDefault(mat => mat.Id.Equals(3)));

            mockConfigWrapper
                .Setup(c => c.GetConfig())
                .Returns(ctx.Config);

            controller = new CatalogusController(mockMateriaalRepository.Object, mockGroepRepository.Object, mockReservatieRepository.Object, mockConfigWrapper.Object);
        }

        [TestMethod]
        public void IndexStudentReturnsAllLendableMaterials()
        {
            ViewResult result = controller.Index(student) as ViewResult;
            MateriaalViewModel[] materials = ((IEnumerable<MateriaalViewModel>)result.Model).ToArray();

            /* zie DummyDataContext -> MateriaalList */
            Assert.AreEqual(2, materials.Length);
        }

        [TestMethod]
        public void IndexLectorReturnsAllMaterials()
        {
            ViewResult result = controller.Index(lector) as ViewResult;
            MateriaalViewModel[] materials = ((IEnumerable<MateriaalViewModel>)result.Model).ToArray();

            Assert.AreEqual(3, materials.Length);
        }

        [TestMethod]
        public void IndexFilterStringReturnsFilteredMaterials()
        {
            ViewResult result = controller.Index(lector, FILTER_WERELDBOL) as ViewResult;
            MateriaalViewModel[] materials = ((IEnumerable<MateriaalViewModel>)result.Model).ToArray();

            Assert.AreEqual(1, materials.Length);
            Assert.AreEqual(FILTER_WERELDBOL, materials[0].Name);
        }

        [TestMethod]
        public void IndexFilterDoelgroepLeergebiedReturnsFilteredMaterials()
        {
            ViewResult result = controller.Index(lector, "", ID_KLEUTERONDERWIJS, ID_WISKUNDE) as ViewResult;
            MateriaalViewModel[] materials = ((IEnumerable<MateriaalViewModel>)result.Model).ToArray();

            Assert.AreEqual(1, materials.Length);
            Assert.AreEqual("Rekenmachine", materials[0].Name);
        }

        [TestMethod]
        public void IndexViewBagDoelgroepId()
        {
            Gebruiker g = ctx.GebruikerList.First(u => u.Email.Equals("ruben@hogent.be"));

            ViewResult result = controller.Index(lector, "", ID_KLEUTERONDERWIJS, ID_WISKUNDE) as ViewResult;

            var DoelgroepId= result.ViewBag.DoelgroepId;

            Assert.AreEqual(ID_KLEUTERONDERWIJS, DoelgroepId);
        }

        [TestMethod]
        public void IndexViewBagLeergebiedId()
        {
            Gebruiker g = ctx.GebruikerList.First(u => u.Email.Equals("ruben@hogent.be"));

            ViewResult result = controller.Index(lector, "", ID_KLEUTERONDERWIJS, ID_WISKUNDE) as ViewResult;

            var LeergebiedId = result.ViewBag.LeergebiedId;

            Assert.AreEqual(ID_WISKUNDE, LeergebiedId);
        }

        [TestMethod]
        public void IndexViewBagFilter()
        {
            Gebruiker g = ctx.GebruikerList.First(u => u.Email.Equals("ruben@hogent.be"));

            ViewResult result = controller.Index(lector, "telraam", ID_KLEUTERONDERWIJS, ID_WISKUNDE) as ViewResult;

            var filter = result.ViewBag.filter;

            Assert.AreEqual("telraam", filter);
        }

        [TestMethod]
        public void DetailReturnsMaterial()
        {
            ViewResult result = controller.Detail(student, 1) as ViewResult;

            MateriaalViewModel m = result.Model as MateriaalViewModel;

            Assert.AreEqual(1, m.Id);
            Assert.AreEqual("Wereldbol", m.Name);
        }

        [TestMethod]
        public void DetailReturnsHttpNotFoundWhenMaterialIsNull()
        {
            HttpNotFoundResult result = controller.Detail(student, 10) as HttpNotFoundResult;

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void DetailViewBagLendingPeriod()
        {
            ViewResult result = controller.Detail(student, 1) as ViewResult;

            var lendingPeriod = result.ViewBag.lendingPeriod;

            Assert.AreEqual(1, lendingPeriod);
        }

        [TestMethod]
        public void DetailViewBagInWishListTrue()
        {
            ViewResult result = controller.Detail(student, 1) as ViewResult;

            var inWishList = result.ViewBag.InWishList;

            Assert.IsTrue(inWishList);
        }

        [TestMethod]
        public void DetailViewBagInWishListFalse()
        {
            ViewResult result = controller.Detail(lector, 1) as ViewResult;

            var inWishList = result.ViewBag.InWishList;

            Assert.IsFalse(inWishList);
        }

        [TestMethod]
        public void DetailViewBagReservatiesCountIsOne()
        {
            ViewResult result = controller.Detail(student, 1) as ViewResult;

            var reservaties = result.ViewBag.reservaties;

            Assert.AreEqual(1, reservaties.Count);
        }

        [TestMethod]
        public void DetailViewBagReservatiesCountIsNull()
        {
            ViewResult result = controller.Detail(lector, 3) as ViewResult;

            var reservaties = result.ViewBag.reservaties;

            Assert.AreEqual(0, reservaties.Count);
        }

        [TestMethod]
        public void DetailViewBagChartList()
        {
            ViewResult result = controller.Detail(lector, 3) as ViewResult;

            int[] chartList = result.ViewBag.chartList;

            Assert.AreEqual(15, chartList.Count());
        }
    }
}
