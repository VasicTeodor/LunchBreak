using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LunchBreak.Client.Services
{
    public class Alertify : IAlertify
    {
        private readonly IJSRuntime _jsRuntime;

        public Alertify(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }
        public async Task Confirm(string message)
        {
            await _jsRuntime.InvokeVoidAsync("AlertConfirm", message);
        }

        public async Task Error(string message)
        {
            await _jsRuntime.InvokeVoidAsync("AlertError", message);
        }

        public async Task Success(string message)
        {
            await _jsRuntime.InvokeVoidAsync("AlertSuccess", message);
        }
    }
}
