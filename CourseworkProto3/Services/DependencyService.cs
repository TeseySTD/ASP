using System;
using System.Collections.Generic;
using System.Text;
using Library.Data.Repo;
using Microsoft.AspNetCore.Authorization;

namespace Library.Services
{
    public static class DependencyService
    {
        public static void InjectDependencies(IServiceCollection services){
            services.AddScoped<UserService>();
            services.AddScoped<UserRepository>();
            services.AddScoped<ProductRepository>();
            
            services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();
        } 
    }
}
