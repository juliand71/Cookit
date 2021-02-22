using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Cookit.WebApp.Data;
using Cookit.WebApp.Models;

namespace Cookit.WebApp.Pages.Recipes
{
    public class EditModel : PageModel
    {
        private readonly Cookit.WebApp.Data.CookitContext _context;

        public EditModel(Cookit.WebApp.Data.CookitContext context)
        {
            _context = context;
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
                .Include(r => r.RecipeEquipment).ThenInclude(re => re.Equipment)
                .Include(r => r.IngredientAmounts).ThenInclude(ia => ia.Ingredient)
                .AsNoTracking().FirstOrDefaultAsync(m => m.RecipeID == id);

            IQueryable<Instruction> recipeInstructions = from i in _context.Instructions select i;
            recipeInstructions = recipeInstructions.Where(i => i.RecipeID == id);
            recipeInstructions.OrderByDescending(i => i.Step);

            Recipe.Instructions = await recipeInstructions.AsNoTracking().ToListAsync();

            if (Recipe == null)
            {
                return NotFound();
            }
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(int id,
            string[] EquipmentName,
            string[] IngredientName,
            int[] IngredientAmount,
            string[] IngredientUnit,
            int[] InstructionStep,
            string[] InstructionDescription)
        {
            // can get equipment and ingredients in any order, so we grab them first
            var recipeToUpdate = await _context.Recipes
                .Include(r => r.RecipeEquipment).ThenInclude(re => re.Equipment)
                .Include(r => r.IngredientAmounts).ThenInclude(ia => ia.Ingredient)
                .AsNoTracking().FirstOrDefaultAsync(m => m.RecipeID == id);

            IQueryable<Instruction> recipeInstructions = from i in _context.Instructions select i;
            recipeInstructions = recipeInstructions.Where(i => i.RecipeID == id);
            recipeInstructions.OrderByDescending(i => i.Step);

            recipeToUpdate.Instructions = await recipeInstructions.AsNoTracking().ToListAsync();

            if (Recipe == null)
            {
                return NotFound();
            }
            _context.Recipes.Update(recipeToUpdate);
            recipeToUpdate.Title = Recipe.Title;
            recipeToUpdate.Description = Recipe.Description;
            UpdateEquipment(EquipmentName, recipeToUpdate);
            UpdateIngredients(IngredientName, IngredientAmount, IngredientUnit, recipeToUpdate);
            UpdateInstructions(InstructionStep, InstructionDescription, recipeToUpdate);

            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

        /*
         * 
         * 
         */
        private void UpdateEquipment(string[] EquipmentName, Recipe recipeToUpdate)
        {
            // loop over our current list of equipment, and remove any that are not present in the new one
            for (int i = recipeToUpdate.RecipeEquipment.Count() - 1; i > -1; i--)
            {
                var recEquip = recipeToUpdate.RecipeEquipment[i];
                var equipmentName = recEquip.Equipment.Name;
                if (!EquipmentName.Contains(equipmentName))
                {
                    recipeToUpdate.RecipeEquipment.RemoveAt(i);
                }
            }

            // loop over the array of new Equipment Names
            foreach(var eqName in EquipmentName)
            {
                // check if there is already an Equipment with that name
                var eqEntry = _context.Equipment.FirstOrDefault(e => e.Name == eqName);
                if (eqEntry == null)
                {
                    eqEntry = new Equipment { Name = eqName };
                }

                // check if there is already an existing RecipeEquipment entry tying the Equipment to this Recipe
                var recEquipEntry = _context.RecipeEquipment.FirstOrDefault(re => re.EquipmentID == eqEntry.EquipmentID && re.RecipeID == recipeToUpdate.RecipeID);
                // if the above exists, then we don't do anything, but if it does not we need to add it
                if (recEquipEntry == null)
                {
                    recEquipEntry = new RecipeEquipment { Equipment = eqEntry, Recipe = recipeToUpdate };
                    recipeToUpdate.RecipeEquipment.Add(recEquipEntry);
                }

            }
        }

        private void UpdateIngredients(string[] IngredientName, int[] IngredientAmount, string[] IngredientUnit, Recipe recipeToUpdate)
        {
            // first loop backwards to determine if any existing ingredients were deleted
            // loop over our current list of equipment, and remove any that are not present in the new one
            for (int i = recipeToUpdate.IngredientAmounts.Count() - 1; i > -1; i--)
            {
                var iaEntry = recipeToUpdate.IngredientAmounts[i];
                var ingredientName = iaEntry.Ingredient.Name;
                if (!IngredientName.Contains(ingredientName))
                {
                    recipeToUpdate.IngredientAmounts.RemoveAt(i);
                }
            }

            //loop over our current array of ingredient amounts
            foreach (var ingAmount in recipeToUpdate.IngredientAmounts)
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
                var ingAmountEntry = _context.IngredientAmounts.FirstOrDefault(ia => ia.RecipeID == recipeToUpdate.RecipeID && ia.IngredientID == ingEntry.IngredientID);
                if (ingAmountEntry == null)
                {
                    ingAmountEntry = new IngredientAmount { Recipe = recipeToUpdate, Ingredient = ingEntry, Amount = ingAmount, Unit = ingUnit };
                    recipeToUpdate.IngredientAmounts.Add(ingAmountEntry);
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

                if (i > recipeToUpdate.Instructions.Count())
                {
                    recipeToUpdate.Instructions.Add(new Instruction { Recipe = recipeToUpdate, Step = step, Description = desc });
                }
                else
                {
                    recipeToUpdate.Instructions[i].Step = step;
                    recipeToUpdate.Instructions[i].Description = desc;
                }
            }
            // check for any instructions that need to be removed
            // should only occur if there are now less instructions than there were previously.
            if (i < recipeToUpdate.Instructions.Count())
            {
                recipeToUpdate.Instructions.RemoveRange(i, recipeToUpdate.Instructions.Count() - i);
            }

        }
    }
}
