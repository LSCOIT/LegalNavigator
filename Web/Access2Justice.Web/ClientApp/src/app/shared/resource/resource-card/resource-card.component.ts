import { Component, OnInit, Input } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import { Global, UserStatus } from '../../../global';

@Component({
  selector: 'app-resource-card',
  templateUrl: './resource-card.component.html',
  styleUrls: ['./resource-card.component.css']
})
export class ResourceCardComponent implements OnInit {
  @Input() personalizedResources;
  @Input() resource: any;
  @Input() searchResource: any;
  @Input() isSearchResults: boolean;
  @Input() showRemoveOption: boolean;
  url: any;
  urlOrigin: string;
  applicationUrl: any = window.location.origin;
  resourceTypeList = [
    'Articles',
    'Essential Readings',
    'Forms',
    'Guided Assistant',
    'Organizations',
    'Related Links',
    'Topics',
    'Videos',
    'WebResources'
    ];

  constructor(
    public sanitizer: DomSanitizer,
    private global: Global
  ) {
    this.sanitizer = sanitizer;
    if (global.role === UserStatus.Shared && location.pathname.indexOf(global.shareRouteUrl) >= 0) {
      global.showShare = false;
      this.showRemoveOption = false;
      global.showDropDown = false;
    }
    else {
      global.showShare = true;
      this.showRemoveOption = true;
      global.showDropDown = true;
    }
  }

  resourceUrl() {
    this.url = this.sanitizer.bypassSecurityTrustResourceUrl(this.resource.url);
    return this.url;
  }

  ngOnInit() {
    if (this.searchResource != null || this.searchResource != undefined) {
      this.resource = this.searchResource;
    } else {
      this.resource = this.resource;
    }
    if (this.resource.itemId) {
      this.resource.id = this.resource.itemId;
    }
    try {
      this.urlOrigin = new URL(this.resource.url).origin;
    } catch (e) {
      this.urlOrigin = this.resource.url;
    }    
  }
}
