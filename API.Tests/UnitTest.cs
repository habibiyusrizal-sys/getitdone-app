using System.ComponentModel.DataAnnotations;
using API.DTOs;

namespace API.Tests;

public class UnitTest
{
    [Fact]
    public void Reject_Task_With_Past_DueDate()
    {
        var task = new TaskDto
        {
            TaskTitle = "Test Task",
            TaskDescription = "Task Description",
            TaskDueDate = DateOnly.FromDateTime(DateTime.Today.AddDays(-1)),
            TaskStatus = Enums.Status.ToDo,
            ProjectId = 1
        };

        var validationResults = new List<ValidationResult>();
        var context = new ValidationContext(task);

        var isValid = Validator.TryValidateObject(task, context, validationResults, true);

        //Assert
        Assert.False(isValid);
        Assert.Contains(validationResults, v => v.ErrorMessage?.Contains("Task Due date cannot be in the past") == true);
    }

    [Fact]
    public void Should_Require_Task_Title()
    {

    }
}