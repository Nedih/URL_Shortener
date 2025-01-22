import { Routes } from '@angular/router';
import { UrlTableComponent } from './components/url/url.component';

export const routes: Routes = [
  //{ path: '', redirectTo: '/urls', pathMatch: 'full' },
  {
    path: '',
    loadComponent: () => import('./components/url/url.component').then(m => m.UrlTableComponent),//component: UrlTableComponent,
    title: 'Urls'
  },
  {
    path: 'urls',
    loadComponent: () => import('./components/url/url.component').then(m => m.UrlTableComponent),//component: UrlTableComponent,
    title: 'Urls'
  },
  //{ path: 'about', component: AboutComponent },
  {
    path: 'about',
    loadComponent: () => import('./components/about/about.component').then(m => m.AboutComponent),
    title: 'About'
  },
  {
    path: 'login',
    loadComponent: () => import('./components/auth/login.component').then(m => m.LoginComponent),
    title: 'Login'
  },
  {
    path: 'register',
    loadComponent: () => import('./components/auth/register.component').then(m => m.RegisterComponent),
    title: 'Register'
  },
  {
    path: '**',
    loadComponent: () => import('./components/not-found/not-found.component').then(m => m.NotFoundComponent),
    title: 'Page Not Found'
  }
];
