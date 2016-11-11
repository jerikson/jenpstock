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

            Model.Product[] stockList= WeAreTesting(Url, StockId);
            

            return View(stockList);
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


        
        public Model.Product[] WeAreTesting(string url, string stockId)
        {
            WebClient c = new WebClient();
            string downloadjson = url + stockId;
            var json = c.DownloadString(downloadjson);
            Rootobject shiet = Newtonsoft.Json.JsonConvert.DeserializeObject<Rootobject>(json);

            //Model.Product shietSource = new Model.Product()
            //{
            //    ProductID = shiet.product.ProductID,
            //    Description = shiet.Description
            //};

            return shiet.response;
        }

        
    }
}



















