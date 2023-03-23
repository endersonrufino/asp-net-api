using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyApi.Data;
using MyApi.Models;

namespace MyApi.Controllers
{
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet("/")]
        public IActionResult Get([FromServices] AppDbContext context)
            => Ok(context.Todos.AsNoTracking().ToList());

        [HttpGet("/{id:int}")]
        public IActionResult GetById([FromRoute] int id, [FromServices] AppDbContext context)
        => Ok(context.Todos.AsNoTracking().FirstOrDefault(x => x.Id == id));        

        [HttpPost("/")]
        public IActionResult Post([FromBody] Todo todo, [FromServices] AppDbContext context)
        {
            context.Todos.Add(todo);
            context.SaveChanges();

            return Created($"/{todo.Id}", todo);
        }

        [HttpPut("/")]
        public IActionResult Put([FromBody] Todo todo, [FromServices] AppDbContext context)
        {
            var existingTodo = context.Todos.FirstOrDefault(x => x.Id == todo.Id);
            existingTodo.Title = todo.Title;
            existingTodo.Done = todo.Done;

            context.Todos.Update(existingTodo);
            context.SaveChanges();

            return Ok(existingTodo);
        }

        [HttpDelete("/{id:int}")]
        public IActionResult Delete([FromRoute] int id, [FromServices] AppDbContext context)
        {
            var existingTodo = context.Todos.FirstOrDefault(x => x.Id == id);

            context.Todos.Remove(existingTodo);
            context.SaveChanges();

            return Ok(existingTodo);
        }
    }
}