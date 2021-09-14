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
        public string Email { get; set; }

        public string Name { get; set; }

        public string House_Id { get; set; }

        public House House { get; set; }

        public string Pronoum { get; set; }

        public Gender Gender { get; set; }

        public string Password { get; set; }

        public int LumusSuccesses { get; set; }

        public int LumusFails { get; set; }
    }
}
