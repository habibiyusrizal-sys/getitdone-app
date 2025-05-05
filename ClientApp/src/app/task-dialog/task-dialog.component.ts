import { CommonModule, DatePipe } from '@angular/common';
import { Component, inject} from '@angular/core';
import {
  FormBuilder,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatNativeDateModule, provideNativeDateAdapter } from '@angular/material/core';
import { MatButtonModule } from '@angular/material/button';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatInputModule } from '@angular/material/input';
import { ActivatedRoute } from '@angular/router';
import { MatSelectModule } from '@angular/material/select';
import { ProjectTasksService } from '../_services/project-tasks.service';


interface Status {
 value: string;
 viewValue: string;
}

@Component({
  selector: 'app-task-dialog',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    FormsModule,
    MatDialogModule,
    MatInputModule,
    MatButtonModule,
    MatFormFieldModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatSelectModule,
  ],
  providers: [provideNativeDateAdapter(), DatePipe],
  templateUrl: './task-dialog.component.html',
  styleUrl: './task-dialog.component.css',
})
export class TaskDialogComponent {
  private dialogRef = inject(MatDialogRef<TaskDialogComponent>);
  private formBuilder = inject(FormBuilder);
  private projectTaskService = inject(ProjectTasksService);
  private datePipe = inject(DatePipe);
  private route = inject(ActivatedRoute);
  private data = inject(MAT_DIALOG_DATA);

  today: Date = new Date();
  selectedStatus: string = '';
  status: Status[] = [
    { value: 'ToDo', viewValue: 'ToDo' },
    { value: 'InProgress', viewValue: 'InProgress' },
    { value: 'Done', viewValue: 'Done' },
  ];

  taskForm = this.formBuilder.group({
    taskTitle: ['', Validators.required],
    taskDescription: [''],
    taskDueDate: [null, Validators.required],
    taskStatus: ['ToDo', Validators.required], // default to 'ToDo'
  });


  create(): void {
    if (this.taskForm.valid) {
      /**
       * { https://localhost:5001/api/task/createtask
            "taskTitle": "Test Repos Respo Test Test",
            "taskDescription": "",
            "taskDueDate": "2025-05-15",
            "taskStatus": "ToDo",
            "projectId": 2
          }
       */
      const dto: any = {
        taskTitle: this.taskForm.value.taskTitle,
        taskDescription: this.taskForm.value.taskDescription,
        taskDueDate: this.datePipe.transform(
          this.taskForm.value.taskDueDate,
          'yyyy-MM-dd'
        ),
        taskStatus: this.taskForm.value.taskStatus,
        projectId: this.data.projectId,
      };

      this.taskForm.markAllAsTouched();

      this.projectTaskService.createTask(dto).subscribe({
        next: (createdTask) => {
          this.dialogRef.close(createdTask);
        },
        error: (err) => console.error(err),
      });
      return;
    }
  }
}
