using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cookit.Models;
using Microsoft.AspNetCore.Identity;

namespace Cookit.Data
{
    // Add profile data for application users by adding properties to the CookitUser class
    public class CookitUser : IdentityUser
    {
        // no use for this as of yet
        //public int Hash { get; set; }

        //public List<Recipe> Recipes { get; set; }
        //public List<Rating> Ratings { get; set; }
    }
}
