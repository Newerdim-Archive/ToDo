using System;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using AutoMapper;
using FluentAssertions;
using ToDo.API.Dto;
using ToDo.API.Helpers;
using ToDo.API.Services;
using ToDo.UnitTests.DataFixture;
using Xunit;

namespace ToDo.UnitTests.Services
{
    public class ToDoServiceTests : IClassFixture<SeededDataFixture>
    {
        private readonly ToDoService _sut;

        public ToDoServiceTests(SeededDataFixture fixture)
        {
            var mapperConfiguration = new MapperConfiguration(config =>
            {
                config.AddProfile<MapperProfile>();
            });

            var mapper = new Mapper(mapperConfiguration);
            
            _sut = new ToDoService(fixture.Context, mapper);
        }

        #region CreateAsync

        [Fact]
        public async Task CreateAsync_ReturnsCreatedToDo()
        {
            // Arrange
            var todo = new CreateToDo
            {
                UserId = 1,
                Title = "title",
                Description = "description",
                Deadline = DateTimeOffset.UtcNow.AddDays(1)
            };
            
            // Act
            var createdToDo = await _sut.CreateAsync(todo);

            // Assert
            createdToDo.Should().NotBeNull();

            createdToDo.Id.Should().Be(2);
            createdToDo.Title.Should().Be(todo.Title);
            createdToDo.Description.Should().Be(todo.Description);
            createdToDo.Deadline.Should().Be(todo.Deadline);
            createdToDo.Completed.Should().BeFalse();
        }


        #endregion
    }
}