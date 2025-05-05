import { CommonModule } from '@angular/common';
import { Component,  OnInit, inject } from '@angular/core';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { ProjectTasksService } from '../_services/project-tasks.service';
import { MatDialog } from '@angular/material/dialog';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule} from '@angular/material/icon';

import { GroupedTasks, SimplifiedTask } from '../_models/project-tasks';
import { TaskDialogComponent } from '../task-dialog/task-dialog.component';

@Component({
  selector: 'app-project-tasks',
  standalone: true,
  imports: [FormsModule, CommonModule, MatButtonModule, MatIconModule, RouterModule],
  templateUrl: './project-tasks.component.html',
  styleUrl: './project-tasks.component.css',
})
export class ProjectTasksComponent implements OnInit {
  groupedTasks: GroupedTasks[] = [];
  projectTitle: string = '';
  projectId: number = 0;
  private route = inject(ActivatedRoute);
  private service = inject(ProjectTasksService);
  private dialog = inject(MatDialog);

  ngOnInit(): void {
    this.route.queryParams.subscribe((params) => {
      this.projectTitle = params['projectTitle'];
      this.projectId = params['projectId'];
    });

    this.projectId =
      Number(this.route.snapshot.paramMap.get('id')) ||
      Number(this.route.snapshot.queryParamMap.get('projectId'));

    this.reloadTasks();
  }

  reloadTasks(): void {
    const projectIdParam = this.route.snapshot.paramMap.get('id');
    const projectId = Number(projectIdParam);
    if (projectId) {
      this.service.tasks(projectId).subscribe({
        next: (response) => {
          this.groupedTasks = response.map((group) => ({
            taskStatus: group.taskStatus,
            tasks: group.tasks.map((tasks) => ({
              id: tasks.id,
              taskTitle: tasks.taskTitle,
              taskDescription: tasks.taskDescription,
              taskDueDate: tasks.taskDueDate,
            })) as SimplifiedTask[],
          }));
        },
        error: (err) => console.error(err),
      });
    }
  }

  updateTaskStatus(taskId: number, newStatus: string): void {
    const statusDto = { status: newStatus };
    this.service.updateTaskStatus(taskId, statusDto).subscribe({
      next: () => {
        this.reloadTasks();
      },
      error: (err) => console.error(err),
    });
  }

  createTaskDialog(): void {

    const dialogRef = this.dialog.open(TaskDialogComponent, {
      width: '350px',
      disableClose: false,
      ariaLabel: 'Create tasks Dialog',
      data: { projectId: this.projectId },
    });

    dialogRef.afterClosed().subscribe(() => {
      this.reloadTasks();
    });
  }

  isOverdue(dueDate: string | Date): boolean {
   const today = new Date();
   return new Date(dueDate) < new Date(today.toDateString());
  }
}
