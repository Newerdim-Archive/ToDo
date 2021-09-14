using System;
using Microsoft.EntityFrameworkCore;
using ToDo.API.Data;
using ToDo.UnitTests.TestHelpers;

namespace ToDo.UnitTests.DataFixture
{
    public class SeededDataFixture : IDisposable
    {
        public readonly DataContext Context;

        public SeededDataFixture()
        {
            Context = CreateNewEmptyDb();
            Context.SeedUsers();
        }

        public void Dispose()
        {
            Context.Dispose();
            GC.SuppressFinalize(this);
        }

        private static DataContext CreateNewEmptyDb()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new DataContext(options);
        }
    }
}