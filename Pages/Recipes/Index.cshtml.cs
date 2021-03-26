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
using Microsoft.Extensions.Options;

namespace Cookit.Pages.Recipes
{
    [AllowAnonymous]
    public class IndexModel : PageModel
    {
        private readonly CookitContext _context;
        private readonly MvcOptions _mvcOptions;
        public IndexModel(CookitContext context, IOptions<MvcOptions> mvcOptions)
        {
            _context = context;
            _mvcOptions = mvcOptions.Value;
        }

        public IList<Recipe> Recipes { get;set; }

        public async Task OnGetAsync(string sortOrder, string searchField)
        {
            IQueryable<Recipe> recipeIQ = from r in _context.Recipes.Take(25) select r;

            switch (sortOrder)
            {
                case "new":
                    recipeIQ = recipeIQ.OrderBy(r => r.DatePosted);
                    break;
                case "top":
                    recipeIQ = recipeIQ.OrderByDescending(r => r.AverageScore);
                    break;
                default:
                    recipeIQ = recipeIQ.OrderBy(r => r.DatePosted);
                    break;
            }

            Recipes = await recipeIQ
                .Include(r => r.Author)
                .Include(r => r.Ratings)
                .AsNoTracking().ToListAsync();
        }
    }
}
