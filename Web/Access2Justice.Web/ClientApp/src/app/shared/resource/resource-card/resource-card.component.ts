import { Component, OnInit, Input } from '@angular/core';
import { Resources } from '../../../profile/personalized-plan/personalized-plan';

@Component({
  selector: 'app-resource-card',
  templateUrl: './resource-card.component.html',
  styleUrls: ['./resource-card.component.css']
})
export class ResourceCardComponent implements OnInit {
  @Input() resource: any;
  @Input() searchResource: any;
  @Input() resources: Resources;
  sampleResource: any;
  constructor() { }

  ngOnInit() {
    if (this.searchResource != null || this.searchResource != undefined) {
      this.resource = this.searchResource;
    } else {
      this.resource = this.resource;
    }

    if (this.resources) {
      console.log(this.resources);
      this.sampleResource = { name: '', resourceType: '', url: '', Description:''};
      this.sampleResource.name = "Resource Name";
      this.sampleResource.resourceType = this.resources.resourceType;
      this.sampleResource.url = this.resources.url;
      this.sampleResource.Description = "Sample Description";
      this.resource = this.sampleResource;
    }
  }

}
