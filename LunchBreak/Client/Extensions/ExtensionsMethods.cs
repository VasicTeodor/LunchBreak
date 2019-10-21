using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace LunchBreak.Client.Extensions
{
    public static class ExtensionsMethods
    {
        public static async Task<T> MySendJsonAsync<T>(this HttpClient httpClient, string requestUri, object content)
        {
            var requestJson = JsonSerializer.Serialize(content);
            var response = await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Post, requestUri)
            {
                Content = new StringContent(requestJson, Encoding.UTF8, "application/json")
            });

            var stringContent = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(stringContent);
        }

    }
}