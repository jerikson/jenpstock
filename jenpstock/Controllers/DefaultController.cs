using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace ParttrapDev.Controllers
{
    public class DefaultController : Controller
    {
        internal Stock Stock;
        internal Stock.Product StockProduct;

        List<Stock> StockList = new List<Stock>();

        internal string Url = "";
        internal string StockId = "GOOGLE&relationId=4";
        
        public ActionResult Index()
        {
           
            ViewBag.Product = GetProduct(Url, StockId);

            //ViewBag.ProductTest = GetProductTest(Url + SttockId);
            ViewBag.Title = Url;
            ViewBag.Stockcode = StockId;

            return View();
        }


        public string GetProduct(string url, string stockcode)
        {
            //WebResponse response = request.GetResponse();
            WebRequest request = WebRequest.Create(url+stockcode);
            Stream dataStream = request.GetResponse().GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string response = reader.ReadToEnd();

            return (response);

        }


        string GetProductTest(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
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




    }
}