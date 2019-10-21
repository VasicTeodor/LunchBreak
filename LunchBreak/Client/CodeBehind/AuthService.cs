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

        public AuthService(HttpClient httpClient,
            ILocalStorageService localStorage, IHttpRequest httpRequest, IJSRuntime jsRuntime)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
            _httpRequest = httpRequest;
            _jsRuntime = jsRuntime;
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

        public async Task<LoginResult> Login(LoginData loginModel)
        {
            var response = await _httpClient.PostJsonAsync<LoginResult>("api/authorization/login", loginModel);
            if (response.Successful)
            {
                await _localStorage.SetItemAsync("authToken", response.Token);
                await _localStorage.SetItemAsync("lunchBreakId", response.Id);
                await _localStorage.SetItemAsync("lunchBreakUser", response.Username);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", response.Token);

                return response;
            }

            return response;
        }

        public async Task Logout()
        {
            await _localStorage.RemoveItemAsync("authToken");
            await _localStorage.RemoveItemAsync("lunchBreakId");
            await _localStorage.RemoveItemAsync("lunchBreakUser");
            _httpClient.DefaultRequestHeaders.Authorization = null;
            await _jsRuntime.InvokeVoidAsync("LogOut");
        }

        public async Task<RegisterResult> Register(UserRegisterDTO registerModel)
        {
            var response = await _httpRequest.HttpPost<RegisterResult>("api/authorization/register", registerModel);
            Console.WriteLine(registerModel);
            Console.WriteLine(response);
            return response;
        }
    }
}