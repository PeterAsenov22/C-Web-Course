namespace ChushkaWebApp.Web.Utilities
{
    using System;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;

    public static class Seeder
    {
        public static void Seed(IServiceProvider provider)
        {
            var roleManager = provider.GetService<RoleManager<IdentityRole>>();
            var adminRoleExists = roleManager.RoleExistsAsync("Administrator").Result;
            if (!adminRoleExists)
            {
                var result = roleManager.CreateAsync(new IdentityRole("Administrator")).Result;
                if (!result.Succeeded)
                {
                    throw new InvalidOperationException();
                }
            }
        }
    }
}
