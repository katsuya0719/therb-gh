using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Utils;

namespace THREB_GH_Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void UtilsTest()
        {
            Assert.AreEqual(' ' + 3, Converter.FillEmpty(3, 2));

        }
    }
}
