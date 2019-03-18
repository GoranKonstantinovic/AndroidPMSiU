using AndroidPMSiU.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace AndroidPMSiU.Services
{
    class DataService
    {
        public static async Task<DataModel> GetData(DateTime? lastSync = null)
        {
            try
            {
                DataModel data = new DataModel();

                string url = DataURL.DATA;
                if (lastSync != null)
                {
                    url = url + "?lastSync=" + lastSync.Value.Ticks;
                }
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AuthenticationService.GetToken());
                    using (HttpResponseMessage message = await client.GetAsync(url))
                    {
                        if (message.StatusCode == HttpStatusCode.OK)
                        {
                            string rawResponse = await message.Content.ReadAsStringAsync();
                            data = JsonConvert.DeserializeObject<DataModel>(rawResponse);
                        }
                    }
                }
                return data;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
