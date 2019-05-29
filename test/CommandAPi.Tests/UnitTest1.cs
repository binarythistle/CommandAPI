using System;
using Xunit;
using Microsoft.EntityFrameworkCore;
using CommandAPI.Models;
using CommandAPI.Controllers;
using Microsoft.AspNetCore.Hosting;

namespace CommandAPi.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
           

            var optionsBuilder = new DbContextOptionsBuilder<CommandContext>();
            optionsBuilder.UseInMemoryDatabase();
            var _dbContext = new CommandContext(optionsBuilder.Options);

            var _controller = new CommandsController(_dbContext);

        }
    }

}
