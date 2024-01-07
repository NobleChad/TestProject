using Microsoft.AspNetCore.Mvc;
using TestProject.Models;

namespace TestProject.Controllers
{
    [Route("api")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private List<ApiTask> _tasks;

        public TasksController()
        {
            _tasks = TaskDataGenerator.GenerateTasks();
        }

        ///<summary> 
        ///API to get all tasks
        ///</summary>
        [HttpGet("GetTasks")]
        public ActionResult<IEnumerable<ApiTask>> GetTasks()
        {
            return _tasks;
        }

        ///<summary> 
        ///API to get task by id
        ///</summary>
        [HttpGet("GetTaskById/{id}")]
        public ActionResult<ApiTask> GetTaskById(int id)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == id);
            if (task == null)
            {
                return NotFound();
            }

            return task;
        }

        ///<summary> 
        ///API to create new task
        ///</summary>
        [HttpPost("CreateTask")]
        public ActionResult<ApiTask> CreateTask(ApiTask task)
        {
            task.Id = _tasks.Max(t => t.Id) + 1;
            _tasks.Add(task);
            return CreatedAtAction(nameof(GetTaskById), new { id = task.Id }, task);
        }

        ///<summary> 
        ///API to edit existing task
        ///</summary>
        [HttpPut("UpdateTask/{id}")]
        public ActionResult<ApiTask> UpdateTask(int id, [FromBody] ApiTask updatedTask)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == id);
            if (task == null)
            {
                return NotFound();
            }

            task.Title = updatedTask.Title;
            task.Description = updatedTask.Description;
            task.IsCompleted = updatedTask.IsCompleted;
            return task;
        }

        ///<summary> 
        ///API to delete task by id
        ///</summary>
        [HttpDelete("DeleteTask/{id}")]
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
