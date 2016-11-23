using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.ShoppingContent.v2;
using Google.Apis.ShoppingContent.v2.Data;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System;
using System.Linq;
using jenpstock;
using System.Diagnostics;

namespace ParttrapDev.Models
{
    public class GoogleApi
    {
        private UserCredential _credential;
        private ShoppingContentService _service;
        private ulong _merchantID;
        private string _clientId = "896777409399-ghva93bgs7qpv293tqj1vp4eefi7n82c.apps.googleusercontent.com";
        private string _clientSecret = "Or7cg3mMtWmMsxIhBjecHcRq";


        public GoogleApi(ulong merchantID)
        {
            _credential = Authenticate();
            _service = CreateService(_credential);
            _merchantID = merchantID;
        }

        private UserCredential Authenticate()
        {
            string[] scopes = new string[] { ShoppingContentService.Scope.Content };

            var credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                          new ClientSecrets
                          {
                              ClientId = _clientId,
                              ClientSecret = _clientSecret
                          },
                          scopes,
                          "user",
                          System.Threading.CancellationToken.None).Result;

            return credential;
        }

        private ShoppingContentService CreateService(UserCredential credential)
        {
            var service = new ShoppingContentService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "jenpbiz",
            });
            return service;
        }

        private JArray ProductGet(string url)
        {
            //WebClient version
            WebClient client = new WebClient();
            JArray jsonObj = new JArray();
            using (WebClient client2 = new WebClient())
            {
                try
                {
                    string json = client.DownloadString(url);
                    jsonObj = JArray.Parse(json);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Message: " + e.Message + "\nStacktrace" + e.StackTrace + "\nTarget: " + e.TargetSite);
                }
            }


            // WebRequest ska tydligen vara bättre för stora överföringar? Ska tydligen inte blocka interface 
            // thread, men MVC har inte interface thread? Det har väl bara WPF o sånt?
            //WebRequest version
            //WebRequest request = WebRequest.Create(url);
            //Stream dataStream = request.GetResponse().GetResponseStream();
            //StreamReader reader = new StreamReader(dataStream);
            //string response = reader.ReadToEnd();
            //JArray jsonObj = JArray.Parse(response);


            return jsonObj;
        }


        ///<summary>
        ///Inserts products to a Google Shopping account using custombatch method to send multiple requests as one.
        ///<para>productUrl is a URI to something that returns a JSON object.</para>
        /// </summary>
        public void ProductInsert(string productUrl)
        {
            JArray productsToPush = ProductGet(productUrl);
            ProductsCustomBatchRequest batchRequest = new ProductsCustomBatchRequest();
            batchRequest.Entries = new List<ProductsCustomBatchRequestEntry>();

            //foreach (var product in productsToPush)
            //{

            //    Google.Apis.ShoppingContent.v2.Data.Product newProduct = new Google.Apis.ShoppingContent.v2.Data.Product()
            //    {
            //        OfferId = product["ProductID"].ToString(),
            //        Title = "My Test Product: " + i,
            //        Description = "This is a test product that I made. It is number " + i + " in a series of " + productsToMake + " that I will create.",
            //        Link = "https://www.example.com/products/Product?productId=" + i,
            //        ImageLink = "https://www.example.com/productImages/ProductImage?productId=" + i + "&imageIndex=0",
            //        ContentLanguage = "sv",
            //        TargetCountry = "SE",
            //        Channel = "online",
            //        Availability = "out of stock",
            //        Condition = "new",
            //        GoogleProductCategory = "3219",
            //        IdentifierExists = false,
            //        Price = new Google.Apis.ShoppingContent.v2.Data.Price()
            //        {
            //            Currency = "SEK",
            //            Value = i + "00"
            //        }
            //    };

            //    ProductsCustomBatchRequestEntry newEntry = new ProductsCustomBatchRequestEntry()
            //    {
            //        Method = "insert",
            //        BatchId = long.Parse(newProduct.OfferId),
            //        MerchantId = _merchantID,
            //        Product = newProduct
            //    };

            //    batchRequest.Entries.Add(newEntry);

            //}





            //        OfferId = "Unique Id",
            //        Title = "I am a title to a product",
            //        Description = "I am a product description.",
            //        Link = "https://www.example.com/products/Product?productId=1",
            //        ImageLink = "https://www.example.com/productImages/ProductImage?productId=1",
            //        ContentLanguage = "sv",
            //        TargetCountry = "SE",
            //        Channel = "online",
            //        Availability = "out of stock",
            //        Condition = "new",
            //        GoogleProductCategory = "3219",
            //        IdentifierExists = false,
            //        Currency = "SEK",
            //        Price = "100"

            // Saknade Google Attributer: 
            // Title, ContentLanguage, TargetCountry,
            // Availability, Condition, GoogleProductCategory, 
            //--------------------------------------------------------------------------
            //GÖRA EFTER LUNCH:
            //Mappa och skriva ut dem attributer jag kan här.
            //Sen implementera dem i riktiga insert ovanför.
            foreach (var product in productsToPush)
            {
                Debug.WriteLine("OfferId: " + product["ProductID"].ToString());
                Debug.WriteLine("Description: " + product["Description"].ToString());
                Debug.WriteLine("Price, incl VAT: " + product["NetPriceInclVAT"]["Amount"].ToString());
                Debug.WriteLine("Currency: " + product["NetPriceInclVAT"]["Currency"]["Code"].ToString());
                Debug.WriteLine("Product Page Link: " + product["AdditionalValues"]["DetailLink"].ToString());
                Debug.WriteLine("Image Link: " + product["AdditionalValues"]["ImageUrl"].ToString());
                //Debug.WriteLine(product[].ToString());
                //Debug.WriteLine(product[].ToString());
                //Debug.WriteLine(product[].ToString());
                //Debug.WriteLine(product[].ToString());
                break;
            }


            try
            {
                ProductsResource.CustombatchRequest reeeq = _service.Products.Custombatch(batchRequest);
                reeeq.Execute();
            }
            catch (Exception e)
            {
               System.Diagnostics.Debug.WriteLine("EXCEPTION THROWN @ProductInsert()");
               System.Diagnostics.Debug.WriteLine("Message: " + e.Message);
               System.Diagnostics.Debug.WriteLine("Stack Trace: " + e.StackTrace);
               System.Diagnostics.Debug.WriteLine("Target Site: " + e.TargetSite);
            }

            return;
        }


        public void ProductUpdate()
        {

        }

        ///<summary>
        ///Deletes products from a Google Shopping account using custombatch method to send multiple requests as one.
        ///<para>productUrl is a URI to something that returns a JSON object.</para>
        /// </summary>
        public void ProductDelete(string productUrl)
        {
            JArray productsToDelete = ProductGet(productUrl);

            ProductsCustomBatchRequest batchRequest = new ProductsCustomBatchRequest();
            batchRequest.Entries = new List<ProductsCustomBatchRequestEntry>();
            int batchId = 1;

            //channel:language:TARGET_COUNTRY:OfferId
            string idConstruct = "";

            foreach (var product in productsToDelete)
            {
                // No static later, when we have more data.
                idConstruct = "online:sv:SE:" + product["ProductID"].ToString();
                ProductsCustomBatchRequestEntry newEntry = new ProductsCustomBatchRequestEntry()
                {
                    BatchId = batchId,
                    MerchantId = _merchantID,
                    Method = "delete",
                    ProductId = idConstruct
                };
                batchRequest.Entries.Add(newEntry);
                batchId++;
            }


            try
            {
                ProductsResource.CustombatchRequest reeeq = _service.Products.Custombatch(batchRequest);
                reeeq.Execute();
            }
            catch (Exception e)
            {
                Debug.WriteLine("EXCEPTION THROWN @ProductDelete()");
                Debug.WriteLine("Message: " + e.Message);
                Debug.WriteLine("Stack Trace: " + e.StackTrace);
                Debug.WriteLine("Target Site: " + e.TargetSite);
            }

            return;
        }

        public void ProductReturn()
        {

        }

        public void ProductStatusesReturn()
        {

        }


    }
}


