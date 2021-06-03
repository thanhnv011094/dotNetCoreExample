using example.DataProvider.Entities;
using example.ViewModel.User;
using System.Threading.Tasks;

namespace example.API.Services
{
    public interface IUserService
    {
        Task<AuthenticateUserResponse> Authenticate(AuthenticateUserRequest request);
        Task<RegisterUserResponse> Register(RegisterUserRequest request);
        Task<User> GetByUserName(string userName);
        Task<SetRoleUserResponse> SetRoleUser(SetRoleUserRequest request);
    }
}
