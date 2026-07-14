import { Routes } from '@angular/router';

// We import the short class names: Login, AppRegistry, UserManagement
import { Login } from './login/login';
import { AppRegistry } from './app-registry/app-registry';
import { UserManagement } from './user-management/user-management';

export const routes: Routes = [
  { path: '', redirectTo: 'login', pathMatch: 'full' },
  { path: 'login', component: Login },
  { path: 'app-registry', component: AppRegistry },
  { path: 'user-management', component: UserManagement }
];
