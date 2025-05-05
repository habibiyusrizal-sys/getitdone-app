import { Component, inject } from '@angular/core';
import { FormBuilder, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { ProjectTasksService } from '../_services/project-tasks.service';
import { Project } from '../_models/project';

@Component({
  selector: 'app-project-dialog',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    FormsModule,
    MatDialogModule,
    MatInputModule,
    MatButtonModule,
  ],
  templateUrl: './project-dialog.component.html',
  styleUrl: './project-dialog.component.css',
})
export class ProjectDialogComponent {
  private dialogRef = inject(MatDialogRef<ProjectDialogComponent>);
  private formBuilder = inject(FormBuilder);
  private projectTaskService = inject(ProjectTasksService);

  projectForm = this.formBuilder.group({
    projectTitle: ['', Validators.required],
    projectDesc: [''],
  });



  create(): void {
    if (this.projectForm.valid) {
     const dto: any = {
       projectTitle: this.projectForm.value.projectTitle,
       projectDesc: this.projectForm.value.projectDesc,
     };

      this.projectForm.markAllAsTouched();
      this.projectTaskService.createProject(dto).subscribe({
        next: createdProject => {
          this.dialogRef.close(createdProject)
        },
        error: err => console.error(err)
      });
      return;
    }
  }
}
