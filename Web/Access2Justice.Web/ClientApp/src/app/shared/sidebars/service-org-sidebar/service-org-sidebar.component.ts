import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { MapService } from '../../map/map.service';
import { IResourceFilter } from '../../search/search-results/search-results.model';
import { NavigateDataService } from '../../services/navigate-data.service';
import { PaginationService } from '../../pagination/pagination.service';
import { MapLocation, LocationDetails } from '../../map/map';
import { Global } from '../../../global';

@Component({
  selector: 'app-service-org-sidebar',
  templateUrl: './service-org-sidebar.component.html',
  styleUrls: ['./service-org-sidebar.component.css']
})
export class ServiceOrgSidebarComponent implements OnInit {
  @Input() fullPage = false;
  organizations: any;
  location: MapLocation;
  locationDetails: LocationDetails;
  subscription: any;
  activeTopic: string;
  @Output()
  showMoreOrganizations = new EventEmitter<string>();
  resourceFilter:
    IResourceFilter = {
      ResourceType: '',
      ContinuationToken: '',
      TopicIds: [],
      ResourceIds: [],
      PageNumber: 0,
      Location: {},
      IsResourceCountRequired: false,
      IsOrder: false,
      OrderByField: '',
      OrderBy: ''
    };
  topicIds: string[] = [];
  total: number = 5;

  constructor(
    private router: Router,
    private activeRoute: ActivatedRoute,
    private mapService: MapService,
    private navigateDataService: NavigateDataService,
    private paginationService: PaginationService,
    private global: Global
  ) { }

  getOrganizations() {
    if (this.router.url.startsWith("/topics") || this.router.url.startsWith("/subtopics")) {
      this.activeTopic = this.activeRoute.snapshot.params['topic'];
      if (this.activeTopic) {
        this.topicIds.push(this.activeTopic);
      }
    }
    if (this.router.url.startsWith("/search")) {
      let searchResponse = this.navigateDataService.getData();
      this.topicIds = searchResponse["topicIds"];
    }
    this.resourceFilter = {
      ResourceType: 'Organizations', TopicIds: this.topicIds, Location: this.location,
      PageNumber: 0, ContinuationToken: '', IsResourceCountRequired: false, ResourceIds: [], IsOrder: false, OrderByField: '', OrderBy:''
    }
    this.paginationService.getPagedResources(this.resourceFilter).subscribe(response => {
      if (response != undefined) {
        this.organizations = response["resources"];
        this.global.organizationsData = response["resources"];

        if (this.global.organizationsData.length > 3) {
          this.global.organizationsData.splice(3, (this.global.organizationsData.length) - 1);
        }
      }
    });
  }

  callOrganizations(event) {
    event.preventDefault();
    this.showMoreOrganizations.emit("Organizations");
  }

  ngOnInit() {
    if (!this.global.organizationsData || this.global.organizationsData.length === 0) {
      if (sessionStorage.getItem("globalMapLocation")) {
        this.locationDetails = JSON.parse(sessionStorage.getItem("globalMapLocation"));
        this.location = this.locationDetails.location;
        this.getOrganizations();
      }
    } else {
      this.organizations = this.global.organizationsData;
    }

    this.subscription = this.mapService.notifyLocation
      .subscribe((value) => {
        this.global.organizationsData = null;
        this.locationDetails = value;
        this.location = this.locationDetails.location;
        this.getOrganizations();
      });
  }

  ngOnDestroy() {
    if (this.subscription != undefined) {
      this.subscription.unsubscribe();
    }
  }
}
