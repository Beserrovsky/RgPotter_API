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
        private readonly IConfiguration _configuration;
        private readonly IPasswordHash _hash;

        public UserController(PotterContext context, IConfiguration configuration, IPasswordHash hash)
        {
            _context = context;
            _configuration = configuration;
            _hash = hash;
        }

#if DEBUG

        // GET: api/User
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            var users = _context.Users.Include(c => c.House).AsNoTracking();
            return await users.ToListAsync();
        }

#endif

        // GET: api/User/5
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(Guid id)
        {
            var user = _context.Users.Include(c => c.House).FirstOrDefaultAsync(i => i.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            return await user;
        }

        // PUT: api/User/5
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(Guid id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/User
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }


        // POST: api/User/Auth
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<string>> Auth(Credentials credentials)
        {
            User user = await Login(credentials);
            if(user == null) return Unauthorized();

            string token = GenJWT(user.Id);

            return token;
        }

        // DELETE: api/User/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult<User>> DeleteUser(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return user;
        }

        private bool UserExists(Guid id)
        {
            return _context.Users.Any(e => e.Id == id);
        }

        async private Task<User> Login(Credentials credentials)
        {
            return await _context.Users.Include(c => c.House).FirstOrDefaultAsync(i => i.Email == credentials.Email && i.Password == _hash.Of(credentials.Password));
        }

        private string GenJWT(Guid userID)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            var issuer = _configuration["Jwt:Issuer"];

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sid, userID.ToString())
            };

            var token = new JwtSecurityToken(
                issuer,
                issuer,
                claims,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
