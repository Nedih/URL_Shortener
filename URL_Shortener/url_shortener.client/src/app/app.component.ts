import { AsyncPipe, NgIf } from '@angular/common';
import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { HeaderComponent } from './components/header/header.component';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { LoadingService } from './services/loading-service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
  imports: [
    NgIf,
    RouterOutlet,
    HeaderComponent,
    MatProgressBarModule
  ]
})
export class AppComponent {
  title = 'URL Shortener';
  
  constructor(
    public loadingService: LoadingService,
    private cdr: ChangeDetectorRef
  ){}

  isLoading(): boolean{
    return this.loadingService.get();
  }
}
