using System.ComponentModel.DataAnnotations;

namespace example.ViewModel.Role
{
    public class CreateRoleRequest
    {
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }
    }
}
