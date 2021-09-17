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

        [Theory, AutoData]
        public async Task CreateAsync_ReturnsCreatedToDo(ToDoToCreate toDoToCreate)
        {
            // Arrange

            // Act
            var createdToDo = await _sut.CreateAsync(1, toDoToCreate);

            // Assert
            createdToDo.Should().NotBeNull();

            createdToDo.Id.Should().Be(2);
            createdToDo.Title.Should().Be(toDoToCreate.Title);
            createdToDo.Description.Should().Be(toDoToCreate.Description);
            createdToDo.Deadline.Should().Be(toDoToCreate.Deadline);
            createdToDo.Completed.Should().BeFalse();
        }


        #endregion

        #region GetAllUserToDosAsync

        [Fact]
        public async Task GetAllUserToDosAsync_ToDosExist_ReturnsToDos()
        {
            // Arrange
            
            // Act
            var toDos = await _sut.GetAllUserToDosAsync(1);

            // Assert
            toDos.Should().NotBeNull().And.HaveCountGreaterThan(0);
        }

        [Fact]
        public async Task GetAllUserToDosAsync_ToDosNotExist_ReturnsEmptyList()
        {
            // Arrange
            
            // Act
            var toDos = await _sut.GetAllUserToDosAsync(2);

            // Assert
            toDos.Should().NotBeNull().And.HaveCount(0);
        }

        #endregion
    }
}