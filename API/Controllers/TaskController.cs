using API.Data;
using API.DTOs;
using API.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace API.Controllers;

public class TaskController(DataContext context) : BaseApiController
{

    [HttpGet("gettask/{id:int}")]
    public async Task<ActionResult<TaskDto>> GetTask(int id)
    {
        var task = await context.Tasks.FindAsync(id);

        if (task == null) return NotFound();

        return Ok( new TaskDto
        {
            Id = task.Id,
            TaskTitle = task.TaskTitle,
            TaskDescription = task.TaskDescription,
            TaskDueDate = task.TaskDueDate,
            TaskStatus = task.TaskStatus,
        });
    }

    [HttpGet("{projectId:int}")]
    public async Task<ActionResult<IEnumerable<TaskDto>>> GetTasksByProject(int projectId)
    {
        var projectExist = await context.Projects.AnyAsync(p => p.Id == projectId);
        if (!projectExist) return BadRequest("Project not found");

        var groupedTasks = await context.Tasks
                .Where(t => t.ProjectId == projectId)
                .GroupBy(t => t.TaskStatus)
                .Select(g => new
                {
                    TaskStatus = g.Key,
                    Tasks = g.Select(t => new TaskDto
                    {
                        Id = t.Id,
                        TaskTitle = t.TaskTitle,
                        TaskDescription = t.TaskDescription,
                        TaskDueDate = t.TaskDueDate,
                        TaskStatus = t.TaskStatus,

                    }).ToList()
                })
                .ToListAsync();

        return Ok(groupedTasks);
    }

    [HttpGet("{projectId:int}/taskstatus")]
    public async Task<ActionResult<IEnumerable<TaskDto>>> GetTaskByStatus(int projectId, [FromQuery] Status status)
    {
        var projectExist = await context.Projects.AnyAsync(p => p.Id == projectId);

        if (!projectExist) return BadRequest("Project not found");

        var tasks = await context.Tasks
                .Where( t => t.ProjectId == projectId && t.TaskStatus == status)
                .Select( t => new TaskDto
                {
                    TaskTitle = t.TaskTitle,
                    TaskDescription = t.TaskDescription,
                    TaskDueDate = t.TaskDueDate,
                    TaskStatus = t.TaskStatus,

                } )
                .ToListAsync();

        return Ok(tasks);
    }

    [HttpGet("overduetasks")]
    public async Task<ActionResult<IEnumerable<TaskDto>>> GetOverdueTasks()
    {
        var today = DateOnly.FromDateTime(DateTime.Today);
        Console.WriteLine(today);

        var overdueTasks = await context.Tasks
                    .Where(t => t.TaskDueDate < today && t.TaskStatus != Status.Done)
                    .Select(t => new TaskDto
                    {
                        TaskTitle = t.TaskTitle,
                        TaskDescription = t.TaskDescription,
                        TaskDueDate = t.TaskDueDate,
                        TaskStatus = t.TaskStatus,
                    }).ToListAsync();

        return Ok(overdueTasks);
    }

    [HttpPost("createtask")]
    public async Task<ActionResult> CreateTask([FromBody] TaskDto taskDto)
    {

        var task =  new Entities.Task
        {
            TaskTitle = taskDto.TaskTitle,
            TaskDescription = taskDto.TaskDescription ?? string.Empty,
            TaskDueDate = taskDto.TaskDueDate,
            TaskStatus = taskDto.TaskStatus,
            ProjectId = taskDto.ProjectId
        };

        context.Tasks.Add(task);
        await context.SaveChangesAsync();

        return Created($"/api/task/{task.Id}", null);
    }

    [HttpPatch("{taskId:int}/status")]
    public async Task<IActionResult> UpdateTaskStatus(int taskId, [FromBody] TaskStatusDto statusDto)
    {
        Console.WriteLine(taskId);
        var task = await context.Tasks.FindAsync(taskId);
        if (task == null) return NotFound();

        Status status = (Status)Enum.Parse(typeof(Status), statusDto.Status);
        task.TaskStatus = status;
        await context.SaveChangesAsync();

        return NoContent();
    }



}
