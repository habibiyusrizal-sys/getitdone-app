using System;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class DataContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Project> Projects { get; set; }
    public DbSet<Entities.Task> Tasks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Project>()
            .HasMany(t => t.Tasks)
            .WithOne(p => p.Project)
            .HasForeignKey(t => t.ProjectId)
            .IsRequired();
    }
}
