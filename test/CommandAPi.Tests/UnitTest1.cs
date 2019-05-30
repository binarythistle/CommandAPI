using System;
using Xunit;
using Microsoft.EntityFrameworkCore;    //Remove
using CommandAPI.Models;                //Remove
using CommandAPI.Controllers;           //Remove
using Microsoft.AspNetCore.Hosting;     //Remove
using Microsoft.AspNetCore.Mvc;         //Remove
using Moq;

namespace CommandAPi.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
           
            // Arrange
            // DB Context
            var optionsBuilder = new DbContextOptionsBuilder<CommandContext>();
            optionsBuilder.UseInMemoryDatabase("InMemDB");
            var _dbContext = new CommandContext(optionsBuilder.Options);

            // IHostingEnvironment
            var mockEnvironment = new Mock<IHostingEnvironment>();  
            mockEnvironment
                .Setup(m => m.EnvironmentName)
                .Returns("Unit Test");
            //var mockEnvironment = new FakeHostEnvironment{EnvironmentName = "Unit Test"};

            // Controller
            var _controller = new CommandsController(_dbContext, mockEnvironment.Object);

            // Act
            var result = _controller.GetCommandItems();

            // Assert - empty result set
            Assert.NotNull(result);

        }

        [Fact]
        public void ReturnsZeroItemsWhenDbIsEmpty()
        {
            // Arrange
            // DB Context
            var optionsBuilder = new DbContextOptionsBuilder<CommandContext>();
            optionsBuilder.UseInMemoryDatabase("InMemDB");
            var _dbContext = new CommandContext(optionsBuilder.Options);
            
            // IHostingEnvironment
            var mockEnvironment = new Mock<IHostingEnvironment>();  
            mockEnvironment
                .Setup(m => m.EnvironmentName)
                .Returns("Unit Test");
            //var mockEnvironment = new FakeHostEnvironment{EnvironmentName = "Unit Test"};

            // Controller
            var _controller = new CommandsController(_dbContext, mockEnvironment.Object);

            // Act
            var result = _controller.GetCommandItems();

            // Assert - empty result set
            //Assert.Empty(result.Value);
            
        }

        [Fact]
        public void ReturnsTwoItemsWhenDbHasTwoItems()
        {
            var itemOne = new Command
            {
                Id = 1,
                HowTo = "Do Something",
                Platform = "XUnit",
                CommandLine = "N/a" 
            };

            var itemTwo = new Command
            {
                Id = 2,
                HowTo = "Do Something Else",
                Platform = "XUnit",
                CommandLine = "N/a" 
            };

            // Arrange
            // DB Context
            var optionsBuilder = new DbContextOptionsBuilder<CommandContext>();
            optionsBuilder.UseInMemoryDatabase("InMemDB");
            var _dbContext = new CommandContext(optionsBuilder.Options);
            _dbContext.CommandItems.Add(itemOne);
            _dbContext.SaveChanges();

            // IHostingEnvironment
            var mockEnvironment = new Mock<IHostingEnvironment>();  
            mockEnvironment
                .Setup(m => m.EnvironmentName)
                .Returns("Unit Test");
            //var mockEnvironment = new FakeHostEnvironment{EnvironmentName = "Unit Test"};

            // Controller
            var _controller = new CommandsController(_dbContext, mockEnvironment.Object);

            // Act
            var result = _controller.GetCommandItems();

            // Assert - empty result set
            Assert.Single(result.Value);
           
        }
    }

}
