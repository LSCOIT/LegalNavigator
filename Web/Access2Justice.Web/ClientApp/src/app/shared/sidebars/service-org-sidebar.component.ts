import { Component, OnInit, Input } from '@angular/core';
import { ServiceOrgService } from '../sidebars/service-org.service';
import { Organization } from '../sidebars/organization';
import { MapService } from '../map/map.service';
import { OnDestroy } from '@angular/core/src/metadata/lifecycle_hooks';
import { MapLocation } from '../map/map';

@Component({
  selector: 'app-service-org-sidebar',
  templateUrl: './service-org-sidebar.component.html',
  styleUrls: ['./service-org-sidebar.component.css']
})
export class ServiceOrgSidebarComponent implements OnInit {
  @Input() fullPage = false;
  organizations: Organization;
  location: MapLocation;
  subscription: any;

  constructor(
    private serviceOrgService: ServiceOrgService,
    private mapService: MapService
  ) { }

  getOrganizations(location) {
    this.serviceOrgService.getOrganizationDetails(location)
      .subscribe(organizations => this.organizations = organizations);
  }

  ngOnInit() {
    if (sessionStorage.getItem("globalMapLocation")) {
      this.location = JSON.parse(sessionStorage.getItem("globalMapLocation"));
    }
    this.subscription = this.mapService.notifyLocation
      .subscribe((value) => {
        this.location = value;
        this.getOrganizations(this.location);
      });
    this.getOrganizations(this.location);
  }

  ngOnDestroy() {
    if (this.subscription != undefined) {
      this.subscription.unsubscribe();
    }
  }
}
