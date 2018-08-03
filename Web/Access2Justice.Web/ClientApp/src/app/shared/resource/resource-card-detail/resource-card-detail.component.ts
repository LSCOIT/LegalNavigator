import { Component, OnInit } from '@angular/core';
import { ArrayUtilityService } from '../../array-utility.service';

@Component({
  selector: 'app-resource-card-detail',
  templateUrl: './resource-card-detail.component.html',
  styleUrls: ['./resource-card-detail.component.css']
})
export class ResourceCardDetailComponent implements OnInit {

  constructor(private arrayUtilityService: ArrayUtilityService) { }
  organizationResource: any;

  ngOnInit() {
    this.organizationResource = this.arrayUtilityService.resource;
  }

}
