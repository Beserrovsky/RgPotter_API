using Microsoft.EntityFrameworkCore;
using RG_Potter_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RG_Potter_API.DB
{
    public class PotterContext : DbContext
    {
        public PotterContext(DbContextOptions<PotterContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("User");
        }
    }
}
