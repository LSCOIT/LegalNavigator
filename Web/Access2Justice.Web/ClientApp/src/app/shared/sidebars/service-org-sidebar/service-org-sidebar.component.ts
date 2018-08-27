import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { MapService } from '../../map/map.service';
import { IResourceFilter } from '../../search/search-results/search-results.model';
import { NavigateDataService } from '../../navigate-data.service';
import { PaginationService } from '../../pagination/pagination.service';
import { MapLocation } from '../../map/map';

@Component({
  selector: 'app-service-org-sidebar',
  templateUrl: './service-org-sidebar.component.html',
  styleUrls: ['./service-org-sidebar.component.css']
})
export class ServiceOrgSidebarComponent implements OnInit {
  @Input() fullPage = false;
  organizations: any;
  location: MapLocation;
  subscription: any;
  activeTopic: string;
  @Output()
  showMoreOrganizations = new EventEmitter<string>();
  resourceFilter: IResourceFilter = { ResourceType: '', ContinuationToken: '', TopicIds: [], ResourceIds: [], PageNumber: 0, Location: {}, IsResourceCountRequired: false };
  topicIds: string[] = [];
  total: number = 5;

  constructor(
    private router: Router,
    private activeRoute: ActivatedRoute,
    private mapService: MapService,
    private navigateDataService: NavigateDataService,
    private paginationService: PaginationService
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
      PageNumber: 0, ContinuationToken: '', IsResourceCountRequired: false, ResourceIds: []
    }
    this.paginationService.getPagedResources(this.resourceFilter).subscribe(response => {
      if (response != undefined) {
        this.organizations = response["resources"];
        if (this.organizations.length > 3) {
          this.organizations.splice(3, (this.organizations.length) - 1);
        }
      }
    });
  }

  callOrganizations() {
    this.showMoreOrganizations.emit("Organizations");
  }

  ngOnInit() {
    if (sessionStorage.getItem("globalMapLocation")) {
      this.location = JSON.parse(sessionStorage.getItem("globalMapLocation"));
      this.getOrganizations();
    }
    this.subscription = this.mapService.notifyLocation
      .subscribe((value) => {
        this.location = value;
        this.getOrganizations();
      });
  }
  ngOnDestroy() {
    if (this.subscription != undefined) {
      this.subscription.unsubscribe();
    }
  }
}
