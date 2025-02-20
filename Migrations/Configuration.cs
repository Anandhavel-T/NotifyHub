using NotifyHub.Models.Domain;
using System.Data.Entity.Migrations;
using System;

namespace NotifyHub.Data.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<NotifyHub.Data.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "NotifyHub.Data.ApplicationDbContext";
        }

        protected override void Seed(NotifyHub.Data.ApplicationDbContext context)
        {
            // Add any seed data here
            context.Users.AddOrUpdate(
                u => u.UserName,
                new User
                {
                    FullName = "System Admin",
                    UserName = "admin",
                    PasswordHash = "YOUR_HASHED_PASSWORD", // Use proper password hashing
                    Email = "admin@system.com",
                    IsActive = true,
                    LastLoginDate = DateTime.UtcNow,
                    CreatedAt = DateTime.UtcNow
                }
            );
        }
    }
}