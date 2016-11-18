using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;

namespace jenpstock.Controllers
{
    public class DefaultController : Controller
    {
        internal string Url = "";
        internal string StockId = "GOOGLE&relationId=4";


        public ActionResult Index()
        {
            List<Model.Product> productList = GetProducts(Url, StockId);

            ViewBag.Title = Url;
            ViewBag.Stockcode = StockId;
            
            return View(productList);
        }

        public List<Model.Product> GetProducts(string url, string stockId)
        {
            List<JToken> productList = new List<JToken>();
            WebRequest request = WebRequest.Create(url + stockId);
            Stream dataStream = request.GetResponse().GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);

            string response = reader.ReadToEnd();
            JArray jsonObj = (JArray)JsonConvert.DeserializeObject(response);

            List<Model.Product> listan = new List<Model.Product>();

            int index = 0;
            foreach (var item in jsonObj)
            {
                Model.Product a = (Model.Product)JsonConvert.DeserializeObject(jsonObj[index].ToString(), typeof(Model.Product));
                listan.Add(a);
                index++;
            }

            return listan;
        }

    }


}











//public JToken GetProduct(string url, string stockId)
//{
//    WebRequest request = WebRequest.Create(url + stockId);
//    Stream dataStream = request.GetResponse().GetResponseStream();
//    StreamReader reader = new StreamReader(dataStream);

//    string response = reader.ReadToEnd();
//    JArray jsonObj = (JArray)JsonConvert.DeserializeObject(response);

//    Debug.WriteLine("Key" + "\t\t" + "Value");

//    foreach (var item in jsonObj)
//    {
//        Debug.WriteLine(item["ProductID"].ToString());
//    }

//    return jsonObj[0];
//}




