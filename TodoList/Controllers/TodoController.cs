using TodoList.Model;
using TodoList.Services;
using TodoList.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace TodoList.Controllers
{
    [Route("api/v1/todos/[controller]")]
    [ApiController]
    [Authorize]
    public class TodoController(ITodoServices todoService) : ControllerBase
    {
        public readonly ITodoServices _todoService = todoService;

        [HttpGet]
        [Route("{userId}")]
        public async Task<IActionResult> Get([FromRoute] int userId, [FromQuery(Name = "fdate")] DateTime? firstDate, [FromQuery(Name = "ldate")] DateTime? lastDate)
        {           

            var todos = await _todoService.GetTodosByUserId(userId, firstDate, lastDate);

            if (lastDate < firstDate)
            {
                return BadRequest("Last date cannot be less than the first date");
            }

            if (todos == null)
            {
                return NotFound();
            }
            return Ok(todos);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] AddUpdateTodos todoObj)
        {
            var todo = await _todoService.CreateNewTodo(todoObj);
            if (todo == null)
            {
                return BadRequest();
            }
            return Ok(todo);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Put([FromRoute] int id, AddUpdateTodos todoObj)
        {
            var todo = await _todoService.UpdateTodo(id, todoObj);
            if (todo == null)
            {
                return NotFound();
            }
            return Ok(todo);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!await _todoService.DeleteTodo(id))
            {
                return NotFound();
            }
            return Ok(new
            {
                message = "Todo deleted succesfully",
                id
            });
        }
    };
}
