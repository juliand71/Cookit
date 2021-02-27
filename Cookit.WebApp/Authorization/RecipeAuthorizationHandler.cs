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
            if (context.User.Identity.Name == resource.Owner)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
