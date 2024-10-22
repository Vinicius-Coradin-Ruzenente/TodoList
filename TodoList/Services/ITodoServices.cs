using TodoList.Model;

namespace TodoList.Services
{
    public interface ITodoServices
    {
        Task<List<Todos>> GetTodosByUserId(int userId, DateTime? firstDate, DateTime? lastDate);
        Task<Todos?> CreateNewTodo(AddUpdateTodos todoObj);
        Task<Todos?> UpdateTodo(int id, AddUpdateTodos todoObj);
        Task<bool> DeleteTodo(int id);
    }

    
}
