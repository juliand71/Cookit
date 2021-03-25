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

namespace Cookit.Pages.Recipes
{
    [AllowAnonymous]
    public class IndexModel : PageModel
    {
        private readonly Cookit.Data.CookitContext _context;

        public IndexModel(Cookit.Data.CookitContext context)
        {
            _context = context;
        }

        public IList<Recipe> Recipe { get;set; }

        public async Task OnGetAsync()
        {
            // need to add paging here at some point
            Recipe = await _context.Recipes.Include(r => r.Author).ToListAsync();
        }
    }
}
