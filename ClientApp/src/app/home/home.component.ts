import { Component, inject, OnInit } from '@angular/core';
import { ProjectTasksService } from '../_services/project-tasks.service';
import { Project } from '../_models/project';
import { Router} from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { ProjectDialogComponent } from '../project-dialog/project-dialog.component';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [FormsModule, CommonModule, BsDropdownModule],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent implements OnInit{
  private dialog = inject(MatDialog);
  private projectTaskService = inject(ProjectTasksService);
  private router = inject(Router);
  projects: Project [] = []

  ngOnInit(): void {
    this.projectList();
  }

  projectList(): void {
    this.projectTaskService.projects().subscribe({
      next: response => {
        this.projects = response
        //console.log(this.projects);
      },
      error: error => console.error(error)
    });

  }

  createProjectDialog(button: HTMLButtonElement): void {
   button.blur();
   const dialogRef = this.dialog.open(ProjectDialogComponent,{
    width: '350px',
    disableClose: false,
    ariaLabel: 'Create project Dialog'
   });

   dialogRef.afterClosed().subscribe(() => {
    this.projectList();
   });
  }

  openProject(project: Project): void {
    this.router.navigate([`/tasks/${project.id}`], {
      queryParams: {projectTitle: project.projectTitle, projectId: project.id}
    });
  }

}
