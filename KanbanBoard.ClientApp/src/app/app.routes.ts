import { Routes } from '@angular/router';
import { Dashboard } from './features/dashboard/dashboard';
import { ProjectManagement } from './features/project-management/project-management';
import { Board } from './features/project-management/board/board';

export interface RouteInfo {
  path: string;
  title: string;
  icon?: string;
}

export const routes: Routes = [
  { path: '', component: Dashboard, data: { title: 'Dashboard', icon: 'board' } },
  { path: 'project', component: ProjectManagement, data: { title: 'Project Management', icon: 'project' } },
  { path: 'settings', component: ProjectManagement, data: { title: 'Settings', icon: 'settings' } },
  { path: '**', redirectTo: '/' }
];