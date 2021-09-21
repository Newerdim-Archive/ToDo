using System;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using ToDo.API.Data;
using ToDo.API.Dto;
using ToDo.API.Helpers;
using ToDo.API.Services;
using ToDo.API.Services.Implementations;
using ToDo.UnitTests.DataFixture;
using Xunit;

namespace ToDo.UnitTests.Services
{
    public class ToDoServiceTests : IClassFixture<SeededDataFixture>
    {
        private readonly DataContext _context;
        private readonly ToDoService _sut;

        public ToDoServiceTests(SeededDataFixture fixture)
        {
            _context = fixture.Context;

            var mapperConfiguration = new MapperConfiguration(config =>
            {
                config.AddProfile<MapperProfile>();
            });

            var mapper = new Mapper(mapperConfiguration);

            _sut = new ToDoService(_context, mapper);
        }

        #region CreateAsync

        [Theory]
        [AutoData]
        public async Task CreateAsync_ReturnsCreatedToDo(ToDoToCreate toDoToCreate)
        {
            // Arrange
            var expectedId = await _context.ToDos.CountAsync() + 1;

            // Act
            var createdToDo = await _sut.CreateAsync(1, toDoToCreate);

            // Assert
            createdToDo.Should().NotBeNull();

            createdToDo.Id.Should().Be(expectedId);
            createdToDo.Title.Should().Be(toDoToCreate.Title);
            createdToDo.Description.Should().Be(toDoToCreate.Description);
            createdToDo.Deadline.Should().Be(toDoToCreate.Deadline);
            createdToDo.Completed.Should().BeFalse();
        }

        #endregion

        #region GetAllAsync

        [Fact]
        public async Task GetAllAsync_Exist_ReturnsToDos()
        {
            // Arrange

            // Act
            var toDos = await _sut.GetAllAsync(1);

            // Assert
            toDos.Should().NotBeNull().And.HaveCountGreaterThan(0);
        }

        [Fact]
        public async Task GetAllAsync_NotExist_ReturnsEmptyList()
        {
            // Arrange

            // Act
            var toDos = await _sut.GetAllAsync(2);

            // Assert
            toDos.Should().NotBeNull().And.HaveCount(0);
        }

        #endregion

        #region GetByIdAsync

        [Fact]
        public async Task GetByIdAsync_Exists_ReturnsToDo()
        {
            // Arrange

            // Act
            var toDo = await _sut.GetByIdAsync(1, 1);

            // Assert
            toDo.Should().NotBeNull();
            toDo.Id.Should().Be(1);
        }

        [Fact]
        public async Task GetByIdAsync_NotExist_ReturnsNull()
        {
            // Arrange

            // Act
            var toDo = await _sut.GetByIdAsync(1, 99);

            // Assert
            toDo.Should().BeNull();
        }

        [Fact]
        public async Task GetByIdAsync_ExistsButWithDifferentUserId_ReturnsNull()
        {
            // Arrange

            // Act
            var toDo = await _sut.GetByIdAsync(2, 1);

            // Assert
            toDo.Should().BeNull();
        }

        #endregion

        #region UpdateAsync

        [Fact]
        public async Task UpdateAsync_Exists_ReturnsUpdatedToDo()
        {
            // Arrange
            var toDoToUpdate = new ToDoToUpdate
            {
                Id = 1,
                Title = "updated title",
                Description = "updated description",
                Deadline = DateTimeOffset.UtcNow,
                Completed = true
            };

            // Act
            var updatedToDo = await _sut.UpdateAsync(1, toDoToUpdate);

            // Assert
            updatedToDo.Should().NotBeNull();
        }

        [Fact]
        public async Task UpdateAsync_Exists_UpdatesToDoInDb()
        {
            // Arrange
            var toDoToUpdate = new ToDoToUpdate
            {
                Id = 1,
                Title = "updated title 1",
                Description = "updated description 1",
                Deadline = DateTimeOffset.UtcNow,
                Completed = true
            };

            // Act
            var updatedToDo = await _sut.UpdateAsync(1, toDoToUpdate);

            var toDoInDb = await _context.ToDos.AsNoTracking().FirstAsync(t => t.Id == updatedToDo.Id);

            // Assert
            updatedToDo.Should().NotBeNull();

            updatedToDo.Id.Should().Be(toDoInDb.Id);
            updatedToDo.Title.Should().Be(toDoInDb.Title);
            updatedToDo.Description.Should().Be(toDoInDb.Description);
            updatedToDo.Deadline.Should().Be(toDoInDb.Deadline);
            updatedToDo.Completed.Should().Be(toDoInDb.Completed);

            toDoInDb.UpdatedAt.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(10));
        }

        [Fact]
        public async Task UpdateAsync_NotExist_ReturnsNull()
        {
            // Arrange
            var toDoToUpdate = new ToDoToUpdate
            {
                Id = 999,
                Title = "updated title",
                Description = "updated description",
                Deadline = DateTimeOffset.UtcNow,
                Completed = true
            };

            // Act
            var updatedToDo = await _sut.UpdateAsync(1, toDoToUpdate);

            // Assert
            updatedToDo.Should().BeNull();
        }

        [Fact]
        public async Task UpdateAsync_ExistButWithDifferentUserId_ReturnsNull()
        {
            // Arrange
            var toDoToUpdate = new ToDoToUpdate
            {
                Id = 1,
                Title = "updated title",
                Description = "updated description",
                Deadline = DateTimeOffset.UtcNow,
                Completed = true
            };

            // Act
            var updatedToDo = await _sut.UpdateAsync(2, toDoToUpdate);

            // Assert
            updatedToDo.Should().BeNull();
        }

        #endregion

        #region DeleteAsync

        [Fact]
        public async Task DeleteAsync_ToDoExists_ReturnsTrue()
        {
            // Arrange

            // Act
            var result = await _sut.DeleteAsync(1, 3);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteAsync_ToDoExists_DeletedToDoFromDb()
        {
            // Arrange
            const int userId = 1;
            const int toDoId = 2;

            // Act
            var result = await _sut.DeleteAsync(userId, toDoId);

            var exists = await _context.ToDos.AnyAsync(t => t.Id == toDoId);

            // Assert
            result.Should().BeTrue();

            exists.Should().BeFalse();
        }

        [Fact]
        public async Task DeleteAsync_ToDoNotExist_ReturnsFalse()
        {
            // Arrange

            // Act
            var result = await _sut.DeleteAsync(1, 999);

            // Assert
            result.Should().BeFalse();
        }

        #endregion
    }
}