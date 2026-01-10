import { Routes } from '@angular/router';
import { Dashboard } from './features/dashboard/dashboard';
import { ProjectManagement } from './features/project-management/project-management';
import { MainLayout } from './shared/layout/main-layout/main-layout';
import { authGuard } from '@core/guards/auth.guard';

export const routes: Routes = [
  {
    path: 'login',
    loadComponent: () =>
      import('./shared/layout/login/login').then(m => m.Login),
  },
  {
    path:'register',
    loadComponent: () =>
      import('./shared/layout/register/register').then(m=> m.Register)
  },
  {
    path: '',
    component: MainLayout,
    canActivate: [authGuard],
    children: [
      { path: '', component: Dashboard },
      { path: 'project', component: ProjectManagement },
      { path: 'settings', component: ProjectManagement },
    ],
  },
];
