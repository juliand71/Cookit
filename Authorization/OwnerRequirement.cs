﻿using Microsoft.AspNetCore.Authorization;

namespace Cookit.Authorization
{
    /**
     * This class doesn't seem to actually need an implementation, but it does seem to need to exist
     */
    public class OwnerRequirement : IAuthorizationRequirement
    {
    }
}
