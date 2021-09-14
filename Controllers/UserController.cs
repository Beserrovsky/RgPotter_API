using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RG_Potter_API.DB;
using RG_Potter_API.Models;
using RG_Potter_API.Models.DTOs;
using RG_Potter_API.Services;

namespace RG_Potter_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly PotterContext _context;
        private readonly IPasswordHash _hash;
        private readonly IConfiguration _configuration;

        public UserController(PotterContext context, IPasswordHash hash, IConfiguration configuration)
        {
            _context = context;
            _hash = hash;
            _configuration = configuration;
        }

#if DEBUG

        // GET: api/User/List
        [HttpGet("List")]
        public async Task<ActionResult<IEnumerable<User>>> List()
        {
            var users = _context.Users.Include(c => c.House).AsNoTracking();
            return await users.ToListAsync();
        }

#endif

        // POST: api/User/Login
        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<ActionResult<string>> Login(Credentials credentials)
        {
            if (!ModelState.IsValid) return BadRequest();
            
            User user = await CheckCredentials(credentials);
            if (user == null) return Unauthorized();

            return new TokenService(_configuration).GenerateToken(user);
        }

        // GET: api/User
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<User>> GetUser()
        {
            var email = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (email == null) return Unauthorized();

            User user = await _context.Users.FindAsync(email);
            if (user == null) return NotFound();

            return user;
        }

        // POST: api/User
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            if (await _context.Users.FindAsync(user.Email) != null) return Conflict();
            if (!_context.Houses.Any(h => h.Id == user.House_Id)) ModelState.AddModelError(nameof(user.House_Id), "House ID invalid!");
            if (!_context.Genders.Any(g => g.Pronoum == user.Pronoum)) ModelState.AddModelError(nameof(user.Pronoum), "Pronoum invalid!");

            if (!ModelState.IsValid) return ValidationProblem();

            user.Password = _hash.Of(user.Password);

            _context.Add(user);
            _context.SaveChanges();
            
            return Created(nameof(GetUser), user);
        }

        // PUT: api/User
        [Authorize]
        [HttpPatch]
        public async Task<ActionResult<User>> PatchUser(User user)
        {

            return Ok();
        }

        // DELETE: api/User
        [Authorize]
        [HttpDelete]
        public async Task<ActionResult<User>> DeleteUser(User user)
        {

            return Ok();
        }

        async private Task<User> CheckCredentials(Credentials credentials)
        {
            return await _context.Users.Include(c => c.House).FirstOrDefaultAsync(i => i.Email == credentials.Email && i.Password == _hash.Of(credentials.Password));
        }
    }
}
