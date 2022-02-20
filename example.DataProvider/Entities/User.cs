using Microsoft.AspNetCore.Identity;

namespace example.DataProvider.Entities
{
    public class User : IdentityUser<int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
