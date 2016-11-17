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
            ViewBag.Stockcode = StockId;
            return View(productList);
        }

    }
}





