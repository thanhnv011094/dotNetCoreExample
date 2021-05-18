using example.ViewModel.User;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace example.Admin.Services
{
    public class UserApiClient : ApiClientBase, IUserApiClient
    {
        public UserApiClient(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
            : base(httpClientFactory, httpContextAccessor, configuration)
        {
        }

        public async Task<AuthenticateUserResponse> Authenticate(AuthenticateUserRequest request)
        {
            string url = "api/user/authenticate";
            return await PostAsync<AuthenticateUserResponse>(url, request);
        }

        public Task<RegisterUserResponse> Register(RegisterUserRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
