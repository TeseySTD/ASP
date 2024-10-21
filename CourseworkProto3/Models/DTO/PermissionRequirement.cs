using System;
using Library.Models.Entities;
using Microsoft.AspNetCore.Authorization;

namespace Library.Models.DTO;

public class PermissionRequirement : IAuthorizationRequirement
{
    public Permission[] Permissions { get; private set; }
    public PermissionRequirement(params Permission[] permissions)
    {
        Permissions = permissions;
    }
}
