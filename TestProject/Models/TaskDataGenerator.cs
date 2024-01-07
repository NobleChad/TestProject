namespace TestProject.Models
{
    public static class TaskDataGenerator
    {
        public static List<ApiTask> GenerateTasks()
        {
            var tasks = new List<ApiTask>
            {
                new ApiTask { Id = 1, Title = "Task 1", Description = "Description 1", IsCompleted = false },
                new ApiTask { Id = 2, Title = "Task 2", Description = "Description 2", IsCompleted = true },
                new ApiTask { Id = 3, Title = "Task 3", Description = "Description 3", IsCompleted = false }
            };
            return tasks;
        }
    }

}
