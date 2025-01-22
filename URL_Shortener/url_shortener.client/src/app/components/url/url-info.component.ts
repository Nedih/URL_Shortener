import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-url-details',
  templateUrl: './url-details.component.html',
  styleUrls: ['./url-details.component.css']
})
export class UrlDetailsComponent implements OnInit {
  public selectedUrl: any; // You can replace `any` with your specific type

  constructor(private route: ActivatedRoute) {
    //const selectedUrl = this.route.snapshot.paramMap.get('url');
  }

  ngOnInit(): void {
    // Fetch the 'urlText' from the URL parameters and load the URL details
    const urlText = this.route.snapshot.paramMap.get('urlText');

    // Fetch the URL details from your API or local data
    // You can replace this with an actual HTTP request
    this.selectedUrl = { // Dummy data, replace with actual API data
      urlText: 'https://example.com',
      shortenUrl: 'https://short.ly/abc123',
      urlCreationDate: '2022-01-01',
      urlDescription: 'This is a shortened URL.',
      userEmail: 'user@example.com',
    };

    console.log(urlText); // For debugging purposes
  }
}
