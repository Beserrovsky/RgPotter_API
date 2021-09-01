using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RG_Potter_API.Services
{
    public interface IPasswordHash // THX Renan!!
    {
        public string Of(string password);
    }

    public class Sha256PasswordHash : IPasswordHash
    {
        public string Of(string password)
        {
            using var sha = SHA256.Create();

            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));

            return string.Join("", bytes.Select(b => b.ToString("x2")));
        }
    }
}
