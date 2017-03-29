using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HoGentLend.Models.Domain;

namespace HoGentLendTests.Models.Domain
{
    [TestClass]
    public class VerlangLijstTest
    {
        
        private VerlangLijst wishList;
        System.Collections.Generic.IList<Materiaal> Materials;
        Materiaal m1;
        Materiaal m2;
        Materiaal m3;

        [TestInitialize()]
        public void MyTestInitialize()
        {
            m1 = new Materiaal();
            m1.Name = "Wereldbol";
            m1.Amount = 3;

            m2 = new Materiaal();
            m2.Name = "Rekenmachine";
            m2.Amount = 5;

            m3 = new Materiaal();
            m3.Name = "Geodriehoek";
            m3.Amount = 8;

            wishList = new VerlangLijst();
            Materials = new System.Collections.Generic.List<Materiaal> { m1, m2 };
            wishList.Materials = Materials;
        }



        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void AddMaterialAlreadyInWishlistFails()
        {
            wishList.AddMaterial(m2);
        }

        [TestMethod()]
        public void AddMaterialCauses3Materials()
        {
            wishList.AddMaterial(m3);
            Assert.AreEqual(3, wishList.Materials.Count);
        }

        [TestMethod()]
        public void RemoveMaterialCauses1Materials()
        {
            wishList.RemoveMaterial(m2);
            Assert.AreEqual(1, wishList.Materials.Count);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void RemoveMaterialNotInWishlistFails()
        {
            wishList.RemoveMaterial(m3);
        }

        [TestMethod]
        public void TestContainsReturnsTrueForExistingItem()
        {
            Assert.IsTrue(wishList.Contains(m1));
        }

        [TestMethod]
        public void TestContainsReturnsFalseForExistingItem()
        {

            Assert.IsFalse(wishList.Contains(m3));
        }
        [TestMethod]
        public void TestContainsReturnsFalseForNull()
        {

            Assert.IsFalse(wishList.Contains(null));
        }
    }
}
