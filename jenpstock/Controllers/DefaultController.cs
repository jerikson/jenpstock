using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.UI.WebControls;
using System.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Serialization;
using System.Runtime.Remoting;

namespace jenpstock.Controllers
{
    public class DefaultController : Controller
    {
        List<Model> StockList = new List<Model>();


        internal string Url = "";
        internal string StockId = "GOOGLE&relationId=4";

        public ActionResult Index()
        {

            //ViewBag.Product = GetProduct(Url, StockId);
            //ViewBag.ProductTest = GetProductTest(Url + StockId);

            //GetAnotherProduct(Url, StockId);
            ViewBag.Title = Url;
            ViewBag.Stockcode = StockId;


            JToken oneProduct = HeyMan(Url, StockId);

            //Call under construction method
            //List<JToken> stockList= WeAreTesting(Url, StockId);
            

            return View(oneProduct);
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

        public string GetProduct(string url, string stockcode)
        {
            //WebResponse response = request.GetResponse();
            WebRequest request = WebRequest.Create(url + stockcode);
            Stream dataStream = request.GetResponse().GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string response = reader.ReadToEnd();

            return (response);

        }

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


        
        public List<JToken> WeAreTesting(string url, string stockId)
        {
            WebClient c = new WebClient();
            string downloadjson = url + stockId;
            var json = c.DownloadString(downloadjson);
            Rootobject shiet = Newtonsoft.Json.JsonConvert.DeserializeObject<Rootobject>(json);
            
            JArray jsonArray = JArray.Parse(downloadjson);

            //Model.Product shietSource = new Model.Product()
            //{
            //    ProductID = shiet.product.ProductID,
            //    Description = shiet.Description
            //};

            return jsonArray.ToList();
        }

        public void WeAreTestingCoinflip()
        {
            string url = "http://winfastlosefaster.ecstudenter.se/Games/ListCoinflipGames";
            WebClient c = new WebClient();
            string downloadjson = url;
            var json = c.DownloadString(downloadjson);
            Rootobject shiet = Newtonsoft.Json.JsonConvert.DeserializeObject<Rootobject>(json);

            JArray jsonArray = JArray.Parse(downloadjson);


        }

        public JToken HeyMan(string url, string stockId)
        {
            WebRequest request = WebRequest.Create(url + stockId);
            Stream dataStream = request.GetResponse().GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);

            string response = reader.ReadToEnd();
            //dynamic jsonObj = JsonConvert.DeserializeObject(response);
            //JObject jsonObj = (JObject)JsonConvert.DeserializeObject(response);
            JArray jsonObj = (JArray)JsonConvert.DeserializeObject(response);

            
            Debug.WriteLine("Key" + "\t\t" + "Value");
            foreach (var item in jsonObj)
            {
                Debug.WriteLine(item["ProductID"].ToString());
                //Debug.WriteLine(item.ToString());
            }


            return jsonObj[0];

        }


    }
}



















