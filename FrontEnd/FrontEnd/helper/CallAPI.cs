using FrontEnd.Models;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace FrontEnd.Helpper
{
    public class CallAPI
    {
        private static HttpClient _httpClient = new HttpClient();


        public ServiceResult POSTData(object json, string url)
        {
            using (var content = new StringContent(JsonConvert.SerializeObject(json), System.Text.Encoding.UTF8, "application/json"))
            {
                HttpResponseMessage result = _httpClient.PostAsync(url, content).Result;
                string returnValue = result.Content.ReadAsStringAsync().Result;

                return JsonConvert.DeserializeObject<ServiceResult>(returnValue); ;
            }
        }

        public object POSTDataAsync(object json, string url)
        {
        
                using (var content = new StringContent(JsonConvert.SerializeObject(json), System.Text.Encoding.UTF8, "application/json"))
                {
                    HttpResponseMessage response = _httpClient.PostAsync(url, content).Result;

                    if (response.StatusCode == System.Net.HttpStatusCode.BadRequest || response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                        return response.Content.ReadAsStringAsync().Result;


                    return response.Content.ReadAsStringAsync().Result; ;
                }
        }



        public object GETDataAsync(string url)
        {


            using (var content = new StringContent(""))
            {
                HttpResponseMessage response = _httpClient.GetAsync(url).Result;
                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest || response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                    return response.Content.ReadAsStringAsync().Result;


                return response.Content.ReadAsStringAsync().Result; ;
            }

        }

    }


}