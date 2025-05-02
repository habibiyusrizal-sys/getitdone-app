using System;

namespace API.Entities;

public class Project
{
    public int Id { get; set; }
    public required string ProjectTitle { get; set; }
    public required string ProjectDesc { get; set; }
    public ICollection<Task> Tasks { get; set; } = new List<Task>();

}
