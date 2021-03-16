using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Cookit.WebApp.Data;
using Cookit.WebApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Cookit.WebApp.Services;
using Cookit.WebApp.Models.PageModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Cookit.WebApp.Pages.Recipes
{
    public class CreateModel : RecipePageModel
    {
        public CreateModel(
            CookitContext context,
            IAuthorizationService authorizationService,
            UserManager<CookitUser> userManager,
            ImageFileService ifs) : base(context, authorizationService, userManager, ifs)
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
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return NotFound(
                        $"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
                }

                Recipe.OwnerHandle = user.Handle;
                Recipe.OwnerEmail = User.Identity.Name;
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
                    Recipe.ImageFileName = imgFileName;
                }
            }


            // Title and Description are automatically bound using the asp-for tag helpers in the cshtml
            // Need to assign all of the navigation properties of the recipe
            Recipe.RecipeEquipment = ParseEquipmentInput(EquipmentName);
            Recipe.IngredientAmounts = ParseIngredientInput(IngredientName, IngredientAmount, IngredientUnit);
            Recipe.Instructions = ParseInstructionInput(InstructionStep, InstructionDescription);

            _context.Recipes.Add(Recipe);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

        /*
         * Take the raw input from the equipment text boxes and create a list of 
         * RecipeEquipment objects. We also create a new Equipment object if needed
         * 
         */
        private List<RecipeEquipment> ParseEquipmentInput(string[] EquipmentName)
        {
            // create a list of Recipe Equipment objects
            List<RecipeEquipment> recipeEquipment = new List<RecipeEquipment>();
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
                RecipeEquipment newRecipeEquipment = new RecipeEquipment { Equipment = eqEntry, Recipe = Recipe };
                recipeEquipment.Add(newRecipeEquipment);
            }

            return recipeEquipment;
        }

        private List<IngredientAmount> ParseIngredientInput(string[] IngredientName, int[] IngredientAmount, string[] IngredientUnit)
        {
            // now we need to do all the same stuff with ingredients
            List<IngredientAmount> ingredientAmounts = new List<IngredientAmount>();
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
                IngredientAmount newIngredientAmount = new IngredientAmount { Ingredient = ingEntry, Amount = ingAmount, Unit = ingUnit, Recipe = Recipe };
                ingredientAmounts.Add(newIngredientAmount);
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

                instructions.Add(new Instruction { Step = step, Description = desc, Recipe = Recipe });
            }

            return instructions;
        }
    }
}
