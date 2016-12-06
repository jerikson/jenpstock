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

        internal string Url2 = "";
        internal string Url3 = "";

        public ActionResult Index()
        {
            GoogleApi googleObject = new GoogleApi(113298073);
            //googleObject.ProductDelete(Url2);
            //googleObject.ProductInsert(Url2, false);
            //googleObject.ProductUpdate(Url2);
            //List<Google.Apis.ShoppingContent.v2.Data.Product> allProducts = googleObject.ProductsReturn(2, page: 8);
            //googleObject.ProductStatusesReturn(maxResults: 1, page: 1);
            //List<Google.Apis.ShoppingContent.v2.Data.Product> myProduct = googleObject.ProductGetSpecificProducts(Url2);

            return View();
        }


        public List<Model.Product> GetProducts(string url)
        {
            List<JToken> productList = new List<JToken>();
            WebRequest request = WebRequest.Create(url);
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





