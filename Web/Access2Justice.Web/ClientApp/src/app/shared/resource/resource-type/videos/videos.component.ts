import { Component, OnInit, Input } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import { Global } from '../../../../global';
import { ShowMoreService } from '../../../sidebars/show-more/show-more.service';

@Component({
  selector: 'app-videos',
  templateUrl: './videos.component.html',
  styleUrls: ['./videos.component.css']
})
export class VideosComponent implements OnInit {
  @Input() resource;
  safeUrl: any;
  url: any;

  constructor(public sanitizer: DomSanitizer,
    private showMoreService: ShowMoreService,
    private global: Global) {
    this.sanitizer = sanitizer;
  }

  clickSeeMoreOrganizationsFromVideos(resourceType: string) {
    this.showMoreService.clickSeeMoreOrganizations(resourceType, this.global.activeSubtopicParam, this.global.topIntent);
  }

  resourceUrl(url) {
    this.url = url.replace("watch?v=", "embed/");
    this.safeUrl = this.sanitizer.bypassSecurityTrustResourceUrl(this.url);
  }

  ngOnInit() {
    this.resourceUrl(this.resource.url);
  }
}
