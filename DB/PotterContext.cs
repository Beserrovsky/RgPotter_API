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

        public DbSet<House> Houses { get; set; }

        public DbSet<Gender> Genders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .ToTable("User");

            modelBuilder.Entity<House>() // House 1:N User
                .ToTable("House")
                .HasMany(house => house.Users)
                .WithOne(user => user.House)
                .HasForeignKey(user => user.House_Id)
                .IsRequired();

            modelBuilder.Entity<Gender>() // Gender N:1 User
                .ToTable("Gender")
                .HasMany(house => house.Users)
                .WithOne(gender => gender.Gender)
                .HasForeignKey(user => user.Pronoum)
                .IsRequired();
        }
    }
}
