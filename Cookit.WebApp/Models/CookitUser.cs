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
        /**
         * This property is really mostly here just cause I'm still learning how to use the Identity
         * Objects, Haven't found a practical purpose for making a subclass of IdentityUser yet tbh
         * Doesn't seem to be a downside though so maybe somewhere down the line I'll need to store more
         * info on users
         */
        [DisplayName("Full Name")]
        public string FullName { get; set; }
    }
}
