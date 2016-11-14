using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace jenpstock.Controllers
{
    public class DefaultController : Controller
    {
        internal string Url = "";
        internal string StockId = "GOOGLE&relationId=4";

        public ActionResult Index()
        {
            JToken oneProduct = GetProduct(Url, StockId);

            ViewBag.Title = Url;
            ViewBag.Stockcode = StockId;
            
            return View(oneProduct);
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




