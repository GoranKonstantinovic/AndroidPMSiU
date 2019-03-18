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
    class AccountService
    {
        public static async Task<List<AccountModel>> GetAccounts()
        {
            try
            {
                List<AccountModel> accounts = new List<AccountModel>();

                string url = DataURL.DATA;
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AuthenticationService.GetToken());
                    using (HttpResponseMessage message = await client.GetAsync(url))
                    {
                        if (message.StatusCode == HttpStatusCode.OK)
                        {
                            string rawResponse = await message.Content.ReadAsStringAsync();
                            accounts = JsonConvert.DeserializeObject<List<AccountModel>>(rawResponse);
                        }
                    }
                }
                return accounts;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
