using Cookit.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cookit.WebApp.Data
{
    public static class DbInitializer
    {
        public static void Initialize(CookitContext context)
        {
            context.Database.EnsureCreated();

            // if Recipes already Exist
            if (context.Recipes.Any())
            {
                return; // DB has already been seeded
            }

            var recipes = new Recipe[]
            {
                new Recipe{Title="Grilled Cheese", Description="The basic sandwich best prepared with Tomato Soup" },
                new Recipe{Title="PB&J", Description="The timeless snack" },
            };



            context.Recipes.AddRange(recipes);
            context.SaveChanges();

            var equipment = new Equipment[]
            {
                new Equipment{Name="Butter Knife"},
                new Equipment{Name="Skillet"},
            };

            context.Equipment.AddRange(equipment);
            context.SaveChanges();

            var recipeEquipment = new RecipeEquipment[]
            {
                new RecipeEquipment{RecipeID=1, EquipmentID=1},
                new RecipeEquipment{RecipeID=2, EquipmentID=2},
            };

            context.RecipeEquipment.AddRange(recipeEquipment);
            context.SaveChanges();

            var ingredients = new Ingredient[]
            {
                new Ingredient{Name="Peanut Butter"},
                new Ingredient{Name="Jelly"},
                new Ingredient{Name="Bread"},
                new Ingredient{Name="Cheese"},
            };

            context.Ingredients.AddRange(ingredients);
            context.SaveChanges();

            var ingredientAmounts = new IngredientAmount[]
            {
                new IngredientAmount{IngredientID=3, RecipeID=1, Amount=2, Unit="Slices"},
                new IngredientAmount{IngredientID=4, RecipeID=1, Amount=1, Unit="Slice"},
                new IngredientAmount{IngredientID=3, RecipeID=2, Amount=2, Unit="Slices"},
                new IngredientAmount{IngredientID=1, RecipeID=2, Amount=4, Unit="TBSP"},
                new IngredientAmount{IngredientID=2, RecipeID=2, Amount=2, Unit="TBSP"},
            };

            context.IngredientAmounts.AddRange(ingredientAmounts);
            context.SaveChanges();

            var instructions = new Instruction[]
            {
                new Instruction{RecipeID=1, Step=2, Description="Place the slice of cheese in between the two slices of bread"},
                new Instruction{RecipeID=1, Step=2, Description="Cook It in a Skillet till it taste good"},
                new Instruction{RecipeID=2, Step=1, Description="Spread the peanut butter on one side of both pieces of bread"},
                new Instruction{RecipeID=2, Step=2, Description="Spread the jelly the same way"},
                new Instruction{RecipeID=2, Step=3, Description="Put the two pieces of bread together"},
            };

            context.Instructions.AddRange(instructions);
            context.SaveChanges();
        }

    }
}
