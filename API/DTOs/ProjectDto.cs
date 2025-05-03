using System;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs;

public class ProjectDto
{
    [Required(ErrorMessage = "Project title is required.")]
    [MaxLength(100, ErrorMessage = "Title cannot exceed 100 character")]
    public required string ProjectTitle { get; set; }

    [Required(ErrorMessage = "Project description is required.")]
    [MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
    public required  string  ProjectDesc { get; set; }
}
