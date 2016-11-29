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

        private JArray ProductGet(string productUrl)
        {
            //WebClient version
            JArray jsonObj = new JArray();
            using (WebClient client = new WebClient())
            {
                try
                {
                    //Kolla ifall ProductTextField2 finns om man använder JsonConvert.DeserializeObject.
                    string json = client.DownloadString(productUrl);
                    //jsonObj = (JArray)JsonConvert.DeserializeObject(json);
                    jsonObj = JArray.Parse(json);
                }
                catch (Exception e)
                {
                    Debug.WriteLine("Message: " + e.Message + "\nStacktrace: " + e.StackTrace + "\nTarget: " + e.TargetSite);
                }
            }


            // WebRequest ska tydligen vara bättre för stora överföringar? Ska tydligen inte blocka interface 
            // thread, men MVC har inte interface thread? Det har väl bara WPF o sånt?
            //WebRequest version
            //////WebRequest request = WebRequest.Create(productUrl);
            //////Stream dataStream = request.GetResponse().GetResponseStream();
            //////StreamReader reader = new StreamReader(dataStream);
            //////string response = reader.ReadToEnd();
            //////JArray jsonObj = JArray.Parse(response);


            //HttpWebRequest version
            //////HttpWebRequest request = (HttpWebRequest)WebRequest.Create(productUrl);
            //////request.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip,deflate");
            //////request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            //////request.ContentType = "application/json; charset=utf-8";
            //////request.Method = WebRequestMethods.Http.Get;
            //////request.Accept = "application/json";

            //////HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            //////Stream dataStream = response.GetResponseStream();
            //////StreamReader reader = new StreamReader(dataStream);
            //////string json = reader.ReadToEnd();
            //////JArray jsonObj = JArray.Parse(json);


            return jsonObj;
        }


        ///<summary>
        ///Inserts and/or updates products to a Google Shopping account using custombatch method to send multiple requests as one.
        ///<para>productUrl is a URI to something that returns a JSON object.</para>
        /// </summary>
        public void ProductInsert(string productUrl, bool update)
        {
            JArray productsToPush = ProductGet(productUrl);
            ProductsCustomBatchRequest batchRequest = new ProductsCustomBatchRequest();
            batchRequest.Entries = new List<ProductsCustomBatchRequestEntry>();

            //Update boolean kommer bara existera i testing, inte i slutgiltliga version.
            //if (update == false)
            //{
            //Insert product
            //    foreach (var product in productsToPush)
            //    {
            //        Product newProduct = new Product()
            //        {
            //            OfferId = product["ProductID"].ToString(),
            //            Title = "Product Title",
            //            Description = "Product description",
            //            Link = "https://www.example.com/products/Product?productId=1",
            //            ImageLink = "https://www.example.com/productImages/ProductImage?productId=1",
            //            ContentLanguage = "sv",
            //            TargetCountry = "SE",
            //            Channel = "online",
            //            Availability = "out of stock",
            //            Condition = "new",
            //            GoogleProductCategory = "3219",
            //            IdentifierExists = false,
            //            Price = new Price()
            //            {
            //                Currency = "SEK",
            //                Value = "100"
            //            }
            //        };

            //        ProductsCustomBatchRequestEntry newEntry = new ProductsCustomBatchRequestEntry()
            //        {
            //            Method = "insert",
            //            BatchId = long.Parse(newProduct.OfferId),
            //            MerchantId = _merchantID,
            //            Product = newProduct
            //        };

            //        batchRequest.Entries.Add(newEntry);

            //    }
            //}
            //else
            //{
            //    //Update product
            //    foreach (var product in productsToPush)
            //    {
            //        Product newProduct = new Product()
            //        {
            //            OfferId = product["ProductID"].ToString(),
            //            Title = "Updated product title!",
            //            Description = "Updated description!",
            //            Link = "https://www.example.com/products/Product?productId=1",
            //            ImageLink = "https://www.example.com/productImages/ProductImage?productId=1",
            //            ContentLanguage = "sv",
            //            TargetCountry = "SE",
            //            Channel = "online",
            //            Availability = "out of stock",
            //            Condition = "new",
            //            GoogleProductCategory = "3219",
            //            IdentifierExists = false,
            //            Price = new Price()
            //            {
            //                Currency = "SEK",
            //                Value = "200"
            //            }
            //        };

            //        ProductsCustomBatchRequestEntry newEntry = new ProductsCustomBatchRequestEntry()
            //        {
            //            Method = "insert",
            //            BatchId = long.Parse(newProduct.OfferId),
            //            MerchantId = _merchantID,
            //            Product = newProduct,
            //        };

            //        batchRequest.Entries.Add(newEntry);

            //    }
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
                //Debug.WriteLine("WOAH SETTLE DOWN, HOMEBOY: " + product);
                Debug.WriteLine("Type: " + product.Type);
                Debug.WriteLine("Product Values: " + product.Values());
                
                //GÖRA EFTER LUNCH:
                //Greja mer med denna fakking JSON skiten...

                Debug.WriteLine("OfferId: " + product["ProductID"].ToString());
                Debug.WriteLine("Description: " + product["Description"].ToString());
                Debug.WriteLine("Price, incl VAT: " + product["NetPriceInclVAT"]["Amount"].ToString());
                Debug.WriteLine("Currency: " + product["NetPriceInclVAT"]["Currency"]["Code"].ToString());
                Debug.WriteLine("Product Page Link: " + product["AdditionalValues"]["DetailLink"].ToString());
                Debug.WriteLine("Image Link: " + product["AdditionalValues"]["ImageUrl"].ToString());
                //Crashar på dessa jag har lagt in:
                if (product["AdditionalValues"]["ProductTextField1"] != null)
                    Debug.WriteLine("CHECK DIS 1: " + product["AdditionalValues"]["ProductTextField1"].ToString());
                if (product["AdditionalValues"]["ProductTextField2"] != null)
                    Debug.WriteLine("CHECK DIS 2: " + product["AdditionalValues"]["ProductTextField2"].ToString());
                if (product["AdditionalValues"]["Status"] != null)
                    Debug.WriteLine("IS DIS WORK?: " + product["AdditionalValues"]["Status"].ToString());
                if (product["ProductFieldsTexts"] != null)
                    Debug.WriteLine("PLEASE WORK: " + product["ProductFieldsTexts"].ToString());

                //Debug.WriteLine(product[].ToString());
                //Debug.WriteLine(product[].ToString());
                //Debug.WriteLine(product[].ToString());
                //Debug.WriteLine(product[].ToString());
                //break;
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


        public void ProductUpdate(string productUrl)
        {
            JArray productsToUpdate = ProductGet(productUrl);
            ProductsCustomBatchRequest batchRequest = new ProductsCustomBatchRequest();
            batchRequest.Entries = new List<ProductsCustomBatchRequestEntry>();

            //foreach (var product in productsToUpdate)
            //{
            //    Product newProduct = new Product()
            //    {
            //        OfferId = product["ProductID"].ToString(),
            //        Title = "Updated product title!",
            //        Description = "Updated description!",
            //        Link = "https://www.example.com/products/Product?productId=1",
            //        ImageLink = "https://www.example.com/productImages/ProductImage?productId=1",
            //        ContentLanguage = "sv",
            //        TargetCountry = "SE",
            //        Channel = "online",
            //        Availability = "out of stock",
            //        Condition = "new",
            //        GoogleProductCategory = "3219",
            //        IdentifierExists = false,
            //        Price = new Price()
            //        {
            //            Currency = "SEK",
            //            Value = "200"
            //        }
            //    };

            //    ProductsCustomBatchRequestEntry newEntry = new ProductsCustomBatchRequestEntry()
            //    {
            //        Method = "insert",
            //        BatchId = long.Parse(newProduct.OfferId),
            //        MerchantId = _merchantID,
            //        Product = newProduct,
            //    };

            //    batchRequest.Entries.Add(newEntry);

            //}

            //try
            //{
            //    ProductsResource.CustombatchRequest reeeq = _service.Products.Custombatch(batchRequest);
            //    reeeq.Execute();
            //}
            //catch (Exception e)
            //{
            //    System.Diagnostics.Debug.WriteLine("EXCEPTION THROWN @ProductInsert()");
            //    System.Diagnostics.Debug.WriteLine("Message: " + e.Message);
            //    System.Diagnostics.Debug.WriteLine("Stack Trace: " + e.StackTrace);
            //    System.Diagnostics.Debug.WriteLine("Target Site: " + e.TargetSite);
            //}

            ProductInsert(productUrl, true);

            return;
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
                // idConstruct final: online = Channel, sv = ContentLanguage, SE = TargetCountry
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

        /// <summary>
        /// Returns a page of the merchant's Google Shopping products
        /// <para>Max 250 per page</para>
        /// </summary>
        /// <returns>Returns a List< Google.Apis.ShoppingContent.v2.Data.Product ></returns>
        public List<Product> ProductReturn()
        {
            string pageToken = null;
            const long maxResults = 250;

            ProductsListResponse productsResponse = null;
            do
            {
                ProductsResource.ListRequest productsRequest = _service.Products.List(_merchantID);
                productsRequest.MaxResults = maxResults;
                productsRequest.PageToken = pageToken;
                productsRequest.IncludeInvalidInsertedItems = true;
                productsResponse = productsRequest.Execute();

                if (productsResponse.NextPageToken != null)
                {
                    pageToken = productsResponse.NextPageToken;
                }
            }
            while (productsResponse.NextPageToken != null);

            return productsResponse.Resources.ToList();
        }

        /// <summary>
        /// Returns a page of the merchant's Google Shopping productstatuses
        /// <para>A productstatus can includ errors and such</para>
        /// <para>Max 250 per page</para>
        /// </summary>
        /// <returns>Returns a List< Google.Apis.ShoppingContent.v2.Data.ProductStatus ></returns>
        public List<ProductStatus> ProductStatusesReturn()
        {
            string pageToken = null;
            const long maxResults = 250;

            ProductstatusesListResponse productStatusesResponse = null;
            do
            {
                ProductstatusesResource.ListRequest productStatusesRequest = _service.Productstatuses.List(_merchantID);
                productStatusesRequest.MaxResults = maxResults;
                productStatusesRequest.PageToken = pageToken;
                productStatusesRequest.IncludeInvalidInsertedItems = true;
                productStatusesResponse = productStatusesRequest.Execute();

                if (productStatusesResponse.NextPageToken != null)
                {
                    pageToken = productStatusesResponse.NextPageToken;
                }
            }
            while (productStatusesResponse.NextPageToken != null);

            return productStatusesResponse.Resources.ToList();
        }


    }
}


