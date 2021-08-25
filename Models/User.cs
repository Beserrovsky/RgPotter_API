using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RG_Potter_API.Models
{
    public class User
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string House_Id { get; set; }

        public House House { get; set; }
    }
}
