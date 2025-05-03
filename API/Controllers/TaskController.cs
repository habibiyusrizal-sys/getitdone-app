using API.Data;
using API.DTOs;
using API.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            TaskTitle = task.TaskTitle,
            TaskDescription = task.TaskDescription,
            TaskDueDate = task.TaskDueDate,
            TaskStatus = task.TaskStatus,
            ProjectId = task.ProjectId
        });
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
                    ProjectId = t.ProjectId
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
                        ProjectId = t.ProjectId
                    }).ToListAsync();

        return Ok(overdueTasks);
    }

    [HttpPost("createtask")]
    public async Task<ActionResult<TaskDto>> CreateTask([FromBody] TaskDto taskDto)
    {
        var project = await context.Projects.FindAsync(taskDto.ProjectId);

        if (project == null)
        {
            return NotFound($"Project not found");
        }

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

        // Sending a 201 Created status code
        // Include location header
        return CreatedAtAction(
            nameof(GetTask),
            new { id = task.Id},
            new TaskDto
            {
                TaskTitle = task.TaskTitle,
                TaskDescription = task.TaskDescription,
                TaskDueDate = task.TaskDueDate,
                TaskStatus = task.TaskStatus,
                ProjectId = task.ProjectId
            });
    }



}
