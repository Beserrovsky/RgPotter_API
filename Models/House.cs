using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RG_Potter_API.Models
{
    public class House
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<User> Users { get; set; }
    }
}
