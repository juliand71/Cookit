using Cookit.WebApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cookit.WebApp.Data
{
    public class CookitContext : IdentityDbContext
    {
        public CookitContext(DbContextOptions<CookitContext> options) : base(options)
        {
            // empty as base class constructor should handle everything needed
        }

        // definte the DbSets
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Instruction> Instructions { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<IngredientAmount> IngredientAmounts { get; set; }
        public DbSet<Equipment> Equipment { get; set; }
        public DbSet<RecipeEquipment> RecipeEquipment { get; set; }
        public DbSet<CookitUser> CookitUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Recipe>().ToTable("Recipe");
            modelBuilder.Entity<Instruction>().ToTable("Instruction");
            modelBuilder.Entity<Ingredient>().ToTable("Ingredient");
            modelBuilder.Entity<IngredientAmount>().ToTable("IngredientAmount");
            modelBuilder.Entity<Equipment>().ToTable("Equipment");
            modelBuilder.Entity<RecipeEquipment>().ToTable("RecipeEquipment");
            modelBuilder.Entity<CookitUser>().ToTable("CookitUser");
        }
    }
}
