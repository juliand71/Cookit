using Cookit.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cookit.Data
{
    public class DbInitializer
    {
        public static async Task InitializeDbAsync(CookitContext context, UserManager<CookitUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            // run any pending migrations
            context.Database.Migrate();
            Console.Write(context.Database.GetMigrations());
            CookitUser sampleAuthour;
            // check if any users exist
            if (!userManager.Users.Any())
            {
                // make sure we have admin and user roles created
                if (!await roleManager.RoleExistsAsync("Admin"))
                {
                    await roleManager.CreateAsync(new IdentityRole("Admin"));
                }
                if (!await roleManager.RoleExistsAsync("User"))
                {
                    await roleManager.CreateAsync(new IdentityRole("User"));
                }
                // create the admin user and a test user
                var adminUser = await CreateAdminUser(userManager, roleManager);
                sampleAuthour = await CreateTestUser(userManager, roleManager);
            }
            else
            {
                sampleAuthour = await userManager.FindByNameAsync("julian");
            }

            if (!context.Recipes.Any())
            {
                CreateSeedRecipes(context, sampleAuthour);
            }
        }

        private static async Task<CookitUser> CreateAdminUser(UserManager<CookitUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            var cookitAdmin = new CookitUser { UserName = "cookitadmin", Email = "admin@letscookitapp.com", EmailConfirmed = true };
            // using a dummy password, change this as soon as possible in production!!
            var result = await userManager.CreateAsync(cookitAdmin, "ChangeMe123!");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(cookitAdmin, "Admin");
            }

            return cookitAdmin;
        }

        private static async Task<CookitUser> CreateTestUser(UserManager<CookitUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            var cookitUser = new CookitUser { UserName = "julian", Email = "juliandixonweb@gmail.com", EmailConfirmed = true };
            // using a dummy password, change this as soon as possible in production!!
            var result = await userManager.CreateAsync(cookitUser, "ChangeMe123!");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(cookitUser, "User");
            }

            return cookitUser;
        }

        private static void CreateSeedRecipes(CookitContext context, CookitUser author)
        {
            // creating the hungarian goulash recipe here: https://www.yummly.com/recipe/Hungarian-Goulash-2514110?prm-v1
            var goulashRecipe = new Recipe
            {
                Title = "Hungarian Goulash",
                Description = "Only for when you are really really Hungary. This is a delicious and cheap recipe. It's mostly just a fancy beef stew. But you can make yourself sound really fancy when you tell your friends and family that you are making Hungarian Goulash!",
                ImageFile = "hgoulash.jpg",
                DatePosted = DateTime.Now.Date,
            };

            goulashRecipe.Author = author;

            var equipment = new Equipment[]
            {
                new Equipment { Name = "Pot" },
                new Equipment { Name = "Mixing Bowl" },
            };

            var equipReqs = new List<EquipmentRequirement>();
            foreach (var eqEntry in equipment)
            {
                equipReqs.Add(new EquipmentRequirement { Recipe = goulashRecipe, Equipment = eqEntry });
            }

            goulashRecipe.EquipmentRequirements = equipReqs;

            var ingredients = new Ingredient[]
            {
                new Ingredient { Name = "Onion" },
                new Ingredient { Name = "Butter" },
                new Ingredient { Name = "Caraway Seeds" },
                new Ingredient { Name = "Paprika" },
                new Ingredient { Name = "Flour" },
                new Ingredient { Name = "Stewing Beef - Cubed" },
                new Ingredient { Name = "Beef Broth" },
                new Ingredient { Name = "Tomatoes - Diced" },
                new Ingredient { Name = "Potatoes" },
                new Ingredient { Name = "Carrots" },
                new Ingredient { Name = "Salt" },
                new Ingredient { Name = "Pepper" },
            };

            var recipeIngredients = new List<RecipeIngredient>();
            recipeIngredients.Add(new RecipeIngredient { Recipe = goulashRecipe, Ingredient = ingredients[0],  Amount = 2, Unit = "Medium" });
            recipeIngredients.Add(new RecipeIngredient { Recipe = goulashRecipe, Ingredient = ingredients[1], Amount = 2, Unit = "Teaspoons" });
            recipeIngredients.Add(new RecipeIngredient { Recipe = goulashRecipe, Ingredient = ingredients[2], Amount = 1, Unit = "Teaspoon" });
            recipeIngredients.Add(new RecipeIngredient { Recipe = goulashRecipe, Ingredient = ingredients[3], Amount = 2, Unit = "Tablespoon" });
            recipeIngredients.Add(new RecipeIngredient { Recipe = goulashRecipe, Ingredient = ingredients[4], Amount = 0.25, Unit = "Cup" });
            recipeIngredients.Add(new RecipeIngredient { Recipe = goulashRecipe, Ingredient = ingredients[5], Amount = 1.5, Unit = "Pounds" });
            recipeIngredients.Add(new RecipeIngredient { Recipe = goulashRecipe, Ingredient = ingredients[6], Amount = 2, Unit = "Cups" });
            recipeIngredients.Add(new RecipeIngredient { Recipe = goulashRecipe, Ingredient = ingredients[7], Amount = 1, Unit = "Cup" });
            recipeIngredients.Add(new RecipeIngredient { Recipe = goulashRecipe, Ingredient = ingredients[8], Amount = 3, Unit = "Cups" });
            recipeIngredients.Add(new RecipeIngredient { Recipe = goulashRecipe, Ingredient = ingredients[9], Amount = 1.5, Unit = "Cups" });
            recipeIngredients.Add(new RecipeIngredient { Recipe = goulashRecipe, Ingredient = ingredients[10], Amount = 1, Unit = "Teaspoon" });
            recipeIngredients.Add(new RecipeIngredient { Recipe = goulashRecipe, Ingredient = ingredients[11], Amount = 0.25, Unit = "Teaspoon" });

            goulashRecipe.RecipeIngredients = recipeIngredients;

            var instructions = new Instruction[]
            {
                new Instruction { Recipe = goulashRecipe, Step = 1, Text = "In a large pot, melt butter and add onion. Cook till translucent. Stir in caraway seeds and paprika and mix well." },
                new Instruction { Recipe = goulashRecipe, Step = 2, Text = "In a bowl, dredge the stew beef with flour. Add beef to the onion mixture and cook for about 2-3 minutes. " },
                new Instruction { Recipe = goulashRecipe, Step = 3, Text = "Slowly add about ¼ cup of the beef broth to lift the brown bits off the bottom of the pan. Then add remaining broth, diced tomatoes (potatoes and carrots if using), salt and pepper." },
                new Instruction { Recipe = goulashRecipe, Step = 4, Text = "Stir and bring to a boil, cover, then reduce to a simmer for about 1 ½ -2 hours or until tender." },
            };

            var recInstructions = new List<Instruction>(instructions);
            goulashRecipe.Instructions = recInstructions;

            var ratings = new List<Rating>();
            ratings.Add(new Rating { Recipe = goulashRecipe, User = author, Score = 5 });
            goulashRecipe.Ratings = ratings;

            context.Recipes.Add(goulashRecipe);
            context.SaveChanges();
        }
    }
}
