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

namespace Cookit.WebApp.Pages.Recipes
{
    [AllowAnonymous]
    public class IndexModel : PageModel
    {
        private readonly Cookit.WebApp.Data.CookitContext _context;

        public IndexModel(Cookit.WebApp.Data.CookitContext context)
        {
            _context = context;
        }


        public IList<Recipe> Recipe { get;set; }

        public async Task OnGetAsync()
        {

            Recipe = await _context.Recipes.ToListAsync();
        }
    }
}
