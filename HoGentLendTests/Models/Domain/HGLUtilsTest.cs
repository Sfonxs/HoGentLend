using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoGentLend.Models.Domain
{
    [TestClass]
    public class HGLUtilsTest
    {
        [TestMethod]
        public void TestToSHA256Hash()
        {
            string input = "Een Test String";

            string output = HGLUtils.ToSHA256Hash(input);

            Assert.AreEqual("41d6928462a05401a568eb518f0e10009cd1c165e471941a10ac60b84b4e624f",
                output);
        }
    }
}
