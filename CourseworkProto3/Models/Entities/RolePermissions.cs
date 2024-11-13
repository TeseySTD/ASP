using System;
using System.Collections.ObjectModel;

namespace Library.Models.Entities;

public static class RolePermissions
{
    private static readonly Dictionary<Roles, List<Permission>> _permissions = new()
    {
        { Roles.Owner, new List<Permission> { Permission.OwnerOnly, Permission.AdminOnly, Permission.OperatorOnly, Permission.Default } },
        { Roles.Administrator, new List<Permission> { Permission.AdminOnly, Permission.OperatorOnly, Permission.Default } },
        { Roles.Operator, new List<Permission> { Permission.OperatorOnly, Permission.Default } },
        { Roles.Default, new List<Permission> { Permission.Default } }
    };

    public static readonly ReadOnlyDictionary<Roles, List<Permission>> Permissions = 
        new(_permissions);
}



