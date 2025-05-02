using System.ComponentModel.DataAnnotations;

namespace API.Enums;

public enum Status
{
    [Display(Name = "To Do")]
    ToDo,

    [Display(Name = "In Progress")]
    InProgress,

    [Display(Name = "Done")]
    Done
}
