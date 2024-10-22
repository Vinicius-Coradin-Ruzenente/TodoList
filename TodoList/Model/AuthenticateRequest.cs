using System.ComponentModel;

namespace TodoList.Model
{
    public class AuthenticateRequest
    {
        [DefaultValue("Test")]
        public required string Username { get; set; }

        [DefaultValue("test123")]
        public required string Password { get; set; }
    }
}
