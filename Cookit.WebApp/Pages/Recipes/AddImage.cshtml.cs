using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Cookit.WebApp.Data;
using Cookit.WebApp.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Cookit.WebApp.Pages.Recipes
{
    public class AddImageModel : PageModel
    {
        private readonly CookitContext _context;
        private readonly IWebHostEnvironment _env;

        public AddImageModel(CookitContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        [BindProperty]
        public Recipe Recipe { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // get our recipe
            Recipe = await _context.Recipes.FindAsync(id);

            if (Recipe == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostUploadAsync(int id, IFormFile ImageFile)
        {
            // get our recipe
            Recipe = await _context.Recipes.FindAsync(id);

            if (Recipe == null)
            {
                return NotFound();
            }

            var _imageDir = _env.WebRootPath;
            var _imgFileName = Path.Combine(Path.GetRandomFileName(), ".jpg");
            var fullImagePath = Path.Combine(_imageDir, _imgFileName);
            using (var fileStream = new FileStream(fullImagePath, FileMode.Create, FileAccess.Write))
            {
                await ImageFile.CopyToAsync(fileStream);
            }

            Recipe.ImageFileName = _imgFileName;

            return RedirectToPage("./Details", new { id = id });
        }
    }
}
