using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Cookit.Models;

namespace Cookit.Data
{
    public class CookitContext : IdentityDbContext<CookitUser>
    {
        public CookitContext(DbContextOptions<CookitContext> options)
            : base(options)
        {
        }

        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Instruction> Instructions { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<RecipeIngredient> RecipeIngredients { get; set; }
        public DbSet<Equipment> Equipment { get; set; }
        public DbSet<EquipmentRequirement> EquipmentRequirements { get; set; }
        public DbSet<CookitUser> CookitUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);

            builder.Entity<Recipe>().ToTable("Recipe");
            builder.Entity<Instruction>().ToTable("Instruction");
            builder.Entity<Ingredient>().ToTable("Ingredient");
            builder.Entity<RecipeIngredient>().ToTable("RecipeIngredient");
            builder.Entity<Equipment>().ToTable("Equipment");
            builder.Entity<EquipmentRequirement>().ToTable("EquipmentRequirement");
        }

        
    }
}
