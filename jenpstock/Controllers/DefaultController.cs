using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;

namespace jenpstock.Controllers
{
    public class DefaultController : Controller
    {
        List<Model> StockList = new List<Model>();


        internal string Url = "http://one.dev.parttrap.com/catalog/getrelatedchildproducts/?stockCode=";
        internal string StockId = "GOOGLE&relationId=4";

        public ActionResult Index()
        {

            ViewBag.Product = GetProduct(Url, StockId);
            //ViewBag.ProductTest = GetProductTest(Url + StockId);
            // GetAnotherProduct(Url, StockId);



            ViewBag.Title = Url;
            ViewBag.Stockcode = StockId;

            return View(StockList);
        }

        public void GetAnotherProduct(string url, string id)
        {
            using (var client = new WebClient())
            {
                var json = client.DownloadString(url + id);
                var serializer = new JavaScriptSerializer();
                StockList.Add(serializer.Deserialize<Model>(json));
            }
        }





        public JToken GetProduct(string url, string stockId)
        {
            WebRequest request = WebRequest.Create(url + stockId);
            Stream dataStream = request.GetResponse().GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);

            string response = reader.ReadToEnd();
            JArray jsonObj = (JArray)JsonConvert.DeserializeObject(response);

            Debug.WriteLine("Key" + "\t\t" + "Value");

            foreach (var item in jsonObj)
            {
                Debug.WriteLine(item["ProductID"].ToString());
            }


            return jsonObj[0];
        }





        //public string GetProduct(string url, string stockcode)
        //{
        //    //WebResponse response = request.GetResponse();
        //    WebRequest request = WebRequest.Create(url + stockcode);
        //    Stream dataStream = request.GetResponse().GetResponseStream();
        //    StreamReader reader = new StreamReader(dataStream);
        //    string response = reader.ReadToEnd();
        //    string text = System.IO.File.ReadAllText(@"D:\dev\Github\jenpstock\jenpstock\Content\stock.txt");

        //    Model.RootObject obj = JsonConvert.DeserializeObject<Model.RootObject>(text);

        //    return text;

        //}


        //}



        //public Model.Product GetProduct(string url, string stockcode)
        //{
        //    WebRequest request = WebRequest.Create(url + stockcode);
        //    Stream dataStream = request.GetResponse().GetResponseStream();
        //    StreamReader reader = new StreamReader(dataStream);
        //    string response = reader.ReadToEnd();
        //    Model.Product deserializedProduct = JsonConvert.DeserializeObject<Model.Product>(response);


        //    return deserializedProduct;

        //}




        //public Model.Product GetProduct(string url, string stockcode)
        //{

        //    //WebResponse response = request.GetResponse();
        //    WebRequest request = WebRequest.Create(url + stockcode);
        //    Stream dataStream = request.GetResponse().GetResponseStream();
        //    StreamReader reader = new StreamReader(dataStream);
        //    string response = reader.ReadToEnd();
        //    Model.Product deserializedProduct = JsonConvert.DeserializeObject<Model.Product>(response);

        //    return deserializedProduct;
        //}

        public string GetProductTest(string url)
        {
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(url);
            try
            {
                WebResponse response = request.GetResponse();
                using (Stream responseStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                    return reader.ReadToEnd();
                }
            }
            catch (WebException e)
            {
                WebResponse errorResponse = e.Response;
                using (Stream responseStream = errorResponse.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.GetEncoding("utf-8"));
                    String errorText = reader.ReadToEnd();
                    Console.WriteLine("Error: " + errorResponse);
                }
                throw;
            }
        }


        
        public void Test(string url, string stockId)
        {
            WebClient c = new WebClient();
            string downloadjson = url + stockId;
            var json = c.DownloadString(downloadjson);
            Model shiet = Newtonsoft.Json.JsonConvert.DeserializeObject<Model>(json);

            Model.Product shietSource = new Model.Product()
            {
                ProductID = 1,
                Description = "desc test"
            };


        }


        
    }
}



















