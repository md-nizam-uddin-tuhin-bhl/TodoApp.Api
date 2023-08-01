using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApp.Api.Data;
using TodoApp.Api.Models;

namespace TodoApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodosController : ControllerBase
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public TodosController(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        [HttpGet]
        public async Task<IActionResult> getAllTodo()    
        {
            var todo = await _applicationDbContext.todos.ToListAsync();
            return Ok(todo);
        }
        [HttpPost]
        public async Task<IActionResult> AddTodo([FromBody]Todo todo)
        {
           todo.Id = Guid.NewGuid();
           await  _applicationDbContext.todos.AddAsync(todo);
           await _applicationDbContext.SaveChangesAsync();
            return Ok(todo);
        }
        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Updatetodo( [FromRoute]Guid id,Todo todos)
        {
            var todo = await _applicationDbContext.todos.FindAsync(id);
            if(todo == null)
            {
                return NotFound();
            }
            todo.IsCompleted = todos.IsCompleted;
            todo.CompletedDate = DateTime.Now;
            await _applicationDbContext.SaveChangesAsync();
            return Ok(todo);
        }

    }
}
