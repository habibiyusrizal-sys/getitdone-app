using System;
using API.Data;
using API.DTOs;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

public class ProjectController(DataContext context) : BaseApiController
{
    [HttpGet("getprojects")]
    public async Task<ActionResult<IEnumerable<ProjectDto>>> GetProjects()
    {
        var projects = await context.Projects
            .Select( p => new ProjectDto
            {
                Id = p.Id,
                ProjectTitle = p.ProjectTitle,
                ProjectDesc = p.ProjectDesc
            }).ToListAsync();

        return projects;
    }

    [HttpPost("createproject")]
    public async Task<ActionResult<Project>> CreateProject(ProjectDto projDto)
    {
        if ( await ProjectExist(projDto.ProjectTitle)) return BadRequest ("Project Title is taken");

        var project = new Project
        {
            ProjectTitle = projDto.ProjectTitle,
            ProjectDesc = projDto.ProjectDesc
        };

        context.Add(project);
        await context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetProject), new { id = project.Id }, project);
    }

    [HttpGet("getproject/{id:int}")]
    public async Task<ActionResult<Project>> GetProject(int id)
    {
        var project = await context.Projects.FindAsync(id);

        if (project == null) return NotFound();

        return Ok(project);
    }




    private async Task<bool> ProjectExist(string projectTitle)
    {
        var project = await context.Projects.AnyAsync(p => p.ProjectTitle.ToLower() == projectTitle.ToLower());

        return project;
    }
}
