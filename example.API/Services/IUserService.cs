using example.API.Models;
using example.DataProvider.Entities;
using System.Threading.Tasks;

namespace example.API.Services
{
    public interface IUserService
    {
        Task<AuthenticateUserResponse> Authenticate(AuthenticateUserRequest request);
        Task<RegisterUserResponse> Register(RegisterUserRequest request);
        Task<User> GetByUserName(string userName);
    }
}
