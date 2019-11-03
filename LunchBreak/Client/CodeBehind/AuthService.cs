using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using LunchBreak.Client.Extensions;
using LunchBreak.Client.Services;
using LunchBreak.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace LunchBreak.Client.CodeBehind
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;
        private readonly IHttpRequest _httpRequest;
        private readonly IJSRuntime _jsRuntime;
        private readonly IAlertify _alertify;

        public AuthService(HttpClient httpClient,
            ILocalStorageService localStorage, IHttpRequest httpRequest, IJSRuntime jsRuntime, IAlertify alertify)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
            _httpRequest = httpRequest;
            _jsRuntime = jsRuntime;
            _alertify = alertify;
        }

        public async Task<string> GetUser()
        {
            var result = await _localStorage.GetItemAsync<string>("lunchBreakUser");
            return result;
        }

        public async Task<string> GetUserId()
        {
            var result = await _localStorage.GetItemAsync<string>("lunchBreakId");
            return result;
        }

        public async Task<bool> IsUser()
        {
            var result = await _jsRuntime.InvokeAsync<bool>("CheckIsUser");
            return result;
        }

        public async Task<bool> IsUserAdmin()
        {
            var result = await _jsRuntime.InvokeAsync<bool>("CheckIsUserAdmin");
            return result;
        }

        public async Task<bool> IsUserEditor()
        {
            var result = await _jsRuntime.InvokeAsync<bool>("CheckIsUserEditor");
            return result;
        }

        public async Task<bool> IsUserApporved()
        {
            var result = await _jsRuntime.InvokeAsync<bool>("UserApproved");
            return result;
        }

        public async Task<LoginResult> Login(LoginData loginModel)
        {
            try
            {
                var response = await _httpClient.PostJsonAsync<LoginResult>("api/authorization/login", loginModel);
                if (response.Successful)
                {
                    await _localStorage.SetItemAsync("authToken", response.Token);
                    await _localStorage.SetItemAsync("lunchBreakId", response.Id);
                    await _localStorage.SetItemAsync("lunchBreakApprovedUser", response.ApprovedAccount);
                    await _localStorage.SetItemAsync("lunchBreakUser", response.Username);
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", response.Token);

                    return response;
                }
                return response;
            }
            catch(Exception)
            {
                await _alertify.Error("Wrong username or password");
                throw;
            }
        }

        public async Task Logout()
        {
            await _localStorage.RemoveItemAsync("authToken");
            await _localStorage.RemoveItemAsync("lunchBreakId");
            await _localStorage.RemoveItemAsync("lunchBreakUser");
            await _localStorage.RemoveItemAsync("lunchBreakApprovedUser");
            _httpClient.DefaultRequestHeaders.Authorization = null;
            await _jsRuntime.InvokeVoidAsync("LogOut");
        }

        public async Task<RegisterResult> Register(UserRegisterDTO registerModel)
        {
            try
            {
                var response = await _httpRequest.HttpPost<RegisterResult>("api/authorization/register", registerModel);
                return response;
            }
            catch(Exception)
            {
                await _alertify.Error("There was error while registering");
                throw;
            }
        }
    }
}