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
    class ContactService
    {
        public static async Task<List<ContactModel>> GetContacts()
        {
            try
            {
                List<ContactModel> contacts = new List<ContactModel>();

                string url = DataURL.CONTACTS;
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AuthenticationService.GetToken());
                    using (HttpResponseMessage message = await client.GetAsync(url))
                    {
                        if (message.StatusCode == HttpStatusCode.OK)
                        {
                            string rawResponse = await message.Content.ReadAsStringAsync();
                            contacts = JsonConvert.DeserializeObject<List<ContactModel>>(rawResponse);
                        }
                    }
                }
                return contacts;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static async Task<ContactModel> CreateContact(ContactModel model)
        {

            ContactModel retVal = null;

            try
            {
                string URL = DataURL.CONTACTS;

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
                            retVal = JsonConvert.DeserializeObject<ContactModel>(rawResponse);
                        }
                    
                    }
                }
            }

            catch (Exception ex)
            {

            }
            return retVal;
        }
    }
}
