using System;
using API.Enums;

namespace API.DTOs;

public class TaskDto
{
    public required string TaskTitle { get; set; }
    public required string  TaskDescription { get; set; }
    public required DateOnly TaskDueDate { get; set; }
    public required Status TaskStatus { get; set; }
    public int ProjectId { get; set; }
}
