using example.ViewModel.User;
using System.Threading.Tasks;

namespace example.Admin.Services
{
    public interface IUserApiClient
    {
        Task<AuthenticateUserResponse> Authenticate(AuthenticateUserRequest request);
        Task<RegisterUserResponse> Register(RegisterUserRequest request);
    }
}
