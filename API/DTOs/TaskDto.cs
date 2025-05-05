using System;
using System.ComponentModel.DataAnnotations;
using API.Enums;
namespace API.DTOs;

public class TaskDto : IValidatableObject
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Title is required.")]
    [MaxLength(100, ErrorMessage = "Title cannot exceed 100 character.")]
    public required string TaskTitle { get; set; }

    [MaxLength(500, ErrorMessage = "Description cannot exceed 500 character.")]
    public string? TaskDescription { get; set; }

    [Required(ErrorMessage = "Due Date is required.")]
    public required DateOnly TaskDueDate { get; set; }

    [Required(ErrorMessage = "Status is required.")]
    [EnumDataType(typeof(Status))]
    public required Status TaskStatus { get; set; }

    [Required(ErrorMessage = "Project Id is required")]
    public int ProjectId { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext valiationContext)
    {
        if (TaskDueDate < DateOnly.FromDateTime(DateTime.Today))
        {
            yield return new ValidationResult(
                "Task Due date cannot be in the past",
                new[] { nameof(TaskDueDate) }
            );
        }
    }
}
