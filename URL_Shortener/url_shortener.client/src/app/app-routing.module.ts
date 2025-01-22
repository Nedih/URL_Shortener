import { Routes } from '@angular/router';
import { UrlTableComponent } from './components/url/url.component';

export const routes: Routes = [
  //{ path: '', redirectTo: '/urls', pathMatch: 'full' },
  {
    path: '',
    loadComponent: () => import('./components/url/url.component').then(m => m.UrlTableComponent),
    title: 'Urls'
  },
  {
    path: 'urls',
    loadComponent: () => import('./components/url/url.component').then(m => m.UrlTableComponent),
    title: 'Urls'
  },
  //{ path: 'about', component: AboutComponent },/url-details
  {
    path: 'url-add',
    loadComponent: () => import('./components/url/url-add.component').then(m => m.UrlCreateComponent),
    title: 'Url Add'
  },
  {
    path: 'url-details/:shortenUrl',
    loadComponent: () => import('./components/url/url-info.component').then(m => m.UrlDetailsComponent),
    title: 'Url Details'
  },
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
