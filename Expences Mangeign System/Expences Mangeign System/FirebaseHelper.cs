using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Expences_Mangeign_System
{
    public static class FirebaseHelper
    {
        private static readonly string DatabaseUrl = "https://expense-tracking-system-6dd2e-default-rtdb.firebaseio.com/";

        // POST data to Firebase
        public static async Task<string> PostDataAsync(string endpoint, string jsonData)
        {
            using (HttpClient client = new HttpClient())
            {
                string url = $"{DatabaseUrl}{endpoint}.json";

                HttpContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                try
                {
                    HttpResponseMessage response = await client.PostAsync(url, content);
                    if (response.IsSuccessStatusCode)
                    {
                        return await response.Content.ReadAsStringAsync();
                    }
                    else
                    {
                        throw new Exception($"Error: {response.StatusCode}, Details: {await response.Content.ReadAsStringAsync()}");
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Exception occurred: {ex.Message}");
                }
            }
        }

        // GET data from Firebase
        public static async Task<string> GetDataAsync(string endpoint)
        {
            using (HttpClient client = new HttpClient())
            {
                string url = $"{DatabaseUrl}{endpoint}.json";

                try
                {
                    HttpResponseMessage response = await client.GetAsync(url);
                    if (response.IsSuccessStatusCode)
                    {
                        return await response.Content.ReadAsStringAsync();
                    }
                    else
                    {
                        throw new Exception($"Error: {response.StatusCode}, Details: {await response.Content.ReadAsStringAsync()}");
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Exception occurred: {ex.Message}");
                }
            }
        }
    }
}
