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
using System.Net.Http;
using System.Net.Http.Headers;
using System.IO;
using System.Threading.Tasks;
using System.Text;
using System.Threading;

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

            #region test
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

            #endregion

            var source = JsonConvert.DeserializeObject<ResConstruction>(text);
        }

        /*
        [TestMethod]
        public void PostDataFromWebByRSTest()
        {

            var client = new RestClient("https://oyster-app-8jboe.ondigitalocean.app/therb/run");
            //client.Timeout = -1;
            var request = new RestRequest()
            {
                Timeout = -1,
                AlwaysMultipartFormData = true,
                Method = Method.Post
            };
            request.AddFile("dataset", @"C:\therb\TEST1.zip");

            //request.AlwaysMultipartFormData = true;
            //var response = client.Execute(request);
            //var response = client.GetAsync(request, cancellationToken);
            for(int i = 0; i < 20; i++)
            {
                try
                {
                    var response = client.Post(request);
                    //Console.WriteLine();
                    if(response.StatusCode == HttpStatusCode.OK)
                    {
                        Console.WriteLine(response.Content);
                        break;
                    }
                }
                catch (Exception ex)
                {
                    //Console.WriteLine(ex.Message);
                }
            }

        }
        */
        [TestMethod]
        public void PostDataFromWebByHTestAsync()
        {

            //送信するファイルへのパス
            string filePath = @"C:\therb\TEST1.zip";
            string fileName = "TEST1.zip";

            string url = @"https://oyster-app-8jboe.ondigitalocean.app/therb/run";

            // ファイルのみでなく文字列も送信してみる
            /*
            string strData = "これは、テストです。";
            StringContent stringContent = new StringContent(strData);
            stringContent.Headers.ContentDisposition =
                    new ContentDispositionHeaderValue("form-data")
                    {
                        Name = "comment"
                    };
            content.Add(stringContent);
            */

            // アップロードするファイル
                //streamContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");

                // メソッド (POST) と送信先の URL 指定
            for (int i = 0;i < 20; i++)
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    try
                    {
                        MultipartFormDataContent content = new MultipartFormDataContent();

                        StreamContent streamContent = new StreamContent(fs);
                        streamContent.Headers.ContentDisposition =
                               new ContentDispositionHeaderValue("form-data")
                               {
                                   Name = "dataset",
                                   FileName = fileName
                               };

                        content.Add(streamContent);
                        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url);
                        request.Content = content;
                        var responseTask = new HttpClient().SendAsync(request); /*{ Timeout = new TimeSpan(0, 0, 10)}*/
                        responseTask.Wait();
                        var response = responseTask.Result;
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            Console.WriteLine(response.Content);
                            break;
                        }
                        Console.WriteLine(response.StatusCode);
                    }
                    catch(Exception e)
                    {
                        Console.WriteLine("error: " + e.Message);
                        Task.Delay(1000);
                        //Thread.Sleep(1000);
                    }
                }
                // ここでファイルを HTTP ストリームに書き込むので、
                // 以下は using の { } 内にないとファイルが読めな
                // いというエラーになる
                // 応答のコンテンツを Stream として取得
                /*
                using (Stream responseStream = await response.Content.ReadAsStreamAsync())
                {
                    using (StreamReader sr =
                        new StreamReader(responseStream, Encoding.UTF8))
                    {
                        Console.WriteLine(sr.ReadToEnd());
                    }
                }*/
            }
        }

    }
}
