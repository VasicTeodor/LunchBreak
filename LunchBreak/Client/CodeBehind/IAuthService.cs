using System.Threading.Tasks;
using LunchBreak.Shared;

namespace LunchBreak.Client.CodeBehind
{
    public interface IAuthService
    {
        Task<LoginResult> Login(LoginData loginModel);
        Task Logout();
        Task<RegisterResult> Register(UserRegisterDTO registerModel);
        Task<string> GetUser();
        Task<string> GetUserId();
        Task<bool> IsUserAdmin();
        Task<bool> IsUserEditor();
        Task<bool> IsUser();
    }
}