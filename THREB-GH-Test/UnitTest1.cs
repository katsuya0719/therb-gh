using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Utils;
//using Grasshopper.Kernel;
//using Rhino.Geometry;
//using Rhino.Geometry.Collections;
//using Rhino.Geometry.Intersect;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using Newtonsoft.Json;
using THERBgh;
using Model;
using System.Net;

namespace THREB_GH_Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void UtilsTest()
        {
            Assert.AreEqual(" 3", Converter.FillEmpty(3, 2));

        }
        [TestMethod]
        public void ExBoxTest()
        {
            var test = new Newtonsoft.Json.Converters.BinaryConverter();
            //var rhino = new Rhino.Geometry.Box();
            ThrebUtilsTest.ExBoxTest();
            Assert.AreEqual(0, 0);
        }
        [TestMethod]
        public void GetDataFromWebTest()
        {
            var wc = new WebClient();
            string text = wc.DownloadString("https://stingray-app-vgak2.ondigitalocean.app/constructions");

            Mock source = JsonConvert.DeserializeObject<Mock>(text);

            //System.Console.WriteLine(text);

        }
    }
}
