using Cookit.Data;
using Cookit.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cookit.Models.PageModels
{
    /// <summary>
    /// Class acting as a base for all the Recipe CRUD Pages
    /// </summary>
    public class RecipePageModel : PageModel
    {
        protected CookitContext _context;
        protected IAuthorizationService _authorizationService;
        protected UserManager<CookitUser> _userManager;
        protected ImageHandler _ifs;

        public RecipePageModel(
            CookitContext context,
            IAuthorizationService authorizationService,
            UserManager<CookitUser> userManager,
            ImageHandler ifs) : base()
        {
            _context = context;
            _authorizationService = authorizationService;
            _userManager = userManager;
            _ifs = ifs;
        }
    }
}
