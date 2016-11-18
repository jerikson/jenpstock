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
        private string _clientId;
        private string _clientSecret;


        public ShoppingContentService CreateService(UserCredential credential)
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




        public void ProductInsert()
        {

        }


        public void ProductUpdate()
        {

        }

        public void ProductDelete()
        {

        }

    }
}


