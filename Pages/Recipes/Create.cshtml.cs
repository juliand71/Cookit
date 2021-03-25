using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Cookit.Data;
using Cookit.Models;
using Cookit.Models.PageModels;
using Cookit.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace Cookit.Pages.Recipes
{
    public class CreateModel : RecipePageModel
    {
        public CreateModel(
            CookitContext context,
            IAuthorizationService authorizationService,
            UserManager<CookitUser> userManager,
            ImageHandler ifs) : base(context, authorizationService, userManager, ifs)
        {

        }

        public IActionResult OnGet()
        {
            if (User.Identity.IsAuthenticated)
            {
                return Page();
            }
            else
            {
                return new ChallengeResult();
            }
        }

        [BindProperty]
        public Recipe Recipe { get; set; }
        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD


        public async Task<IActionResult> OnPostAsync(IFormFile ImageFile,
            string[] EquipmentName,
            string[] IngredientName,
            int[] IngredientAmount,
            string[] IngredientUnit,
            int[] InstructionStep,
            string[] InstructionDescription
            )
        {
            if (User.Identity.IsAuthenticated)
            {
                var RecipeOwner = await _userManager.GetUserAsync(User);
                Recipe.Author = RecipeOwner;
            }
            else
            {
                return new ChallengeResult();
            }

            if (ImageFile != null)
            {
                if (_ifs.IsValidFileType(ImageFile.FileName))
                {
                    string imgFileName = _ifs.GetNewFileName(ImageFile.FileName);
                    _ifs.SaveImageFile(ImageFile, imgFileName);
                    //_ifs.OptimizeImageFile(imgFileName);
                    Recipe.ImageFile = imgFileName;
                }
            }


            // Title and Description are automatically bound using the asp-for tag helpers in the cshtml
            // Need to assign all of the navigation properties of the recipe
            Recipe.EquipmentRequirements = ParseEquipmentInput(EquipmentName);
            Recipe.RecipeIngredients = ParseIngredientInput(IngredientName, IngredientAmount, IngredientUnit);
            Recipe.Instructions = ParseInstructionInput(InstructionStep, InstructionDescription);

            _context.Recipes.Add(Recipe);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

        /*
         * Take the raw input from the equipment text boxes and create a list of 
         * EquipmentRequirement objects. We also create a new Equipment object if needed
         * 
         */
        private List<EquipmentRequirement> ParseEquipmentInput(string[] EquipmentName)
        {
            // create a list of Recipe Equipment objects
            List<EquipmentRequirement> recipeEquipment = new List<EquipmentRequirement>();
            // loop over the list of names
            foreach (var eqstring in EquipmentName)
            {
                // check if already exists in the database
                Equipment eqEntry = _context.Equipment.FirstOrDefault(e => e.Name == eqstring);
                // if not then create a new Equipment Object
                if (eqEntry == null)
                {
                    eqEntry = new Equipment { Name = eqstring };
                    // not necessary to save this to DB. Entity Framework does the magic for us!
                    // will be automatically propogated when we save our recipe
                }
                EquipmentRequirement newEquipmentRequirement = new EquipmentRequirement { Equipment = eqEntry, Recipe = Recipe };
                recipeEquipment.Add(newEquipmentRequirement);
            }

            return recipeEquipment;
        }

        private List<RecipeIngredient> ParseIngredientInput(string[] IngredientName, int[] IngredientAmount, string[] IngredientUnit)
        {
            // now we need to do all the same stuff with ingredients
            List<RecipeIngredient> ingredientAmounts = new List<RecipeIngredient>();
            // loop using an index this time since there are multiple inputs
            for (int i = 0; i < IngredientName.Length; i++)
            {
                string ingName = IngredientName[i];
                int ingAmount = IngredientAmount[i];
                string ingUnit = IngredientUnit[i];

                // check if already exists in the database
                Ingredient ingEntry = _context.Ingredients.FirstOrDefault(i => i.Name == ingName);
                // if not then create a new object
                if (ingEntry == null)
                {
                    ingEntry = new Ingredient { Name = ingName };
                    // not necessary to save this to DB. Entity Framework does the magic for us!
                }
                RecipeIngredient newRecipeIngredient = new RecipeIngredient { Ingredient = ingEntry, Amount = ingAmount, Unit = ingUnit, Recipe = Recipe };
                ingredientAmounts.Add(newRecipeIngredient);
            }
            return ingredientAmounts;
        }

        private List<Instruction> ParseInstructionInput(int[] InstructionStep, string[] InstructionDescription)
        {
            // finally we need to get the instructions list sorted through
            List<Instruction> instructions = new List<Instruction>();

            for (int i = 0; i < InstructionStep.Length; i++)
            {
                int step = InstructionStep[i];
                string desc = InstructionDescription[i];

                // instructions won't potentially exist in the database before hand
                // relation ship is one recipe to many instructions, and instructions do not have multiple recipes
                // no need to search DB for instruction

                instructions.Add(new Instruction { Step = step, Text = desc, Recipe = Recipe });
            }

            return instructions;
        }
    }
}