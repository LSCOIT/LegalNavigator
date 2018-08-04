import { Component, OnInit, Input } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';

@Component({
  selector: 'app-videos',
  templateUrl: './videos.component.html',
  styleUrls: ['./videos.component.css']
})
export class VideosComponent implements OnInit {
  @Input() resource;
  url: any;

  constructor(public sanitizer: DomSanitizer) {
    this.sanitizer = sanitizer;
  }

  resourceUrl() {
    this.url = this.sanitizer.bypassSecurityTrustResourceUrl(this.resource.url);
    return this.url;
  }

  ngOnInit() {
  }

}
