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
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Converters;

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
            //ThrebUtilsTest.ExBoxTest();
            Assert.AreEqual(0, 0);
        }
        [TestMethod]
        public void GetDataFromWebTest()
        {
            var wc = new WebClient();
            string text = wc.DownloadString("https://stingray-app-vgak2.ondigitalocean.app/constructions");
            /*
            string text = @"{
  ""data"": [
    {
      ""category"": ""exteriorWall"", 
      ""description"": ""from therb"", 
      ""id"": ""1"", 
      ""materials"": [
        {
          ""classification"": 1, 
          ""conductivity"": 1.2, 
          ""density"": 2200.0, 
          ""description"": ""from Therb"", 
          ""id"": ""2"", 
          ""name"": ""concrete"", 
          ""specificHeat"": 840.0
        }
      ], 
      ""name"": ""RC exterior wall"", 
      ""tags"": [
        {
          ""id"": ""1"", 
          ""name"": ""RC\u9020""
        }
      ], 
      ""thickness"": [
        120.0
      ]
    }, 
    {
      ""category"": ""interiorFloor"", 
      ""description"": ""from therb"", 
      ""id"": ""3"", 
      ""materials"": [
        {
          ""classification"": 1, 
          ""conductivity"": 0.111, 
          ""density"": 550.0, 
          ""description"": ""from Therb"", 
          ""id"": ""3"", 
          ""name"": ""plywood"", 
          ""specificHeat"": 1880.0
        }, 
        {
          ""classification"": 1, 
          ""conductivity"": 0.037, 
          ""density"": 32.0, 
          ""description"": ""from Therb"", 
          ""id"": ""4"", 
          ""name"": ""polyurethane"", 
          ""specificHeat"": 840.0
        }
      ], 
      ""name"": ""interior floor"", 
      ""tags"": [], 
      ""thickness"": [
        15.0, 
        45.0
      ]
    }
  ], 
  ""message"": ""could retrieve constructions"", 
  ""status"": ""success""
}";
            */
            //var source = JsonConvert.DeserializeObject<Mock_TEST>(text);
            var source = JsonConvert.DeserializeObject<Mock_TEST>(text);
            //List<Person> persons = JsonConvert.DeserializeObject<List<Person>>(json, new PersonConverter());
            //source.AfterRun();

            //System.Console.WriteLine(text);

        }

        public class Mock_TEST
        {
            //[JsonProperty(ItemConverterType = typeof(ConstructionConverter_TEST))]
            public List<Construction_TEST> data = new List<Construction_TEST>();
        }

        public class Construction_TEST
        {
            public int id;
            [JsonProperty(PropertyName = "category")]
            public ElementType categories;
            public List<Material> materials;
            public List<Double> thickness;
        }

        public class ConstructionConverter_TEST : Newtonsoft.Json.Converters.CustomCreationConverter<Construction_TEST>
        {
            public override Construction_TEST Create(Type objectType)
            {
                return new Construction_TEST();
            }
        }

        /*
        public class ConstructionConverter_TEST : JsonCreationConverter<Construction_TEST>
        {
            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                throw new NotImplementedException();
            }

            protected override Construction_TEST Create(Type objectType, JObject jObject)
            {
                return new Construction_TEST();
            }

            private bool FieldExists(string fieldName, JObject jObject)
            {
                return jObject[fieldName] != null;
            }
        }


        public abstract class JsonCreationConverter<T> : JsonConverter
        {
            /// <summary>
            /// Create an instance of objectType, based properties in the JSON object
            /// </summary>
            /// <param name="objectType">type of object expected</param>
            /// <param name="jObject">
            /// contents of JSON object that will be deserialized
            /// </param>
            /// <returns></returns>
            protected abstract T Create(Type objectType, JObject jObject);

            public override bool CanConvert(Type objectType)
            {
                return typeof(T).IsAssignableFrom(objectType);
            }

            public override bool CanWrite
            {
                get { return false; }
            }

            public override object ReadJson(JsonReader reader,
                                            Type objectType,
                                             object existingValue,
                                             JsonSerializer serializer)
            {
                // Load JObject from stream
                JObject jObject = JObject.Load(reader);

                // Create target object based on JObject
                T target = Create(objectType, jObject);

                // Populate the object properties
                serializer.Populate(jObject.CreateReader(), target);

                return target;
            }
        }
        */

        public enum NamedEnum
        {
            [EnumMember(Value = "@first")]
            First,

            [EnumMember(Value = "@second")]
            Second,
            Third
        }

        private class EnumColList<T>
        {
            private List<EnumContainer<T>> data;
        }
        private class EnumContainer<T>
        {
            public T Number { get; set; }
        }

        [TestMethod]
        public void DeserializeNameEnumTest()
        {
            string json = @"{
  ""Number"": ""@first""
}";

            EnumContainer<NamedEnum> c = JsonConvert.DeserializeObject<EnumContainer<NamedEnum>>(json, new StringEnumConverter());
            Assert.AreEqual(NamedEnum.First, c.Number);

            json = @"{
  ""Number"": ""Third""
}";

            c = JsonConvert.DeserializeObject<EnumContainer<NamedEnum>>(json, new StringEnumConverter());
            Assert.AreEqual(NamedEnum.Third, c.Number);

            json = @"{
    ""data"" : [
        {
            ""Number"": ""Third"", 
            ""Number"": ""@first"", 
            ""Number"": ""@second"", 
            ""Number"": ""@first""
        }
    ],
}";

            var d = JsonConvert.DeserializeObject<EnumColList<NamedEnum>>(json, new StringEnumConverter());
        }

    }
}
