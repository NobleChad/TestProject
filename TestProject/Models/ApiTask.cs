using System.Text.Json.Serialization;

namespace TestProject.Models
{
    public class ApiTask
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
    }
}
