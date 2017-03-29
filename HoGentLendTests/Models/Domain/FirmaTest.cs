using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HoGentLend.Models.Domain;

namespace HoGentLendTests.Models.Domain
{
    [TestClass]
    public class FirmaTest
    {
        [TestMethod]
        public void BasisFirmaTest()
        {
            // eigenlijk heeft firma geen testen nodig omdat in deze applicatie geen firmas
            // zullen aangemaakt worden (daarom is er ook geen validatie, die gebeurd 
            // al in de java applicatie), maar deze test is voor de 80%+ code coverage.
            Firma firma = new Firma()
            {
               Email = "TestEmail@email.com",
               Name = "TestFirma"
            };
            Assert.AreEqual("TestEmail@email.com", firma.Email);
            Assert.AreEqual("TestFirma", firma.Name);
            Assert.AreEqual(0, firma.Id);

        }
    }
}
