using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using TodoList.Helpers;
using TodoList.Model;

namespace TodoList.Model
{
    public class User
    {
        public int Id { get; set; }
        public required string Username { get; set; }
        public required string Password { get; set; }
        public required string Email { get; set; }
        [JsonConverter(typeof(DateOnlyJsonConverter))]
        public DateTime CreatedAt { get; } = DateTime.UtcNow.ToLocalTime();
        [JsonConverter(typeof(DateOnlyJsonConverter))]
        public DateTime UpdatedAt { get; set; } 
        public bool IsActive { get; set; }
        public List<Todos> Tasks { get; set; } = [];
    }
}