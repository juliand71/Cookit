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
    public class IndexModel : PageModel
    {
        private readonly Cookit.WebApp.Data.CookitContext _context;

        public IndexModel(Cookit.WebApp.Data.CookitContext context)
        {
            _context = context;
        }

        public string TitleSort { get; set; }
        public string CurrentFilter { get; set; }
        public string CurrentSort { get; set; }
        public IList<Recipe> Recipe { get;set; }

        public async Task OnGetAsync(string sortOrder)
        {
            TitleSort = String.IsNullOrEmpty(sortOrder) ? "title_desc" : "";

            IQueryable<Recipe> recipesIQ = from r in _context.Recipes select r;

            switch (sortOrder)
            {
                case "title_desc":
                    recipesIQ.OrderByDescending(r => r.Title);
                    break;
                default:
                    recipesIQ.OrderByDescending(r => r.Title);
                    break;
            }

            Recipe = await recipesIQ.AsNoTracking().ToListAsync();
        }
    }
}
