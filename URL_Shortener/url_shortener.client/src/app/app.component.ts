import { Component } from '@angular/core';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { HeaderComponent } from './components/header/header.component';
import { LoginComponent } from './components/auth/login.component';
import { HttpClient } from '@angular/common/http';
import { UrlTableComponent } from './components/url/url.component';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
  imports: [RouterOutlet, RouterLink, RouterLinkActive, HeaderComponent, UrlTableComponent]
})
export class AppComponent{

  title = 'URL Shortener';

  constructor(private http: HttpClient) { }
}
