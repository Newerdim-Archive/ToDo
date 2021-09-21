using System;
using System.Collections.Generic;
using ToDo.API.Data;
using ToDo.API.Entities;
using ToDo.API.Enum;

namespace ToDo.IntegrationTests.Helpers
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
                    Username = "User1",
                    Email = "User1@mail.com",
                    ExternalId = "1",
                    Provider = ExternalAuthProvider.Google,
                    ProfilePictureUrl = "www.example.com/picure/1",
                    CreatedAt = DateTimeOffset.UtcNow,
                    UpdatedAt = DateTimeOffset.UtcNow,
                    ToDos = new List<API.Entities.ToDo>
                    {
                        new()
                        {
                            Id = 1,
                            Title = "Task 1",
                            Description = "Description",
                            Deadline = DateTimeOffset.UtcNow,
                            Completed = false,
                            UserId = 1,
                            CreatedAt = DateTimeOffset.UtcNow,
                            UpdatedAt = DateTimeOffset.UtcNow
                        },
                        new()
                        {
                            Id = 2,
                            Title = "Task 2",
                            Description = "Description",
                            Deadline = DateTimeOffset.UtcNow,
                            Completed = false,
                            UserId = 1,
                            CreatedAt = DateTimeOffset.UtcNow,
                            UpdatedAt = DateTimeOffset.UtcNow
                        },
                        new()
                        {
                            Id = 3,
                            Title = "Task 3",
                            Description = "Description",
                            Deadline = DateTimeOffset.UtcNow,
                            Completed = true,
                            UserId = 1,
                            CreatedAt = DateTimeOffset.UtcNow,
                            UpdatedAt = DateTimeOffset.UtcNow
                        }
                    }
                },
                new User
                {
                    Id = 2,
                    Username = "User2",
                    Email = "User2@mail.com",
                    ExternalId = "2",
                    Provider = ExternalAuthProvider.Google,
                    ProfilePictureUrl = "www.example.com/picure/2",
                    CreatedAt = DateTimeOffset.UtcNow,
                    UpdatedAt = DateTimeOffset.UtcNow
                }
            };

            context.Users.AddRange(users);
            context.SaveChanges();
        }
    }
}