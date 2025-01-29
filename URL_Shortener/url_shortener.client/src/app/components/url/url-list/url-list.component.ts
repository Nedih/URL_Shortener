import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { AsyncPipe, NgFor, NgIf } from '@angular/common';
import { Router } from '@angular/router';
import { AuthService } from '../../../services/auth.service';
import { Url, UrlService } from '../../../services/url.service';
import { LoadingService } from '../../../services/loading.service';
import { ToastService } from '../../../services/toast.service';

@Component({
  selector: 'app-url-table',
  templateUrl: './url-list.component.html',
  standalone: true,
  imports: [NgFor, NgIf, AsyncPipe],
  styleUrls: ['./url-list.component.scss'],
})
export class UrlListComponent implements OnInit {
  public urls: Url[] = [];
  public selectedUrl: Url | null = null;

  constructor(
    public loadingService: LoadingService,
    private authService: AuthService,
    public urlService: UrlService,
    private router: Router,
    private changeDetectorRef: ChangeDetectorRef,
    private messageService: ToastService
  ) { }

  ngOnInit() {
    this.loadingService.show();
    this.changeDetectorRef.detectChanges();
    this.fetchUrls();
  }

  fetchUrls() {
    this.urlService.getUrls().subscribe({
      next: (result) => {
        console.log('Fetched URLs:', result);
        this.urls = result;
      },
      error: (error) => {
        console.error('Error fetching URLs:', error);
      },
      complete: () => {
        console.log('URL fetch complete');
        this.loadingService.hide();
        this.changeDetectorRef.detectChanges();
      }
    });
  }

  deleteUrl(shorten: string): void {
    this.urlService.deleteUrl(shorten).subscribe({
      next: () => {
        console.log('Delete URL');
        this.messageService.showSuccess("URL was deleted!");
      },
      error: (error) => {
        this.messageService.showError(error);
        console.error('Error deleting URLs:', error);
      },
      complete: () => {
        this.messageService.showSuccess('URL delete complete');
        this.fetchUrls();
      }
    });
  }

  addUrl() {
    this.router.navigate(['/url-add']);
  }

  isAdmin() {
    let res = this.authService.isAdmin();
    return res;
  }

  isLoggedIn(): boolean {
    return this.authService.isLoggedIn();
  }

  isUserOwner(email: string) {
    return this.authService.getEmail() === email;
  }

  viewUrlDetails(url: string): void {
    this.router.navigate(['/url-details', url]);
  }

  isLoading(): boolean {
    return this.loadingService.get();
  }

  title = 'url_shortener.client';
}
