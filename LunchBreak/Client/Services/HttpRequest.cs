using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace LunchBreak.Client.Services
{
    public class HttpRequest : IHttpRequest
    {
        private readonly IJSRuntime _jsRuntime;
        private readonly IAlertify _alertify;

        public HttpRequest(IJSRuntime jsRuntime, IAlertify alertify)
        {
            _jsRuntime = jsRuntime;
            _alertify = alertify;
        }
        public async Task<T> HttpGet<T>(string url, object data = null)
        {
            try
            {
                if (data == null)
                {
                    data = new { noData = true };
                }
                return (await _jsRuntime.InvokeAsync<T>("HttpGet", url, data));
            }
            catch (System.Exception)
            {
                await _alertify.Error("There was error while getting data from server");
                throw;
            }
        }

        public async Task<T> HttpPost<T>(string url, object data)
        {
            try
            {
                return (await _jsRuntime.InvokeAsync<T>("HttpPost", url, data));
            }
            catch (System.Exception)
            {
                await _alertify.Error("There was error while posting data to server");
                throw;
            }
        }

        public async Task<T> HttpDelete<T>(string url, object data)
        {
            try
            {
                return (await _jsRuntime.InvokeAsync<T>("HttpDelete", url, data));
            }
            catch (System.Exception)
            {
                await _alertify.Error("There was error while deleting data from server");
                throw;
            }
        }

        public async Task<T> HttpPut<T>(string url, object data)
        {
            try
            {
                return (await _jsRuntime.InvokeAsync<T>("HttpPut", url, data));
            }
            catch (System.Exception)
            {
                await _alertify.Error("There was error while updating data from server");
                throw;
            }
        }
    }
}