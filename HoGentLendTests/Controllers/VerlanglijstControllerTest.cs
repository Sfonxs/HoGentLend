using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using HoGentLend.Controllers;
using HoGentLend.Models.Domain;
using HoGentLend.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Microsoft.CSharp;

namespace HoGentLendTests.Controllers
{
    [TestClass]
    public class VerlanglijstControllerTest
    {
        private VerlanglijstController controller;
        private Mock<IMateriaalRepository> mockMateriaalRepository;
        private Mock<IGebruikerRepository> mockGebruikerRepository;
        private Mock<IConfigWrapper> mockConfigWrapper;
        private Gebruiker student, lector;

        private int WERELDBOL_ID = 1;
        private int ONBESCHKBAAR_ID = 2;
        private int ONGELDIG_ID = 0;
        private DummyDataContext ctx;
        private Materiaal wereldbol;

        [TestInitialize]
        public void SetupContext()
        {
            ctx = new DummyDataContext();
            var firstName = "Firstname";
            var lastName = "Lastname";
            var email = "Firstname.lastname@hogent.be";
            this.student = new Student(firstName, lastName, email);
            this.lector = new Lector(firstName, lastName, email);

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
            controller = new VerlanglijstController(mockMateriaalRepository.Object, mockConfigWrapper.Object);
            this.wereldbol = new Materiaal()
            {
                Name = "Wereldbol",
                Amount = 5,
                IsLendable = true
            };
            mockMateriaalRepository.Setup(m => m.FindBy(WERELDBOL_ID)).Returns(wereldbol);
            mockMateriaalRepository.Setup(m => m.FindBy(ONBESCHKBAAR_ID)).Returns(new Materiaal()
            {
                Name = "Wereldbol",
                Amount = 5,
                IsLendable = false
            });
            mockMateriaalRepository.Setup(m => m.FindBy(ONGELDIG_ID)).Returns(null as Materiaal);
        }

        [TestMethod]
        public void IndexReturnsAllMaterialsWishListAnViewBag()
        {
            Gebruiker g = ctx.GebruikerList.First(u => u.Email.Equals("ruben@hogent.be"));
            // Act
            ViewResult result = controller.Index(g) as ViewResult;

            MateriaalViewModel[] materials = ((IEnumerable<MateriaalViewModel>)result.Model).ToArray();

            // Assert
            Assert.AreEqual(2, materials.Length);
            Assert.AreEqual("Rekenmachine", materials[0].Name);
            Assert.AreEqual(2, materials[0].Amount);
            var vb = result.ViewBag;
            Assert.AreEqual("10:30", vb.ophaalTijd);
            Assert.AreEqual("17:30", vb.indienTijd);
            Assert.AreEqual("maandag", vb.ophaalDag);
            Assert.AreEqual("vrijdag", vb.indienDag);
            Assert.AreEqual(0, vb.aantalWeken);
            Assert.AreEqual(DateTime.Now.ToString("dd/MM/yyyy"), vb.vandaag);
        }

        [TestMethod]
        public void IndexViewBagOphaalDag()
        {
            Gebruiker g = ctx.GebruikerList.First(u => u.Email.Equals("ruben@hogent.be"));

            ViewResult result = controller.Index(g) as ViewResult;

            var ophaalDag = result.ViewBag.ophaalDag;

            Assert.AreEqual("maandag", ophaalDag);
        }

        [TestMethod]
        public void IndexViewBagIndienDag()
        {
            Gebruiker g = ctx.GebruikerList.First(u => u.Email.Equals("ruben@hogent.be"));

            ViewResult result = controller.Index(g) as ViewResult;

            var indienDag = result.ViewBag.indienDag;

            Assert.AreEqual("vrijdag", indienDag);
        }

        [TestMethod]
        public void IndexViewBagAantalWeken()
        {
            Gebruiker g = ctx.GebruikerList.First(u => u.Email.Equals("ruben@hogent.be"));

            ViewResult result = controller.Index(g) as ViewResult;

            var aantalWeken = result.ViewBag.aantalWeken;

            Assert.AreEqual(0, aantalWeken);
        }

        [TestMethod]
        public void IndexViewBagOphaalTijd()
        {
            Gebruiker g = ctx.GebruikerList.First(u => u.Email.Equals("ruben@hogent.be"));

            ViewResult result = controller.Index(g) as ViewResult;

            var ophaalTijd = result.ViewBag.ophaalTijd;

            Assert.AreEqual("10:30", ophaalTijd);
        }

        [TestMethod]
        public void IndexViewBagIndienTijd()
        {
            Gebruiker g = ctx.GebruikerList.First(u => u.Email.Equals("ruben@hogent.be"));

            ViewResult result = controller.Index(g) as ViewResult;

            var indienTijd = result.ViewBag.indienTijd;

            Assert.AreEqual("17:30", indienTijd);
        }

        [TestMethod]
        public void AddSuccesfulAddsToWishListAndSavesAndReturnsMessage()
        {
            JsonResult result = controller.Add(student, WERELDBOL_ID);
            var jsonData = result.Data.GetType().GetProperty("status");
            var propertyStatus = jsonData.GetValue(result.Data, null);

            Assert.AreEqual(1, student.WishList.Materials.Count);
            mockMateriaalRepository.Verify(m => m.SaveChanges(), Times.Once);

            Assert.IsTrue(propertyStatus.Equals("success"));
        }

        [TestMethod]
        public void AddInvalidIdDoesNotToWishListAndDoesNotSavAndReturnsError()
        {
            JsonResult result = controller.Add(student, ONGELDIG_ID);
            var jsonData = result.Data.GetType().GetProperty("status");
            var propertyStatus = jsonData.GetValue(result.Data, null);

            Assert.AreEqual(0, student.WishList.Materials.Count);
            mockMateriaalRepository.Verify(m => m.SaveChanges(), Times.Never);

            Assert.IsTrue(propertyStatus.Equals("error"));
        }

        [TestMethod]
        public void TestAddCanNotAddToStudentWishListIfMaterialIsUnavailable()
        {
            JsonResult result = controller.Add(student, ONBESCHKBAAR_ID);
            var jsonData = result.Data.GetType().GetProperty("status");
            var propertyStatus = jsonData.GetValue(result.Data, null);

            Assert.AreEqual(0, student.WishList.Materials.Count);
            mockMateriaalRepository.Verify(m => m.SaveChanges(), Times.Never);

            Assert.IsTrue(propertyStatus.Equals("error"));

        }

        [TestMethod]
        public void TestRemoveRemovesMaterialsFromWishListAndSavesAndReturnsMsg()
        {
            student.WishList.Materials.Add(wereldbol);

            controller.Remove(student, WERELDBOL_ID);

            Assert.AreEqual(0, student.WishList.Materials.Count);
            mockMateriaalRepository.Verify(m => m.SaveChanges(), Times.Once);
            Assert.IsTrue(controller.TempData["msg"] != null);
        }
        [TestMethod]
        public void TestRemoveDoesNotRemoveIfMaterialsIsInvalidAndDoesNotSaveAndReturnsError()
        {
            student.WishList.Materials.Add(wereldbol);

            controller.Remove(student, ONGELDIG_ID);

            Assert.AreEqual(1, student.WishList.Materials.Count);
            mockMateriaalRepository.Verify(m => m.SaveChanges(), Times.Never);
            Assert.IsTrue(controller.TempData["err"] != null);
        }
        [TestMethod]
        public void TestRemoveDoesNotRemoveIfMaterialsIsNotInListAndDoesNotSaveAndReturnsError()
        {
            student.WishList.Materials.Add(wereldbol);

            controller.Remove(student, ONBESCHKBAAR_ID);

            Assert.AreEqual(1, student.WishList.Materials.Count);
            mockMateriaalRepository.Verify(m => m.SaveChanges(), Times.Never);
            Assert.IsTrue(controller.TempData["err"] != null);
        }

        [TestMethod]
        public void TestRemovePostRemovesMaterialsFromWishListAndSavesAndReturnsMsg()
        {
            student.WishList.Materials.Add(wereldbol);

            JsonResult result = controller.RemovePost(student, WERELDBOL_ID);
            var jsonData = result.Data.GetType().GetProperty("status");
            var propertyStatus = jsonData.GetValue(result.Data, null);


            Assert.AreEqual(0, student.WishList.Materials.Count);
            mockMateriaalRepository.Verify(m => m.SaveChanges(), Times.Once);
            Assert.IsTrue(propertyStatus.Equals("success"));
        }
        [TestMethod]
        public void TestRemovePostDoesNotRemoveIfMaterialsIsInvalidAndDoesNotSaveAndReturnsError()
        {
            student.WishList.Materials.Add(wereldbol);

            JsonResult result = controller.RemovePost(student, ONGELDIG_ID);
            var jsonData = result.Data.GetType().GetProperty("status");
            var propertyStatus = jsonData.GetValue(result.Data, null);

            Assert.AreEqual(1, student.WishList.Materials.Count);
            mockMateriaalRepository.Verify(m => m.SaveChanges(), Times.Never);
            Assert.IsTrue(propertyStatus.Equals("error"));
        }
        [TestMethod]
        public void TestRemovePostDoesNotRemoveIfMaterialsIsNotInListAndDoesNotSaveAndReturnsError()
        {
            student.WishList.Materials.Add(wereldbol);

            JsonResult result = controller.RemovePost(student, ONBESCHKBAAR_ID);
            var jsonData = result.Data.GetType().GetProperty("status");
            var propertyStatus = jsonData.GetValue(result.Data, null);

            Assert.AreEqual(1, student.WishList.Materials.Count);
            mockMateriaalRepository.Verify(m => m.SaveChanges(), Times.Never);
            Assert.IsTrue(propertyStatus.Equals("error"));
        }
    }
}
