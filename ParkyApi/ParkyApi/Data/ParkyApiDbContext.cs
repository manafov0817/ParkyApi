using Microsoft.EntityFrameworkCore;
using ParkyApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyApi.Data
{
    public class ParkyApiDbContext : DbContext
    {
        public ParkyApiDbContext() { }
        public ParkyApiDbContext(DbContextOptions options) : base(options) { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=WINDOWS-EHNNMHA\\SQLEXPRESS; Initial Catalog = ParkyApiDbContext; Integrated Security = SSPI");
        } 
        public DbSet<NationalPark> NationalParks { get; set; }
        public DbSet<Trail> Trails { get; set; }
        public DbSet<User> Users { get; set; }

    }
}
