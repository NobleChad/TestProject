using Microsoft.AspNetCore.Mvc;
using TestProject.Models;

namespace TestProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private List<ApiTask> _tasks;

        public TasksController()
        {
            _tasks = TaskDataGenerator.GenerateTasks();
        }
        
        // GET api/tasks
        [HttpGet]
        public ActionResult<IEnumerable<ApiTask>> GetTasks()
        {
            return _tasks;
        }

        // GET api/tasks/1
        [HttpGet("{id}")]
        public ActionResult<ApiTask> GetTaskById(int id)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == id);
            if (task == null)
            {
                return NotFound();
            }

            return task;
        }

        // POST api/tasks
        [HttpPost]
        public ActionResult<ApiTask> CreateTask([FromBody] ApiTask task)
        {
            task.Id = _tasks.Max(t => t.Id) + 1;
            _tasks.Add(task);
            return CreatedAtAction(nameof(GetTaskById), new { id = task.Id }, task);
        }

        // PUT api/tasks/1
        [HttpPut("{id}")]
        public IActionResult UpdateTask(int id,[FromBody] ApiTask updatedTask)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == id);
            if (task == null)
            {
                return NotFound();
            }

            task.Title = updatedTask.Title;
            task.Description = updatedTask.Description;
            task.IsCompleted = updatedTask.IsCompleted;
            return NoContent();
        }

        // DELETE api/tasks/1
        [HttpDelete("{id}")]
        public IActionResult DeleteTask(int id)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == id);
            if (task == null)
            {
                return NotFound();
            }

            _tasks.Remove(task);
            return NoContent();
        }
    }

}
