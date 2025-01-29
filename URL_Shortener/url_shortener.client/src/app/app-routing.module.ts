import { Routes } from '@angular/router';

export const routes: Routes = [
  //{ path: '', redirectTo: '/urls', pathMatch: 'full' },
  {
    path: '',
    loadComponent: () => import('./components/url/url-list/url-list.component').then(m => m.UrlListComponent),
    title: 'Urls'
  },
  {
    path: 'urls',
    loadComponent: () => import('./components/url/url-list/url-list.component').then(m => m.UrlListComponent),
    title: 'Urls'
  },
  {
    path: 'url-add',
    loadComponent: () => import('./components/url/url-create/url-create.component').then(m => m.UrlCreateComponent),
    title: 'Url Add'
  },
  {
    path: 'url-details/:shortenUrl',
    loadComponent: () => import('./components/url/url-info/url-info.component').then(m => m.UrlDetailsComponent),
    title: 'Url Details'
  },
  {
    path: 'about',
    loadComponent: () => import('./components/about/about.component').then(m => m.AboutComponent),
    title: 'About'
  },
  {
    path: 'login',
    loadComponent: () => import('./components/auth/login/login.component').then(m => m.LoginComponent),
    title: 'Login'
  },
  {
    path: 'register',
    loadComponent: () => import('./components/auth/register/register.component').then(m => m.RegisterComponent),
    title: 'Register'
  },
  {
    path: '**',
    loadComponent: () => import('./components/not-found/not-found.component').then(m => m.NotFoundComponent),
    title: 'Page Not Found'
  }
];
