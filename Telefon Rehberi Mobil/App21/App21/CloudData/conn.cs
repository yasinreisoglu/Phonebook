using App21.Model;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using RestSharp;
using System.Net.Http.Headers;

namespace App21.CloudData
{
    class Conn
    {
        private string url;

        private HttpClient client;
        private List<User> users;
        public Conn()
        {
            client = new HttpClient();
            users  = new List<User>();
            url = "http://10.0.2.2:65292/api/values";
        }
        public List<User> userAll()
        {

            var token = new RestClient("http://10.0.2.2:65292/api/token/token?user=deneme&password=110");
            var requesttoken = new RestRequest(Method.POST);
            IRestResponse respo = token.Execute(requesttoken);
            string tokenkey = respo.Content.Replace("\"", "");
            List<User> userlist = new List<User>();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenkey);
            var task = client.GetAsync(url).ContinueWith((taskResponse) =>
            {
                
                var response = taskResponse.Result;
                var jsonString = response.Content.ReadAsStringAsync();
                jsonString.Wait();
                userlist = JsonConvert.DeserializeObject<List<User>>(jsonString.Result);
            });
            task.Wait();
            return userlist;
        }


    
    }
}