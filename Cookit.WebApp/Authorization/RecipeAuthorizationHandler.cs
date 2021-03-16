using Cookit.WebApp.Models;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cookit.WebApp.Authorization
{
    public class RecipeAuthorizationHandler : AuthorizationHandler<OwnerRequirement, Recipe>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                                       OwnerRequirement requirement,
                                                       Recipe resource)
        {
            // kinda cheating here but need a way to delete recipes wrongly submitted or something
            if (context.User.IsInRole(WC.AdminRole) || context.User.Identity.Name == "admin@letscookitapp.com") 
            {
                context.Succeed(requirement);
            }

            if (context.User.Identity.Name == resource.OwnerEmail)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
