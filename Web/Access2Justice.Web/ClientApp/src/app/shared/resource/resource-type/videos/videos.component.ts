import { Component, OnInit, Input } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';

@Component({
  selector: 'app-videos',
  templateUrl: './videos.component.html',
  styleUrls: ['./videos.component.css']
})
export class VideosComponent implements OnInit {
  @Input() resource;
  safeUrl: any;
  url: any;

  constructor(public sanitizer: DomSanitizer) {
    this.sanitizer = sanitizer;
  }

  resourceUrl(url) {
    this.url = url.replace("watch?v=", "embed/");
    this.safeUrl = this.sanitizer.bypassSecurityTrustResourceUrl(this.url);
  }

  ngOnInit() {
    this.resourceUrl(this.resource.url);
  }
}
