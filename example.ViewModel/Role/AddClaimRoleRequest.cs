using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace example.ViewModel.Role
{
    public class AddClaimRoleRequest
    {
        [Required]
        public string RoleName { get; set; }

        [Required]
        public string ClaimType { get; set; }

        [Required]
        public string ClaimValue { get; set; }

    }
}
