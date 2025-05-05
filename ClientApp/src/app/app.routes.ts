import { Routes } from '@angular/router';
import { ProjectTasksComponent } from './project-tasks/project-tasks.component';
import { HomeComponent } from './home/home.component';

export const routes: Routes = [
    { path: '', component: HomeComponent},
    { path: 'tasks/:id', component: ProjectTasksComponent}
];
