using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Cookit.WebApp.Models
{
    public class CookitUser : IdentityUser
    {

        // "handle" for the user so that the e-mail's aren't displayed publicly
        // ie something like @jdjuice
        public string Handle { get; set; }
        
    }
}
