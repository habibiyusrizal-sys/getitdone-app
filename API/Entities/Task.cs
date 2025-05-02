using System;
using API.Enums;

namespace API.Entities;

public class Task
{
    public int Id { get; set; }
    public required string TaskTitle { get; set; }
    public required string  TaskDescription { get; set; }
    public required DateOnly TaskDueDate { get; set;}
    public required Status TaskStatus { get; set; } = Status.ToDo;
    public int ProjectId { get; set;} // Foreign key for project Id
    public Project? Project { get; set; }
}
