using Cookit.Models;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cookit.Authorization
{
    public class RecipeAuthorizationHandler : AuthorizationHandler<OwnerRequirement, Recipe>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                                       OwnerRequirement requirement,
                                                       Recipe resource)
        {
            // if the user is an admin, or if they are the author of the recipe
            if (context.User.IsInRole("Admin") || context.User.Identity.Name == resource.Author.UserName)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
