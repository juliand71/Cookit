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
                new Recipe{Title="Test Recipe 1", Description="Test Description 1" },
                new Recipe{Title="Test Recipe 2", Description="Test Description 2" },
                new Recipe{Title="Test Recipe 3", Description="Test Description 3" },
                new Recipe{Title="Test Recipe 4", Description="Test Description 4" },
                new Recipe{Title="Test Recipe 5", Description="Test Description 5" },
                new Recipe{Title="Test Recipe 6", Description="Test Description 6" },
                new Recipe{Title="Test Recipe 7", Description="Test Description 7" },
                new Recipe{Title="Test Recipe 8", Description="Test Description 8" },
                new Recipe{Title="Test Recipe 9", Description="Test Description 9" },
                new Recipe{Title="Test Recipe 10", Description="Test Description 10" },
                new Recipe{Title="Test Recipe 11", Description="Test Description 11" },
                new Recipe{Title="Test Recipe 12", Description="Test Description 12" },
            };



            context.Recipes.AddRange(recipes);
            context.SaveChanges();

            var equipment = new Equipment[]
            {
                new Equipment{Name="Butter Knife"},
                new Equipment{Name="Skillet"},
                new Equipment{Name="TEST EQ"},
            };

            context.Equipment.AddRange(equipment);
            context.SaveChanges();

            var recipeEquipment = new RecipeEquipment[]
            {
                new RecipeEquipment{RecipeID=1, EquipmentID=2},
                new RecipeEquipment{RecipeID=2, EquipmentID=1},
                new RecipeEquipment{RecipeID=3, EquipmentID=3},
                new RecipeEquipment{RecipeID=4, EquipmentID=3},
                new RecipeEquipment{RecipeID=5, EquipmentID=3},
                new RecipeEquipment{RecipeID=6, EquipmentID=3},
                new RecipeEquipment{RecipeID=7, EquipmentID=3},
                new RecipeEquipment{RecipeID=8, EquipmentID=3},
                new RecipeEquipment{RecipeID=9, EquipmentID=3},
                new RecipeEquipment{RecipeID=10, EquipmentID=3},
                new RecipeEquipment{RecipeID=11, EquipmentID=3},
                new RecipeEquipment{RecipeID=12, EquipmentID=3},
                new RecipeEquipment{RecipeID=13, EquipmentID=3},
                new RecipeEquipment{RecipeID=14, EquipmentID=3},
            };

            context.RecipeEquipment.AddRange(recipeEquipment);
            context.SaveChanges();

            var ingredients = new Ingredient[]
            {
                new Ingredient{Name="Peanut Butter"},
                new Ingredient{Name="Jelly"},
                new Ingredient{Name="Bread"},
                new Ingredient{Name="Test Ingredient 1"},
                new Ingredient{Name="Test Ingredient 2"},
            };

            context.Ingredients.AddRange(ingredients);
            context.SaveChanges();

            var ingredientAmounts = new IngredientAmount[]
            {
                new IngredientAmount{IngredientID=3, RecipeID=1, Amount=2, Unit="Slices"},
                new IngredientAmount{IngredientID=4, RecipeID=1, Amount=1, Unit="Slice"},
                new IngredientAmount{IngredientID=3, RecipeID=2, Amount=2, Unit="Slices"},
                new IngredientAmount{IngredientID=1, RecipeID=2, Amount=2, Unit="TBSP"},
                new IngredientAmount{IngredientID=2, RecipeID=2, Amount=2, Unit="TBSP"},
                new IngredientAmount{IngredientID=4, RecipeID=3, Amount=1, Unit="TEST1"},
                new IngredientAmount{IngredientID=5, RecipeID=3, Amount=1, Unit="TEST2"},
                new IngredientAmount{IngredientID=4, RecipeID=4, Amount=1, Unit="TEST1"},
                new IngredientAmount{IngredientID=5, RecipeID=4, Amount=1, Unit="TEST2"},
                new IngredientAmount{IngredientID=4, RecipeID=5, Amount=1, Unit="TEST1"},
                new IngredientAmount{IngredientID=5, RecipeID=5, Amount=1, Unit="TEST2"},
                new IngredientAmount{IngredientID=4, RecipeID=6, Amount=1, Unit="TEST1"},
                new IngredientAmount{IngredientID=5, RecipeID=6, Amount=1, Unit="TEST2"},
                new IngredientAmount{IngredientID=4, RecipeID=7, Amount=1, Unit="TEST1"},
                new IngredientAmount{IngredientID=5, RecipeID=7, Amount=1, Unit="TEST2"},
                new IngredientAmount{IngredientID=4, RecipeID=8, Amount=1, Unit="TEST1"},
                new IngredientAmount{IngredientID=5, RecipeID=8, Amount=1, Unit="TEST2"},
                new IngredientAmount{IngredientID=4, RecipeID=9, Amount=1, Unit="TEST1"},
                new IngredientAmount{IngredientID=5, RecipeID=9, Amount=1, Unit="TEST2"},
                new IngredientAmount{IngredientID=4, RecipeID=10, Amount=1, Unit="TEST1"},
                new IngredientAmount{IngredientID=5, RecipeID=10, Amount=1, Unit="TEST2"},
                new IngredientAmount{IngredientID=4, RecipeID=11, Amount=1, Unit="TEST1"},
                new IngredientAmount{IngredientID=5, RecipeID=11, Amount=1, Unit="TEST2"},
                new IngredientAmount{IngredientID=4, RecipeID=12, Amount=1, Unit="TEST1"},
                new IngredientAmount{IngredientID=5, RecipeID=12, Amount=1, Unit="TEST2"},
                new IngredientAmount{IngredientID=4, RecipeID=13, Amount=1, Unit="TEST1"},
                new IngredientAmount{IngredientID=5, RecipeID=13, Amount=1, Unit="TEST2"},
                new IngredientAmount{IngredientID=4, RecipeID=14, Amount=1, Unit="TEST1"},
                new IngredientAmount{IngredientID=5, RecipeID=14, Amount=1, Unit="TEST2"},


            };

            context.IngredientAmounts.AddRange(ingredientAmounts);
            context.SaveChanges();

            var instructions = new Instruction[]
            {
                new Instruction{RecipeID=1, Step=1, Description="Place the slice of cheese in between the two slices of bread"},
                new Instruction{RecipeID=1, Step=2, Description="Cook It in a Skillet till it taste good"},
                new Instruction{RecipeID=2, Step=1, Description="Spread the peanut butter on one side of both pieces of bread"},
                new Instruction{RecipeID=2, Step=2, Description="Spread the jelly the same way"},
                new Instruction{RecipeID=2, Step=3, Description="Put the two pieces of bread together"},
                new Instruction{RecipeID=3, Step=1, Description="TEST STEP 1"},
                new Instruction{RecipeID=3, Step=2, Description="TEST STEP 2"},
                new Instruction{RecipeID=4, Step=1, Description="TEST STEP 1"},
                new Instruction{RecipeID=4, Step=2, Description="TEST STEP 2"},
                new Instruction{RecipeID=5, Step=1, Description="TEST STEP 1"},
                new Instruction{RecipeID=5, Step=2, Description="TEST STEP 2"},
                new Instruction{RecipeID=6, Step=1, Description="TEST STEP 1"},
                new Instruction{RecipeID=6, Step=2, Description="TEST STEP 2"},
                new Instruction{RecipeID=7, Step=1, Description="TEST STEP 1"},
                new Instruction{RecipeID=7, Step=2, Description="TEST STEP 2"},
                new Instruction{RecipeID=8, Step=1, Description="TEST STEP 1"},
                new Instruction{RecipeID=8, Step=2, Description="TEST STEP 2"},
                new Instruction{RecipeID=9, Step=1, Description="TEST STEP 1"},
                new Instruction{RecipeID=9, Step=2, Description="TEST STEP 2"},
                new Instruction{RecipeID=10, Step=1, Description="TEST STEP 1"},
                new Instruction{RecipeID=10, Step=2, Description="TEST STEP 2"},
                new Instruction{RecipeID=11, Step=1, Description="TEST STEP 1"},
                new Instruction{RecipeID=11, Step=2, Description="TEST STEP 2"},
                new Instruction{RecipeID=12, Step=1, Description="TEST STEP 1"},
                new Instruction{RecipeID=12, Step=2, Description="TEST STEP 2"},
                new Instruction{RecipeID=13, Step=1, Description="TEST STEP 1"},
                new Instruction{RecipeID=13, Step=2, Description="TEST STEP 2"},
                new Instruction{RecipeID=14, Step=1, Description="TEST STEP 1"},
                new Instruction{RecipeID=14, Step=2, Description="TEST STEP 2"},
            };

            context.Instructions.AddRange(instructions);
            context.SaveChanges();
        }

    }
}
