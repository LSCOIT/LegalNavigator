import { Component, OnInit,Input } from '@angular/core';
import { ServiceOrgService } from '../sidebars/service-org.service';
import { Organization } from '../sidebars/organization';
import { LocationService } from '../location/location.service';


@Component({
  selector: 'app-service-org-sidebar',
  templateUrl: './service-org-sidebar.component.html',
  styleUrls: ['./service-org-sidebar.component.css']
})
export class ServiceOrgSidebarComponent implements OnInit {

  organizations: any[];
  
  constructor(private serviceOrgService: ServiceOrgService, private locationService: LocationService) { }

  getOrganizationDetail(value) {
    this.serviceOrgService.getOrganizationDetail(value)
      .subscribe(organizations =>
        this.organizations = organizations
      );
  }

  ngOnInit() {
    var mapLocation = localStorage.getItem("GetCM");
    console.log("test1");
    this.locationService.notifyLocation.subscribe(this.getOrganizationDetail('Hawaii'));
  }

}
