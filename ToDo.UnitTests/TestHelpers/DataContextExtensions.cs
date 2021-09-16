using System;
using ToDo.API.Data;
using ToDo.API.Entities;
using ToDo.API.Enum;

namespace ToDo.UnitTests.TestHelpers
{
    public static class DataContextExtensions
    {
        public static void SeedUsers(this DataContext context)
        {
            var users = new[]
            {
                new User
                {
                    Id = 1,
                    ExternalId = "1",
                    Email = "user1@mail.com",
                    Username = "user1",
                    Provider = ExternalAuthProvider.Google,
                    ProfilePictureUrl = "www.example.com/picture/1",
                    CreatedAt = DateTimeOffset.UtcNow,
                    UpdatedAt = DateTimeOffset.UtcNow
                },
                new User
                {
                    Id = 2,
                    ExternalId = "2",
                    Email = "user2@mail.com",
                    Username = "user2",
                    Provider = ExternalAuthProvider.Google,
                    ProfilePictureUrl = "www.example.com/picture/2",
                    CreatedAt = DateTimeOffset.UtcNow,
                    UpdatedAt = DateTimeOffset.UtcNow
                }
            };

            context.Users.AddRange(users);

            context.SaveChanges();
        }
    }
}