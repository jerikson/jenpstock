using System;
using System.IO;
using System.Net;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace jenpstock.Controllers
{
    public class DefaultController : Controller
    {
        internal string Url = "http://one.dev.parttrap.com/catalog/getrelatedchildproducts/?stockCode=";
        internal string StockId = "GOOGLE&relationId=4";

        public ActionResult Index()
        {
            List<Model.Product> productList = GetProduct(Url, StockId);
            ViewBag.Stockcode = StockId;
            return View(productList);
        }

        public List<Model.Product> GetProduct(string url, string stockId)
        {
            List<Model.Product> productList = new List<Model.Product>();
            WebRequest request = WebRequest.Create(url + stockId);
            Stream dataStream = request.GetResponse().GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string response = reader.ReadToEnd();
            JArray jsonObj = (JArray)JsonConvert.DeserializeObject(response);

            for (var i = 0; i < jsonObj.Count; i++)
            {
                Model.Product a = (Model.Product)JsonConvert.DeserializeObject(jsonObj[i].ToString(), typeof(Model.Product));
                productList.Add(a);
            }
            return productList;
        }
    }
}





