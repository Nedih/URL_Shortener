import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { UrlService } from './url.service';

@Component({
  selector: 'app-url-info',
  templateUrl: './url-info.component.html',
  styleUrls: ['./url-info.component.css']
})
export class UrlInfoComponent implements OnInit {
  url: any;

  constructor(private route: ActivatedRoute, private urlService: UrlService) { }

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      //this.urlService.getUrlInfo(+id).subscribe((data) => {
        //this.url = data;
      //});
    } else {
      console.error('URL ID is missing');
    }
  }
}
