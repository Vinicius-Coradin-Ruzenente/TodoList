using Microsoft.EntityFrameworkCore;
using TodoList.Entity;
using TodoList.Model;

namespace TodoList.Services
{
    public class TodoServices : ITodoServices
    {
        private readonly TodoListDbContext _db;     

        public TodoServices(TodoListDbContext db)
        {
            _db = db;
        }
        public async Task<List<Todos>> GetTodosByUserId(int userId, DateTime? firstDate, DateTime? lastDate)
        {
            DateTime firstDateValue = firstDate ?? new DateTime(1, 1, 1); // '0001-01-01'
            DateTime lastDateValue = lastDate ?? new DateTime(9999, 12, 30); // '9999-12-31'

            return await _db.Todos.Where(todos => todos.UserId == userId && todos.CreatedAt >= firstDateValue && todos.CreatedAt <= lastDateValue.AddDays(1)).ToListAsync();
        }

        public async Task<Todos?> CreateNewTodo(AddUpdateTodos todoObj)
        {
            var todo = new Todos()
            {
                Task = todoObj.Task,
                UserId = todoObj.UserId,
            };
            _db.Todos.Add(todo);
            var result = await _db.SaveChangesAsync();
            return result >= 0 ? todo : null;
        }

        public async Task<Todos?> UpdateTodo(int id, AddUpdateTodos todoObj)
        {
            var todo = await _db.Todos.FirstOrDefaultAsync(index => index.Id == id);
            if (todo != null)
            {               
                todo.Task = todoObj.Task;
                todo.IsDone = todoObj.IsDone;
                todo.UserId = todo.UserId;
                if (todoObj.IsDone != false)
                {
                    todo.DoneAt = DateTime.UtcNow.ToLocalTime();
                }

                var result = await _db.SaveChangesAsync();
                return result >= 0 ? todo : null;
            }
                return null;            
        }

        public async Task<bool> DeleteTodo(int id)
        {
            var todo = await _db.Todos.FirstOrDefaultAsync(index => index.Id == id);
            if (todo != null)
            {
                _db.Todos.Remove(todo);
                var result = await _db.SaveChangesAsync();
                return result >= 0;
            }
            return false;
        }

    }
}
