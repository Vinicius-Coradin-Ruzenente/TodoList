namespace TodoList.Model
{
    public class AddUpdateUser
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
        public bool IsActive { get; set; } = true;
        public required string Email { get; set; }
    }
}
