import { Component, Input, OnInit } from '@angular/core';
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

  constructor(
    public sanitizer: DomSanitizer,
    private showMoreService: ShowMoreService,
    private global: Global
  ) {
    this.sanitizer = sanitizer;
  }

  clickSeeMoreOrganizationsFromVideos(resourceType: string) {
    this.showMoreService.clickSeeMoreOrganizations(
      resourceType,
      this.global.activeSubtopicParam,
      this.global.topIntent
    );
  }

  resourceUrl(videoUrl: string) {
    if (!videoUrl || !videoUrl.includes('youtube')) {
      return;
    }

    const parsedUrl = new URL(videoUrl);
    let embeddingUrl = `https://${parsedUrl.hostname}/embed/`;

    if (parsedUrl.searchParams.has('list')) {
      embeddingUrl += `videoseries?list=${parsedUrl.searchParams.get('list')}`;

      if (parsedUrl.searchParams.has('index')) {
        embeddingUrl += `&index=${parsedUrl.searchParams.get('index')}`;
      }
    } else {
      embeddingUrl += parsedUrl.searchParams.get('v');
    }

    this.url = embeddingUrl;
    this.safeUrl = this.sanitizer.bypassSecurityTrustResourceUrl(this.url);
  }

  ngOnInit() {
    this.resourceUrl(this.resource.url);
  }
}
