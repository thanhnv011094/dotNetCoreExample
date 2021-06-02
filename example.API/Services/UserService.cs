using example.API.Helpers;
using example.DataProvider;
using example.DataProvider.Entities;
using example.ViewModel.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace example.API.Services
{
    public class UserService : IUserService
    {
        private readonly AppSettings _appSettings;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signinManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IOptions<AppSettings> appSettings,
            UserManager<User> userManager,
            SignInManager<User> signinManager,
            RoleManager<Role> roleManager,
            IUnitOfWork unitOfWork)
        {
            _appSettings = appSettings.Value;
            _userManager = userManager;
            _signinManager = signinManager;
            _roleManager = roleManager;
            _unitOfWork = unitOfWork;
        }

        public async Task<AuthenticateUserResponse> Authenticate(AuthenticateUserRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);

            //var x = await _unitOfWork.UserReponsitory.GetAllAsync();

            // return null if user not found
            if (user == null) return null;

            var result = await _signinManager.PasswordSignInAsync(user, request.Password, request.RememberMe, true);
            if (!result.Succeeded)
            {
                return null;
            }

            await _signinManager.SignOutAsync();

            // authentication successful so generate jwt token
            var token = await GenerateJwtToken(user);

            return new AuthenticateUserResponse
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                Token = token
            };
        }

        public async Task<User> GetByUserName(string userName)
        {
            return await _userManager.FindByNameAsync(userName);
        }

        public async Task<RegisterUserResponse> Register(RegisterUserRequest request)
        {
            var user = new User()
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserName = request.UserName
            };
            
            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                return null;
            }

            return new RegisterUserResponse()
            {
                FirstName = user.FirstName,
                LastName = user.LastName
            };
        }

        // helper methods

        private async Task<string> GenerateJwtToken(User user)
        {
            // generate token that is valid for 3 mins
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var roles = await _userManager.GetRolesAsync(user);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                    new[] {
                        new Claim(ClaimTypes.Name, user.UserName.ToString()),
                        new Claim(ClaimTypes.GivenName, user.FirstName.ToString()),
                        new Claim(ClaimTypes.Role,string.Join(",", roles))
                    }
                ),
                Expires = DateTime.UtcNow.AddMinutes(3),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
