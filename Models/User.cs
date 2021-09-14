using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RG_Potter_API.Models
{
    public class User
    {
        [Key]
        [Required]
        public string Email { get; set; }

        [Required]
        [StringLength(75, MinimumLength = 5)]
        public string Name { get; set; }

        [Required]
        public string House_Id { get; set; } = "none";

        public House House { get; set; }

        [Required]
        public string Pronoum { get; set; }

        public Gender Gender { get; set; }

        [Required]
        public string Password { get; set; }

        public int LumusSuccesses { get; set; } = 0;

        public int LumusFails { get; set; } = 0;
    }
}
