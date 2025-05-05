import { HttpClient, HttpHeaders } from '@angular/common/http';
import { inject,Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Project } from '../_models/project';
import { Task } from '../_models/task';
import { GroupedTasks } from '../_models/project-tasks';

@Injectable({
  providedIn: 'root'
})
export class ProjectTasksService {

  private http = inject(HttpClient);
  baseUrl = 'https://localhost:5001/api/';

 // fetch project list from projects
  projects(): Observable<Project[]> {
    return this.http.get<Project[]>(this.baseUrl + 'project/getprojects');
  }

  //
  createProject(project: Project): Observable<Project> {
    return this.http.post<Project>(
      this.baseUrl + 'project/createproject',
      project,
      {
        headers: new HttpHeaders({ 'Content-Type': 'application/json' }),
      }
    );
  }

  tasks(projectId: number): Observable<GroupedTasks[]> {
    return this.http.get<GroupedTasks[]>(
      `${this.baseUrl}task/${projectId}`
    );
  }

  createTask(task: Task): Observable<Task> {
    return this.http.post<Task>(
      `${this.baseUrl}task/createtask`,
      task,
      {
        headers: new HttpHeaders({ 'Content-Type': 'application/json' })
      }
    )
  }

  updateTaskStatus(taskId: number, statusDto: { status: string}): Observable<void> {
    return this.http.patch<void>(`${this.baseUrl}task/${taskId}/status`, statusDto);
  }
}
