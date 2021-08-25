using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RG_Potter_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RG_Potter_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<UserController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public User Get(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
