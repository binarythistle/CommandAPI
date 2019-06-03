using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using CommandAPI.Models;
using Microsoft.AspNetCore.Hosting;


namespace CommandAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommandsController : ControllerBase
    {
        private readonly CommandContext _context;
        private IHostingEnvironment _hostEnv;

        public CommandsController(CommandContext context, IHostingEnvironment hostEnv)
        {
            _context = context;
            _hostEnv = hostEnv;
        }


        //GET:      api/commands
        [HttpGet]
        public ActionResult<IEnumerable<Command>> GetCommandItems()
        {
            //TODO - Can we apply this gloablly at the Controller level, rather than at the Action level
            if(Response != null)
                Response.Headers.Add("Environment", _hostEnv.EnvironmentName);

            return _context.CommandItems;
        }

        //GET:      api/commands/Id
        [HttpGet("{id}")]
        public ActionResult<Command> GetCommandItem(int id)
        {
            
            var commandItem = _context.CommandItems.Find(id);
            /*
            if(commandItem == null)
            {
                return NotFound();
            }
            */
            return commandItem; 
        }
        
        /*
        //POST:     api/commands
        [HttpPost]
        public ActionResult<Command> PostCommandItem(Command command)
        {
            _context.CommandItems.Add(command);
            _context.SaveChanges();

            return CreatedAtAction("GetCommandItem", new Command{Id = command.Id}, command);
        }
        */
    }
}

