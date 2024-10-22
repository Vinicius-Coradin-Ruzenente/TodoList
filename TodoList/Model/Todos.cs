using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using TodoList.Helpers;

namespace TodoList.Model
{
    public class Todos
    {
        public int Id { get; set; }
        public required string Task { get; set; }
        public bool IsDone { get; set; } = false;
        [JsonConverter(typeof(DateOnlyJsonConverter))]
        public DateTime CreatedAt { get; } = DateTime.UtcNow.ToLocalTime();
        [JsonConverter(typeof(DateOnlyJsonConverter))]
        public DateTime DoneAt { get; set; }
        public required int UserId { get; set; }
    }
}
