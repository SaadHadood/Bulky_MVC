﻿using Bulky.Models;
using Microsoft.EntityFrameworkCore;

namespace Bulky.DataAccess.Context;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        
    }

    public DbSet<CategoryModel> Categories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CategoryModel>().HasData(
            
                new CategoryModel { Id = 1, Name = "Action", DisplayOrder = 1 },
                new CategoryModel { Id = 2, Name = "SciFi", DisplayOrder = 2 },
                new CategoryModel { Id = 3, Name = "History", DisplayOrder = 3 }

        );
    }
}
