using System;
using Library.Data.Repo;
using Library.Models.DTO;
using Library.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using NuGet.Packaging;

namespace Library.Services;

public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public PermissionAuthorizationHandler(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        var userId = context.User.Claims.FirstOrDefault(x => x.Type == "userId")?.Value;
        

        if (userId == null || !int.TryParse(userId, out var id))
        {
            return;
        }

        using var scope = _serviceScopeFactory.CreateScope();

        var userRepository = scope.ServiceProvider
            .GetRequiredService<UserRepository>();

        var userPermissions = await userRepository.GetUserPermissionsAsync(id);

        if(userPermissions.Intersect(requirement.Permissions).Any()){
            context.Succeed(requirement);
        }
    }
}   
