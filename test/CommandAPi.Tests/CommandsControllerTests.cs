using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;   
using CommandAPI.Controllers;
using CommandAPI.Models;
using Moq;
using Xunit;

namespace CommandAPi.Tests
{
    public class CommandsControllerTests : IDisposable
    {
        DbContextOptionsBuilder<CommandContext> optionsBuilder;
        CommandContext dbContext;
        Mock<IHostingEnvironment> mockEnvironment;
        CommandsController controller;

        public CommandsControllerTests()
        {
            optionsBuilder = new DbContextOptionsBuilder<CommandContext>();
            optionsBuilder.UseInMemoryDatabase("UnitTestInMemBD");
            dbContext = new CommandContext(optionsBuilder.Options);
            mockEnvironment = new Mock<IHostingEnvironment>();
            mockEnvironment.Setup(m => m.EnvironmentName).Returns("UnitTest");
            controller = new CommandsController(dbContext, mockEnvironment.Object);
            
        }

        public void Dispose()
        {
            optionsBuilder = null;
            foreach (var cmd in dbContext.CommandItems)
            {
                dbContext.CommandItems.Remove(cmd);
            }
            dbContext.SaveChanges();
            dbContext.Dispose();
            mockEnvironment = null;
            controller = null;
        }
        
        //ACTION 1 Tests: GET       /api/commands

        [Fact]
        public void ReturnNItemsWhenDBHasNObjects()
        {
            //Arrange
            var command = new Command
            { 
                HowTo = "Do Somethting",
                Platform = "Some Platform",
                CommandLine = "Some Command"
            };
            var command2 = new Command
            {
                HowTo = "Do Somethting",
                Platform = "Some Platform",
                CommandLine = "Some Command"
            };
            dbContext.CommandItems.Add(command);
            dbContext.CommandItems.Add(command2);
            dbContext.SaveChanges();

            //Act
            var result = controller.GetCommandItems();

            //Assert
            Assert.Equal(2, result.Value.Count());
        }
        
        [Fact]
        public void ReturnsTheCorrectType()
        {
            //Arrange

            //Act
            var result = controller.GetCommandItems();

            //Assert
            Assert.IsType<ActionResult<IEnumerable<Command>>>(result);
        }
        

        [Fact]
        public void ReturnsOneItemWhenDBHasOneObject()
        {
            //Arrange
            var command = new Command
            { 
                HowTo = "Do Somethting",
                Platform = "Some Platform",
                CommandLine = "Some Command"
            };

            dbContext.CommandItems.Add(command);
            dbContext.SaveChanges();

            //Act
            var result = controller.GetCommandItems();

            //Assert
            Assert.Single(result.Value);
        }

        [Fact]
        public void ReturnsZeroItemsWhenDBIsEmpty()
        {
            //Arrange

            //Act
            var result = controller.GetCommandItems();

            //Assert
            Assert.Empty(result.Value);
            
        }

        //END OF ACTION 1 Tests: GET       /api/commands

        //---------------------------------------------------------------

        //ACTION 2 Tests: GET       /api/commands/id
        
        [Fact]
        public void ReturnsNullResultWhenInvalidID()
        {
            //Arrange
            //DB should be empty, any ID will be invalid

            //Act
            var result = controller.GetCommandItem(0);

            //Assert
            Assert.Null(result.Value);
        }
        
        
        [Fact]
        public void Returns404NotFoundWhenInvalidID()
        {
            //Arrange
            //DB should be empty, any ID will be invalid

            //Act
            var result = controller.GetCommandItem(0);

            //Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }
        
        


        //END OF ACTION 2 Tests: GET       /api/commands/id

        //---------------------------------------------------------------
    }

}

