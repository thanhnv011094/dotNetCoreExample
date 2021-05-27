using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace example.ViewModel.User
{
    public class ForgotPasswordRequest
    {
        [Required]
        public string Email { get; set; }
    }
}
