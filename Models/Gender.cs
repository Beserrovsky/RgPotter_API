using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RG_Potter_API.Models
{
    public class Gender
    {
        [Key]
        public string Pronoum { get; set; }

        public IEnumerable<User> Users { get; set; }
    }
}
