using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Cookit.WebApp.Data;
using Cookit.WebApp.Models;

namespace Cookit.WebApp.Pages.Recipes
{
    public class DetailsModel : PageModel
    {
        private readonly Cookit.WebApp.Data.CookitContext _context;

        public DetailsModel(Cookit.WebApp.Data.CookitContext context)
        {
            _context = context;
        }

        public Recipe Recipe { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Recipe = await _context.Recipes
                .Include(r => r.RecipeEquipment).ThenInclude(re => re.Equipment)
                .Include(r => r.IngredientAmounts).ThenInclude(ia => ia.Ingredient)
                .Include(r => r.Instructions)
                .AsNoTracking().FirstOrDefaultAsync(m => m.RecipeID == id);

            if (Recipe == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
