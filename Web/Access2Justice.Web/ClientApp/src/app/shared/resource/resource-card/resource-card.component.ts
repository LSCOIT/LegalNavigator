import { Component, OnInit, Input } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';

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
  
  constructor(public sanitizer: DomSanitizer) {
    this.sanitizer = sanitizer;
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
  }
}
