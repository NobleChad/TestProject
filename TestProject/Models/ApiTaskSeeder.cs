namespace TestProject.Models
{
    public static class TaskDataGenerator
    {
        public static List<ApiTask> GenerateTasks()
        {
            var tasks = new List<ApiTask>();

            tasks.Add(new ApiTask { Id = 1, Title = "Task 1", Description = "Description 1", IsCompleted = false });
            tasks.Add(new ApiTask { Id = 2, Title = "Task 2", Description = "Description 2", IsCompleted = true });
            tasks.Add(new ApiTask { Id = 3, Title = "Task 3", Description = "Description 3", IsCompleted = false });
            return tasks;
        }
    }

}
