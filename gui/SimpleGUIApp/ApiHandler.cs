using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimpleGUIApp
{
    public class ApiHandler
    {
        private Form form;

        public ApiHandler(Form form)
        {
            this.form = form;
        }

        // Håndterer GET-forespørsel
        public async Task<string> SendGetRequest(string url)
        {
            using HttpClient client = new HttpClient();
            try
            {
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException e)
            {
                return $"Request error: {e.Message}";
            }
        }

        // Håndterer POST-forespørsel
        public async Task<string> SendPostRequest(string url, string jsonData)
        {
            using HttpClient client = new HttpClient();
            try
            {
                HttpContent content = new StringContent(jsonData, System.Text.Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(url, content);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException e)
            {
                return $"Request error: {e.Message}";
            }
        }
    }
}
