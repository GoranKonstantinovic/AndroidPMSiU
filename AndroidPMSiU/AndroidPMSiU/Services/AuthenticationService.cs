using AndroidPMSiU.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace AndroidPMSiU.Services
{
    class AuthenticationService
    {
        public static async Task<bool> Login(AccountModel accountModel)
        {
            try
            {

                var dict = new Dictionary<string, string>();
                dict.Add("grant_type", "password");
                dict.Add("username", accountModel.Username);
                dict.Add("password", accountModel.Password);
                string url = DataURL.TOKEN_AUTHENTICATION;
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    using (HttpResponseMessage response = await client.PostAsync(url, new FormUrlEncodedContent(dict)))
                    {
                        var responseData = await response.Content.ReadAsStringAsync();
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            AccountResponseModel data = JsonConvert.DeserializeObject<AccountResponseModel>(responseData);
                            SaveToken(data.access_token);
                            InsertSyncTime();
                            return true;
                        }
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private static void SaveToken(string token)
        {
            App.Current.Properties["token"] = token;
        }

        public static string GetToken()
        {
            return App.Current.Properties.ContainsKey("token") ? App.Current.Properties["token"].ToString() : null;
        }

        public static void ClearToken()
        {
            App.Current.Properties.Remove("token");
        }

        public static void InsertSyncTime()
        {
            App.Current.Properties["synctime"] = DateTime.Now;
        }

        public static DateTime? GetSyncTime()
        {
            return App.Current.Properties.ContainsKey("synctime") ? (DateTime)App.Current.Properties["synctime"] : (DateTime?)null;
        }


    }
}
