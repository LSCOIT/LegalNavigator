import { Component, OnInit,Input } from '@angular/core';
import { ServiceOrgService } from '../sidebars/service-org.service';
import { Organization } from '../sidebars/organization';
import { LocationService } from '../location/location.service';
import { OnDestroy } from '@angular/core/src/metadata/lifecycle_hooks';
import { MapLocation } from '../location/location';

@Component({
  selector: 'app-service-org-sidebar',
  templateUrl: './service-org-sidebar.component.html',
  styleUrls: ['./service-org-sidebar.component.css']
})
export class ServiceOrgSidebarComponent implements OnInit
{
  @Input() fullPage;
  organizations: Organization;
  location: MapLocation;
  subscription: any;
  constructor(private serviceOrgService: ServiceOrgService, private locationService: LocationService)
  {
  }
  getOrganizations(location)
  {
    this.serviceOrgService.getOrganizationDetails(location)
      .subscribe(organizations =>
        this.organizations = organizations); 
  }
  ngOnInit()
  {
    if (sessionStorage.getItem("globalMapLocation"))
    {
      this.location = JSON.parse(sessionStorage.getItem("globalMapLocation"));
    }
    this.subscription = this.locationService.notifyLocation.subscribe((value) => {
    this.location = value;  
    this.getOrganizations(this.location);
    });

    this.getOrganizations(this.location);
  }
  ngOnDestroy()
  {
    if (this.subscription != undefined)
    {
      this.subscription.unsubscribe();
    }
  }
}
