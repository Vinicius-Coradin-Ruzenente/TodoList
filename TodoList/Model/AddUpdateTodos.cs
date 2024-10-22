namespace TodoList.Model
{
    public class AddUpdateTodos
    {
        public required string Task { get; set; }
        public bool IsDone { get; set; } = false;
        public required int UserId { get; set; }
    }
}
