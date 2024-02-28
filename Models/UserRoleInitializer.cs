using Microsoft.AspNetCore.Identity;

namespace EpicBookstoreSprint.Models
{
    public static class UserRoleInitializer
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            var userManger = serviceProvider.GetRequiredService<UserManager<DefaultUser>>();

            string[] roleNames = { "Admin", "User" };

            IdentityResult roleResult;

            foreach (var role in roleNames)
            {
                var roleExists = await roleManager.RoleExistsAsync(role);

                if (!roleExists)
                {
                    roleResult = await roleManager.CreateAsync(new IdentityRole(role));
                }



            }

            var email = "admin@gmail.com";
            var password = "Password123_";

            if (userManger.FindByEmailAsync(email).Result == null)
            {
                DefaultUser user = new()
                {
                    Email = email,
                    UserName = email,
                    FirstName = "Admin",
                    LastName = "Admission",
                    Address = "Adstreet 3",
                    City = "Big city",
                    ZipCode = "12345"
                   

                };

                IdentityResult result = userManger.CreateAsync(user, password).Result;

                if (result.Succeeded)
                {
                    userManger.AddToRoleAsync(user,"Admin").Wait();
                }
            }

        }
    }
}
