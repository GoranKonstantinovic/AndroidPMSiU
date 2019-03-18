using AndroidPMSiU.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace AndroidPMSiU.Services
{
    class MessageService
    {
        public static async Task<List<MessageModel>> GetMessages()
        {
            try
            {
                List<MessageModel> messages = new List<MessageModel>();

                string url = DataURL.MESSAGES;
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    using (HttpResponseMessage message = await client.GetAsync(url))
                    {
                        if (message.StatusCode == HttpStatusCode.OK)
                        {
                            string rawResponse = await message.Content.ReadAsStringAsync();
                            messages = JsonConvert.DeserializeObject<List<MessageModel>>(rawResponse);
                        }
                    }
                }
                return messages;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public static async Task<bool> CreateSentMessage(SendMessageModel model)
        {
            
            try
            {
                string URL = DataURL.MESSAGES;

                string stringContent = Newtonsoft.Json.JsonConvert.SerializeObject(model);

                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AuthenticationService.GetToken());

                    using (var result = await client.PostAsync(URL, new StringContent(stringContent, Encoding.UTF8, "application/json")))
                    {
                        if (result.StatusCode == HttpStatusCode.OK)
                        {
                            string rawResponse = await result.Content.ReadAsStringAsync();
                            return true;
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                
            }
            return false;
        }

        public static async void IsReaded(long id)
        {
            try
            {
                List<MessageModel> messages = new List<MessageModel>();

                string url = DataURL.MESSAGES + "/" + id;
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AuthenticationService.GetToken());
                    using (HttpResponseMessage message = await client.PutAsync(url, null))
                    {
                        string rawResponse = await message.Content.ReadAsStringAsync();

                    }
                }
            }
            catch (Exception)
            {

            }
        }
    }
}
