using System.IO;
using System.Net;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using ParttrapDev.Models;
using System.Linq;

namespace jenpstock.Controllers
{
    public class DefaultController : Controller
    {
        GoogleApi google = new GoogleApi();
        
        internal string Url = "";
        internal string StockId = "";
       
        public ActionResult Index()
        {
            JArray productList = google.ProductGet(Url);
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





