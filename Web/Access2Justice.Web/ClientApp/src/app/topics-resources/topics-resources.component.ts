import { Component, OnInit } from '@angular/core';
import { ServiceOrgService } from '../shared/sidebars/service-org.service';

@Component({
  selector: 'app-topics-resources',
  templateUrl: './topics-resources.component.html',
  styleUrls: ['./topics-resources.component.css']
})
export class TopicsResourcesComponent implements OnInit {

  constructor(private serviceOrgService: ServiceOrgService) { }

  clickSeeMoreOrganizationsFromTopic(resourceType: string) {
    this.serviceOrgService.clickSeeMoreOrganizations(resourceType, undefined);
  }
  ngOnInit() {
  }

}
