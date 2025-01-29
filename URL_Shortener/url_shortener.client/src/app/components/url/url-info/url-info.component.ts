import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NgIf } from '@angular/common';
import { MatListModule } from '@angular/material/list';
import { Url, UrlService } from '../../../services/url.service';
import { LoadingService } from '../../../services/loading.service';

@Component({
  selector: 'app-url-details',
  templateUrl: './url-info.component.html',
  standalone: true,
  styleUrls: ['./url-info.component.scss'],
  imports: [MatListModule, NgIf]
})
export class UrlDetailsComponent implements OnInit {
  public selectedUrl: Url | undefined; 

  constructor(
    private route: ActivatedRoute,
    public urlService: UrlService,
    private router: Router,
    public loadingService: LoadingService,
    private changeDetectorRef: ChangeDetectorRef
  ) { }

  ngOnInit(): void {
    this.loadingService.show();
    this.changeDetectorRef.detectChanges();
    
    const shortenUrl = this.route.snapshot.paramMap.get('shortenUrl') || '';
    this.fetchUrl(shortenUrl);

    console.log(shortenUrl); 
  }

  fetchUrl(shortenUrl: string) {
    this.urlService.getUrl(shortenUrl).subscribe({
      next: (result) => {
        console.log('Fetched URL:', result);
        this.selectedUrl = result;
      },
      error: (error) => {
        console.error('Error fetching URL:', error);
      },
      complete: () => {
        console.log('URL fetch complete');
        this.loadingService.hide();
        this.changeDetectorRef.detectChanges();
      }
    });
  }

  goBack() {
    this.router.navigate(['/']);
  }

  isLoading(): boolean {
    return this.loadingService.get();
  }
}
