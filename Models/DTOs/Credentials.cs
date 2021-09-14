using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RG_Potter_API.Models.DTOs
{
    public class Credentials
    {
        private string email;
        [Required]
        public string Email { get => email; set { email = value.ToLower(); } }

        [Required]
        public string Password { get; set; }
    }
}
