﻿using Microsoft.EntityFrameworkCore;
using RealEstateAPI.Models;
using System.Reflection.Metadata.Ecma335;

namespace RealEstateAPI.Data
{
    public class ApiDbContext:DbContext 
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Property> Properties { get; set; } 


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=RealEstateDb;");
        }
    }
}
