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

namespace ParttrapDev.Models
{
    public class GoogleApi
    {
        private UserCredential _credential;
        private ShoppingContentService _service;
        private string _clientId = "896777409399-ghva93bgs7qpv293tqj1vp4eefi7n82c.apps.googleusercontent.com";
        private string _clientSecret = "Or7cg3mMtWmMsxIhBjecHcRq";


        public GoogleApi()
        {
            _credential = Authenticate();
            _service = CreateService(_credential);
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

        public JArray ProductGet(string url)
        {
            WebClient client = new WebClient();
            JArray jsonObj = new JArray();
            try
            {
                string json = client.DownloadString(url);
                jsonObj = (JArray)JsonConvert.DeserializeObject(json);
            }
            catch (Exception e)
            {
                Console.WriteLine("Message: " + e.Message + "\nStacktrace" + e.StackTrace + "\nTarget: " + e.TargetSite);
            }
            finally
            {
                client.Dispose();
            }

            return jsonObj;
        }




        public void ProductInsert(string productUrl)
        {
            JArray productsToPush = ProductGet(productUrl);
            //MORE CODE, OBV
        }


        public void ProductUpdate()
        {

        }

        public void ProductDelete()
        {

        }

        public void ProductReturn()
        {

        }

        public void ProductStatusesReturn()
        {

        }

    }
}


