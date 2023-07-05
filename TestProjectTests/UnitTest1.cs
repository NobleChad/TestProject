using System.Collections.Generic;
using TestProject.Models;

namespace TestProjectTests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            // Arrange
            List<ApiTask> x;
            List<ApiTask> y = new List<ApiTask>();

            // Act
            x = TaskDataGenerator.GenerateTasks();
            y.Add(new ApiTask { Id = 1, Title = "Task 1", Description = "Description 1", IsCompleted = false });
            y.Add(new ApiTask { Id = 2, Title = "Task 2", Description = "Description 2", IsCompleted = true });
            y.Add(new ApiTask { Id = 3, Title = "Task 3", Description = "Description 3", IsCompleted = false });

            // Assert
            Assert.Equal(x, y, new ApiTaskEqualityComparer());

        }
        [Theory]
        [InlineData("lul", true)]
        [InlineData("lul1", false)]
        public void lulsich(string name, bool expected)
        {
            //Arrange
            var _name = name;
            var _expected = expected;
            bool gigaexpected = false;

            //Act
            if (_name == "lul") { gigaexpected = true; };
            //Assert
            Assert.Equal(_expected, gigaexpected);

        }
        
    }
}