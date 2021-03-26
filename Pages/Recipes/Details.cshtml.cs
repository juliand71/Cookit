using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Cookit.Data;
using Cookit.Models;
using Microsoft.AspNetCore.Authorization;
using Cookit.Models.PageModels;
using Microsoft.AspNetCore.Identity;
using Cookit.Services;

namespace Cookit.Pages.Recipes
{
    [AllowAnonymous]
    public class DetailsModel : RecipePageModel
    {

        public DetailsModel(
            CookitContext context,
            IAuthorizationService authorizationService,
            UserManager<CookitUser> userManager,
            ImageHandler ifs) : base(context, authorizationService, userManager, ifs)
        {
        }

        public Recipe Recipe { get; set; }
        public Rating NewRating { get; set; }
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
                .Include(r => r.Ratings)
                .AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);

            IQueryable<Instruction> recipeInstructions = from i in _context.Instructions select i;
            recipeInstructions = recipeInstructions.Where(i => i.RecipeId == id);

            Recipe.Instructions = await recipeInstructions.OrderBy(i => i.Step).ToListAsync();

            foreach (var rating in Recipe.Ratings)
            {
                if (rating.UserId == _userManager.GetUserId(User))
                {
                    // user has already rated this recipe
                    NewRating = rating;
                }
            }
            if (NewRating == null)
            {
                NewRating = new Rating { Recipe = Recipe, User = await _userManager.GetUserAsync(User), Score = 1 };
            }
            if (Recipe == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
