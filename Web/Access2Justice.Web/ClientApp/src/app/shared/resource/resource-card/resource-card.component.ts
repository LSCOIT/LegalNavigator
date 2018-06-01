import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-resource-card',
  templateUrl: './resource-card.component.html',
  styleUrls: ['./resource-card.component.css']
})
export class ResourceCardComponent implements OnInit {
  @Input() resource: any;
  @Input() searchResource: any;
  constructor() { }

  ngOnInit() {
    if (this.searchResource !== null || this.searchResource !== undefined) {
      this.resource = this.searchResource;
    } else {
      this.resource = this.resource;
    }
  }

}
