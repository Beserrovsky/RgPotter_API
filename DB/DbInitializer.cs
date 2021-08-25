using Microsoft.Extensions.Configuration;
using RG_Potter_API.DB;
using RG_Potter_API.Models;
using System;

namespace RG_Potter_API
{
    internal class DbInitializer
    {
        private readonly bool _reset;

        public DbInitializer(IConfiguration configuration)
        {
            _reset = bool.Parse(configuration["ResetDB"] ?? "true");
        }

        internal void Initialize(PotterContext context)
        {
            if (_reset) context.Database.EnsureDeleted();

            if (context.Database.EnsureCreated()) return;

            var users = new[]
            {
                new User
                {

                }
            };

            foreach (var user in users) context.Users.Add(user);

            context.SaveChanges();
        }
    }
}