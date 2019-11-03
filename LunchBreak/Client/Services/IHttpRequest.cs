using System.Threading.Tasks;

namespace LunchBreak.Client.Services
{
    public interface IHttpRequest
    {
        Task<T> HttpGet<T>(string url, object data = null);
        Task<T> HttpPost<T>(string url, object data);
        Task<T> HttpDelete<T>(string url, object data);
        Task<T> HttpPut<T>(string url, object data);
    }
}