using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Cookit.Data;
using Cookit.Models;
using Cookit.Models.PageModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Cookit.Services;
using Microsoft.AspNetCore.Http;

namespace Cookit.Pages.Recipes
{
    public class EditModel : RecipePageModel
    {

        public EditModel(
            CookitContext context,
            IAuthorizationService authorizationService,
            UserManager<CookitUser> userManager,
            ImageHandler ifs) : base(context, authorizationService, userManager, ifs)
        {

        }

        [BindProperty]
        public Recipe Recipe { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // can get equipment and ingredients in any order, so we grab them first
            Recipe = await _context.Recipes
                .Include(r => r.EquipmentRequirements).ThenInclude(re => re.Equipment)
                .Include(r => r.RecipeIngredients).ThenInclude(ia => ia.Ingredient)
                .Include(r => r.Author)
                .AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);

            // need to make sure instructions are properly sorted
            IQueryable<Instruction> recipeInstructions = from i in _context.Instructions select i;
            recipeInstructions = recipeInstructions.Where(i => i.RecipeId == id);
            recipeInstructions.OrderByDescending(i => i.Step);

            Recipe.Instructions = await recipeInstructions.AsNoTracking().ToListAsync();

            if (Recipe == null)
            {
                return NotFound();
            }

            // make sure current user is authorized
            var authorizeResult = await _authorizationService.AuthorizeAsync(User, Recipe, "CUDPolicy");

            if (authorizeResult.Succeeded)
            {
                return Page();
            }
            else if (User.Identity.IsAuthenticated)
            {
                return new ForbidResult();
            }
            else
            {
                return new ChallengeResult();
            }

        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(int id,
            IFormFile ImageFile,
            string[] EquipmentName,
            string[] IngredientName,
            int[] IngredientAmount,
            string[] IngredientUnit,
            int[] InstructionStep,
            string[] InstructionDescription)
        {


            // can get equipment and ingredients in any order, so we grab them first
            var recipeToUpdate = await _context.Recipes
                .Include(r => r.EquipmentRequirements).ThenInclude(re => re.Equipment)
                .Include(r => r.RecipeIngredients).ThenInclude(ia => ia.Ingredient)
                .Include(r => r.Author)
                .AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);

            // get the instructions in sorted order
            IQueryable<Instruction> recipeInstructions = from i in _context.Instructions select i;
            recipeInstructions = recipeInstructions.Where(i => i.RecipeId == id);
            recipeInstructions.OrderByDescending(i => i.Step);

            recipeToUpdate.Instructions = await recipeInstructions.AsNoTracking().ToListAsync();

            // if we didn't find a recipe
            if (Recipe == null)
            {
                return NotFound();
            }

            // make sure user trying to edit is the owner of the recipe
            var authorizeResult = await _authorizationService.AuthorizeAsync(User, recipeToUpdate, "CUDPolicy");

            if (authorizeResult.Succeeded)
            {
                // add the recipe to be updated
                _context.Recipes.Update(recipeToUpdate);
                // update title and description
                recipeToUpdate.Title = Recipe.Title;
                recipeToUpdate.Description = Recipe.Description;

                // check if the user added a new image
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

                UpdateEquipment(EquipmentName, recipeToUpdate);
                UpdateIngredients(IngredientName, IngredientAmount, IngredientUnit, recipeToUpdate);
                UpdateInstructions(InstructionStep, InstructionDescription, recipeToUpdate);

                await _context.SaveChangesAsync();

                return RedirectToPage("./Index");
            }
            // if the user is logged in, but not the owner of the recipe, forbid them from editing
            else if (User.Identity.IsAuthenticated)
            {
                return new ForbidResult();
            }
            else
            {
                // otherwise we prompt them to sign in
                return new ChallengeResult();
            }

        }

        /*
         * 
         * 
         */
        private void UpdateEquipment(string[] EquipmentName, Recipe recipeToUpdate)
        {
            // loop over our current list of equipment, and remove any that are not present in the new one
            for (int i = recipeToUpdate.EquipmentRequirements.Count() - 1; i > -1; i--)
            {
                var recEquip = recipeToUpdate.EquipmentRequirements[i];
                var equipmentName = recEquip.Equipment.Name;
                if (!EquipmentName.Contains(equipmentName))
                {
                    recipeToUpdate.EquipmentRequirements.RemoveAt(i);
                }
            }

            // loop over the array of new Equipment Names
            foreach (var eqName in EquipmentName)
            {
                // check if there is already an Equipment with that name
                var eqEntry = _context.Equipment.FirstOrDefault(e => e.Name == eqName);
                if (eqEntry == null)
                {
                    eqEntry = new Equipment { Name = eqName };
                }

                // check if there is already an existing EquipmentRequirement entry tying the Equipment to this Recipe
                var equipmentReqiurement = _context.EquipmentRequirements.FirstOrDefault(er => er.EquipmentId == eqEntry.Id && er.RecipeId == recipeToUpdate.Id);
                // if the above exists, then we don't do anything, but if it does not we need to add it
                if (equipmentReqiurement == null)
                {
                    equipmentReqiurement = new EquipmentRequirement { Equipment = eqEntry, Recipe = recipeToUpdate };
                    recipeToUpdate.EquipmentRequirements.Add(equipmentReqiurement);
                }

            }
        }

        private void UpdateIngredients(string[] IngredientName, int[] IngredientAmount, string[] IngredientUnit, Recipe recipeToUpdate)
        {
            // first loop backwards to determine if any existing ingredients were deleted
            // loop over our current list of equipment, and remove any that are not present in the new one
            for (int i = recipeToUpdate.RecipeIngredients.Count() - 1; i > -1; i--)
            {
                var iaEntry = recipeToUpdate.RecipeIngredients[i];
                var ingredientName = iaEntry.Ingredient.Name;
                if (!IngredientName.Contains(ingredientName))
                {
                    recipeToUpdate.RecipeIngredients.RemoveAt(i);
                }
            }

            //loop over our current array of ingredient amounts
            foreach (var ingAmount in recipeToUpdate.RecipeIngredients)
            {
                // handle updating existing ingredient amounts / units
                // i.e. if the recipe still contains butter, but they need to increase from 1 tbsp to 2 tbsp

                // search for an entry that matches the current name
                for (var i = 0; i < IngredientName.Length; i++)
                {
                    if (ingAmount.Ingredient.Name == IngredientName[i])
                    {
                        // if we find a match, we want to update the corresponding Amount and Unit Values
                        ingAmount.Amount = IngredientAmount[i];
                        ingAmount.Unit = IngredientUnit[i];
                    }
                }
            }

            // now we have to handle adding any completely new ingredients
            for (var i = 0; i < IngredientName.Length; i++)
            {
                string ingName = IngredientName[i];
                int ingAmount = IngredientAmount[i];
                string ingUnit = IngredientUnit[i];

                // check if there is an ingredient with that name in the database
                var ingEntry = _context.Ingredients.FirstOrDefault(i => i.Name == ingName);
                if (ingEntry == null)
                {
                    // create a new ingredient entry
                    ingEntry = new Ingredient { Name = ingName };
                }

                // same logic as the equipment function
                var recipeIngredient = _context.RecipeIngredients.FirstOrDefault(ri => ri.RecipeId == recipeToUpdate.Id && ri.IngredientId == ingEntry.Id);
                if (recipeIngredient == null)
                {
                    recipeIngredient = new RecipeIngredient { Recipe = recipeToUpdate, Ingredient = ingEntry, Amount = ingAmount, Unit = ingUnit };
                    recipeToUpdate.RecipeIngredients.Add(recipeIngredient);
                }
            }
        }

        private void UpdateInstructions(int[] InstructionStep, string[] InstructionDescription, Recipe recipeToUpdate)
        {
            // for instructions we can just loop over the list of current instructions and update the description field


            int i;
            for (i = 0; i < InstructionStep.Length; i++)
            {
                int step = InstructionStep[i];
                string desc = InstructionDescription[i];

                // if there is a new step being added
                if (i > recipeToUpdate.Instructions.Count())
                {
                    recipeToUpdate.Instructions.Add(new Instruction { Recipe = recipeToUpdate, Step = step, Text = desc });
                }
                // or if we are editing an existing step
                else
                {
                    recipeToUpdate.Instructions[i].Step = step;
                    recipeToUpdate.Instructions[i].Text = desc;
                }
            }
            // the above loop goes through the incructions that were listed in the post request
            // but it's possible a user removed a step
            // we don't have to worry about which step was removed, we just need to save the instructions that the user gave us
            // but if there are less instructions than before, we need to make sure our instruction list is cleaned up
            // i.e we need to trim the end off it

            // check for any instructions that need to be removed
            // should only occur if there are now less instructions than there were previously.
            if (i < recipeToUpdate.Instructions.Count())
            {
                recipeToUpdate.Instructions.RemoveRange(i, recipeToUpdate.Instructions.Count() - i);
            }

        }
    }
}
