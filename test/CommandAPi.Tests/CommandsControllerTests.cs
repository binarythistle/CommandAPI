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
        public void GetCommandsReturnNItemsWhenDBHasNObjects()
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
        public void GetCommandsReturnsTheCorrectType()
        {
            //Arrange

            //Act
            var result = controller.GetCommandItems();

            //Assert
            Assert.IsType<ActionResult<IEnumerable<Command>>>(result);
        }
        

        [Fact]
        public void GetCommandsReturnsOneItemWhenDBHasOneObject()
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
        public void GetCommandsReturnsZeroItemsWhenDBIsEmpty()
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
        public void GetCommandItemReturnsNullResultWhenInvalidID()
        {
            //Arrange
            //DB should be empty, any ID will be invalid

            //Act
            var result = controller.GetCommandItem(0);

            //Assert
            Assert.Null(result.Value);
        }
        
        
        [Fact]
        public void GetCommandItemIDReturns404NotFoundWhenInvalidID()
        {
            //Arrange
            //DB should be empty, any ID will be invalid

            //Act
            var result = controller.GetCommandItem(0);

            //Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }
        
        [Fact]
        public void GetCommandItemReturnsTheCorrectType()
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

            var cmdId = command.Id;

            //Act
            var result = controller.GetCommandItem(cmdId);

            //Assert
            Assert.IsType<ActionResult<Command>>(result);
        }

        [Fact]
        public void GetCommandItemReturnsTheCorrectResouce()
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

            var cmdId = command.Id;

            //Act
            var result = controller.GetCommandItem(cmdId);

            //Assert
            Assert.Equal(cmdId, result.Value.Id);
        }

        //END OF ACTION 2 Tests: GET       /api/commands/id
        //---------------------------------------------------------------


        //ACTION 3 TESTS: POST       /api/commands

        //TEST 3.1 VALID OBJECT SUBMITTED – OBJECT COUNT INCREMENTS BY 1
        [Fact]
        public void PostCommandItemObjectCountIncrementWhenValidObject()
        {
            //Arrange
            var command = new Command
            { 
                HowTo = "Do Somethting",
                Platform = "Some Platform",
                CommandLine = "Some Command"
            };
            var oldCount = dbContext.CommandItems.Count();

            //Act
            var result = controller.PostCommandItem(command);

            //Assert
            Assert.Equal(oldCount + 1, dbContext.CommandItems.Count());
        }

        //TEST 3.2 TEST 3.2 VALID OBJECT SUBMITTED – VALID OBJECT RETURNED AS RESULT

        //TEST 3.3 VALID OBJECT SUBMITTED – 201 CREATED RETURN CODE
        [Fact]
        public void PostCommandItemReturns201CreatedWhenValidObject()
        {
            //Arrange
            var command = new Command
            { 
                HowTo = "Do Somethting",
                Platform = "Some Platform",
                CommandLine = "Some Command"
            };

            //Act
            var result = controller.PostCommandItem(command);

            //Assert
            Assert.IsType<CreatedAtActionResult>(result.Result);

        }

        //TEST 3.4 INVALID OBJECT SUBMITTED – OBJECT COUNT DOES NOT CHANGE

        //TEST 3.5 INVALID OBJECT SUBMITTED – 400 BAD REQUEST RETURN CODE
        [Fact]
        public void PostCommandItemReturns400BadRequestWhenInvalidObject()
        {
            //Arrange
            var command = new Command();

            //Act
            var result = controller.PostCommandItem(command);

            //Assert
            Assert.IsType<CreatedAtActionResult>(result.Result);
        }

        //TEST 3.6 SUBMIT OBJECT WITH ID – OBJECT COUNT DOES NOT CHANGE

        //TEST 3.7 SUBMIT OBJECT WITH ID – 400 BAD REQUEST RETURN CODE
        [Fact]
        public void PostCommandItemReturns400BadRequestWhenIDSupplied()
        {
            //Arrange
            var command = new Command
            { 
                Id = 1,
                HowTo = "Do Somethting",
                Platform = "Some Platform",
                CommandLine = "Some Command"
            };

            //Act
            var result = controller.PostCommandItem(command);

            //Assert
            Assert.IsType<CreatedAtActionResult>(result.Result);

        }


        //END OF ACTION 3 Tests: POST       /api/commands
        //---------------------------------------------------------------
    }

}

