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

            // if Recipes already Exist
            if (context.Recipes.Any())
            {
                return; // DB has already been seeded
            }

            var recipes = new Recipe[]
            {
                new Recipe{Title="Grilled Cheese", Description="The basic sandwich best prepared with Tomato Soup", ImageFileName="grilled-cheese.jpg", ImageCaption = "Damn that's a tasty lookin' grilled cheese" },
            };



            context.Recipes.AddRange(recipes);
            context.SaveChanges();

            var equipment = new Equipment[]
            {
                new Equipment{Name="Skillet"},
            };

            context.Equipment.AddRange(equipment);
            context.SaveChanges();

            var recipeEquipment = new RecipeEquipment[]
            {
                new RecipeEquipment{RecipeID=1, EquipmentID=1},
            };

            context.RecipeEquipment.AddRange(recipeEquipment);
            context.SaveChanges();

            var ingredients = new Ingredient[]
            {
                new Ingredient{Name="Bread"},
                new Ingredient{Name="Cheese"},
            };

            context.Ingredients.AddRange(ingredients);
            context.SaveChanges();

            var ingredientAmounts = new IngredientAmount[]
            {
                new IngredientAmount{IngredientID=1, RecipeID=1, Amount=2, Unit="Slices"},
                new IngredientAmount{IngredientID=2, RecipeID=1, Amount=1, Unit="Slice"},
            };

            context.IngredientAmounts.AddRange(ingredientAmounts);
            context.SaveChanges();

            var instructions = new Instruction[]
            {
                new Instruction{RecipeID=1, Step=1, Description="Place the slice of cheese in between the two slices of bread"},
                new Instruction{RecipeID=1, Step=2, Description="Cook It in a Skillet till it taste good"},
            };

            context.Instructions.AddRange(instructions);
            context.SaveChanges();
        }

    }
}
