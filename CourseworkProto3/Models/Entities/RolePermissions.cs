using System;
using System.Collections.ObjectModel;

namespace Library.Models.Entities;

public static class RolePermissions
{
    private static readonly Dictionary<Roles, List<Permission>> _permissions = new()
    {
        { Roles.Owner, new List<Permission> { Permission.Create, Permission.Read, Permission.Update, Permission.Delete } },
        { Roles.Administrator, new List<Permission> { Permission.Create, Permission.Read, Permission.Update } },
        { Roles.Operator, new List<Permission> { Permission.Read, Permission.Update } },
        { Roles.Default, new List<Permission> { Permission.Read } }
    };

    public static readonly ReadOnlyDictionary<Roles, List<Permission>> Permissions = 
        new(_permissions);
}



