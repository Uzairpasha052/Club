using backend.Models;
using Microsoft.AspNetCore.Identity;

// namespace backend.Data;

namespace backend.Data
{
    public static class SeedData
    {
        public static async Task Initialize(AppDbContext context)
        {
            if (context.Users.Any())
                return;

            var admin = new User
            {
                FullName = "System Admin",
                Email = "admin@company.com",
                Role = "Admin"
            };

            var passwordHasher = new PasswordHasher<User>();
            admin.Password = passwordHasher.HashPassword(admin, "Admin@123");

            context.Users.Add(admin);
            await context.SaveChangesAsync();
        }
    }
}