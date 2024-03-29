﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Cookit.Data;
using Cookit.Models;
using Cookit.Models.PageModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Cookit.Services;

namespace Cookit.Pages.Recipes
{
    public class DeleteModel : RecipePageModel
    {

        public DeleteModel(
            CookitContext context,
            IAuthorizationService authorizationService,
            UserManager<CookitUser> userManager,
            ImageHandler ifs) : base(context, authorizationService, userManager, ifs)
        {

        }

        [BindProperty]
        public Recipe Recipe { get; set; }
        public string ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            Recipe = await _context.Recipes
                .Include(r => r.Author)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);

            if (Recipe == null)
            {
                return NotFound();
            }

            var authorizeResult = await _authorizationService.AuthorizeAsync(User, Recipe, "CUDPolicy");

            if (authorizeResult.Succeeded)
            {
                if (saveChangesError.GetValueOrDefault())
                {
                    ErrorMessage = "Delete failed. Try again";
                }

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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipe = await _context.Recipes
                .Include(r => r.Author)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);

            if (recipe == null)
            {
                return NotFound();
            }

            var authorizeResult = await _authorizationService.AuthorizeAsync(User, recipe, "CUDPolicy");

            if (authorizeResult.Succeeded)
            {
                try
                {
                    _context.Recipes.Remove(recipe);
                    await _context.SaveChangesAsync();
                    return RedirectToPage("./Index");
                }
                catch (DbUpdateException /* ex */)
                {
                    //Log the error (uncomment ex variable name and write a log.)
                    return RedirectToAction("./Delete",
                                         new { id, saveChangesError = true });
                }
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
    }
}
