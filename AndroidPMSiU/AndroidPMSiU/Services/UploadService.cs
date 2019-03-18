using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace AndroidPMSiU.Services
{
    class UploadService
    {
        public static async Task<string> UploadImage(byte[] fileBytes, string fileName)
        {

            try
            {
                string URL = "http://gogikole-001-site1.gtempurl.com/api/Upload";

                using (var client = new HttpClient())
                {
                    HttpClient httpClient = new HttpClient();
                    MultipartFormDataContent form = new MultipartFormDataContent();

                    form.Add(new ByteArrayContent(fileBytes, 0, fileBytes.Length), "profileImg", fileName);
                    using (var result = await client.PostAsync(URL, form))
                    {
                            string url = await result.Content.ReadAsStringAsync();
                        if (result.StatusCode == HttpStatusCode.OK)
                        {
                            return url;
                        }
                    }
                }
            }

            catch (Exception ex)
            {
            }

            return null;
        }
    }
}
