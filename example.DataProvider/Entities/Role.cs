using Microsoft.AspNetCore.Identity;

namespace example.DataProvider.Entities
{
    public class Role : IdentityRole<int>
    {
        public string Description { get; set; }
    }
}
