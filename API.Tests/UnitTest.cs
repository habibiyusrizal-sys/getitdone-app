using System.ComponentModel.DataAnnotations;
using API.Controllers;
using API.Data;
using API.Data.Migrations;
using API.DTOs;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
    public void TaskTitle_IsRequired_ShouldFailValidation()
    {
        // Arrange
        var dto = new TaskDto
        {
            TaskTitle = null,  // Required field missing
            TaskDescription = "Optional description",
            TaskDueDate = DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
            TaskStatus = Enums.Status.ToDo,
            ProjectId = 1
        };

        var validationResults = new List<ValidationResult>();
        var context = new ValidationContext(dto, null, null);

        // Act
        bool isValid = Validator.TryValidateObject(dto, context, validationResults, true);

        // Assert
        Assert.False(isValid);
        Assert.Contains(validationResults, v => v.MemberNames.Contains("TaskTitle"));
    }

    [Fact]
    public void Filter_Task_By_Status()
    {
        var tasks = new List<Entities.Task>
        {
            new Entities.Task { Id = 1, TaskTitle = "Task A", TaskDescription = "Desc A", TaskDueDate = DateOnly.FromDateTime(DateTime.Today), TaskStatus = Enums.Status.ToDo, ProjectId = 1 },
            new Entities.Task { Id = 2, TaskTitle = "Task B", TaskDescription = "Desc B", TaskDueDate = DateOnly.FromDateTime(DateTime.Today.AddDays(1)), TaskStatus = Enums.Status.InProgress, ProjectId = 1 },
            new Entities.Task { Id = 3, TaskTitle = "Task C", TaskDescription = "Desc C", TaskDueDate = DateOnly.FromDateTime(DateTime.Today.AddDays(2)), TaskStatus = Enums.Status.ToDo, ProjectId = 2 },
            new Entities.Task { Id = 4, TaskTitle = "Task D", TaskDescription = "Desc D", TaskDueDate = DateOnly.FromDateTime(DateTime.Today.AddDays(3)), TaskStatus = Enums.Status.Done, ProjectId = 2 },
        };

        var todoTasks = tasks.Where(t => t.TaskStatus == Enums.Status.ToDo).ToList();

        //Assert
        Assert.Equal(2, todoTasks.Count);
        Assert.All(todoTasks, t => Assert.Equal(Enums.Status.ToDo, t.TaskStatus));
    }

    [Fact]
    public void Identify_Overdue_Tasks()
    {
        var today = DateOnly.FromDateTime(DateTime.Today);

        var tasks = new List<Entities.Task>
        {
            new Entities.Task { Id = 1, TaskTitle = "Overdue Task", TaskDescription = "Desc A", TaskDueDate = DateOnly.FromDateTime(DateTime.Today.AddDays(-1)), TaskStatus = Enums.Status.ToDo, ProjectId = 1 },
            new Entities.Task { Id = 2, TaskTitle = "Task B", TaskDescription = "Desc B", TaskDueDate = DateOnly.FromDateTime(DateTime.Today.AddDays(1)), TaskStatus = Enums.Status.InProgress, ProjectId = 1 },
            new Entities.Task { Id = 3, TaskTitle = "Task C", TaskDescription = "Desc C", TaskDueDate = DateOnly.FromDateTime(DateTime.Today.AddDays(2)), TaskStatus = Enums.Status.ToDo, ProjectId = 2 },
            new Entities.Task { Id = 4, TaskTitle = "Task D", TaskDescription = "Desc D", TaskDueDate = DateOnly.FromDateTime(DateTime.Today.AddDays(3)), TaskStatus = Enums.Status.Done, ProjectId = 2 },
        };

        var overdueTasks = tasks.Where(t => t.TaskDueDate < today);
        var overdueTasksList = overdueTasks.ToList();
        Assert.Single(overdueTasks);
        Assert.Equal("Overdue Task", overdueTasksList[0].TaskTitle);

    }

    [Fact]
    public async System.Threading.Tasks.Task GetTaskById_ReturnsNotFound_WhenTaskDoesnNotExist()
    {
        var options = new DbContextOptionsBuilder<DataContext>()
        .UseInMemoryDatabase(databaseName: "TestDB_GetTaskById_NotFound")
        .Options;


        using var context = new DataContext(options);
        var controller = new TaskController(context);

        // Act
        var result = await controller.GetTask(999); // Non-existent ID

        // Assert
        var notFoundResult = Assert.IsType<NotFoundResult>(result.Result);
        Assert.Equal(404, notFoundResult.StatusCode);
    }
}