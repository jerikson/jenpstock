using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace ParttrapDev.Controllers
{
    public class DefaultController : Controller
    {
        internal string Url = "";
        internal string Id = "GOOGLE";

        public ActionResult Index()
        {
            ViewBag.Product = GetProduct(Url, Id);
            ViewBag.Title = Url;
            ViewBag.Stockcode = Id;

            return View();
        }


        public string GetProduct(string url, string stockcode)
        {
            WebRequest request = WebRequest.Create(url+stockcode);
            Stream dataStream = request.GetResponse().GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string response = reader.ReadToEnd();

            return response;

        }




        //WebResponse response = request.GetResponse();





    }
}