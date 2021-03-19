using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Cookit.WebApp.Data;
using Cookit.WebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Cookit.WebApp.Services;
using Cookit.WebApp.Models.PageModels;

namespace Cookit.WebApp.Pages.Recipes
{
    [AllowAnonymous]
    public class DetailsModel : RecipePageModel
    {

        public DetailsModel(
            CookitContext context,
            IAuthorizationService authorizationService,
            UserManager<IdentityUser> userManager,
            ImageFileService ifs) : base(context, authorizationService, userManager, ifs)
        {
        }

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

            Recipe.Instructions = await recipeInstructions.OrderBy(i => i.Step).ToListAsync();
            
            if (Recipe == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
