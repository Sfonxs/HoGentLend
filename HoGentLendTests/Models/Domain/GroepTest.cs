using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HoGentLend.Models.Domain;

namespace HoGentLendTests.Models.Domain
{
    [TestClass]
    public class GroepTest
    {
        [TestMethod]
        public void BasisFirmaTest()
        {
            // eigenlijk heeft Groep geen testen nodig omdat in deze applicatie geen groepen
            // zullen aangemaakt worden (daarom is er ook geen validatie, die gebeurd 
            // al in de java applicatie), maar deze test is voor de 80%+ code coverage.
            Groep groep = new Groep()
            {
                IsLeerGebied = true,
                Name = "GroepName"
            };
            Assert.AreEqual("GroepName", groep.Name);
            Assert.AreEqual(true, groep.IsLeerGebied);
            Assert.AreEqual(0, groep.Id);

        }
    }
}
